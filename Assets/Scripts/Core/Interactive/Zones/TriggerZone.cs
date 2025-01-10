using UnityEngine;
using System;
using System.Collections.Generic;
using Core.Base.Event;
using Core.Interactive.Base;

namespace Core.Interactive.Zones
{
    /// <summary>
    /// Implements a configurable trigger zone that can detect and respond to object entry/exit
    /// Supports different shapes, filtering, and custom event handling
    /// </summary>
    public class TriggerZone : BaseInteractive
    {
        public enum ZoneShape
        {
            Box,
            Sphere,
            Custom
        }

        [Serializable]
        public class ZoneSettings
        {
            public ZoneShape shape = ZoneShape.Box;
            public Vector3 boxSize = Vector3.one;
            public float sphereRadius = 1f;
            public bool useCustomCollider = false;
            public LayerMask targetLayers = -1;
            public string[] requiredTags;
            public bool triggerOnce = false;
            public bool persistTriggerState = false;
            public float cooldownTime = 0f;
        }

        #region Properties
        [SerializeField]
        protected ZoneSettings settings = new ZoneSettings();
        
        protected HashSet<GameObject> objectsInZone = new HashSet<GameObject>();
        protected bool hasTriggered;
        protected float lastTriggerTime;
        
        protected Collider zoneCollider;
        protected List<Collider> customColliders = new List<Collider>();
        #endregion

        #region Events
        public event Action<GameObject> OnObjectEnter;
        public event Action<GameObject> OnObjectExit;
        public event Action<List<GameObject>> OnZoneActivated;
        public event Action OnZoneDeactivated;
        #endregion

        #region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();
            InitializeCollider();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            HandleObjectEnter(other.gameObject);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            HandleObjectExit(other.gameObject);
        }

        protected virtual void OnDrawGizmos()
        {
            // Draw zone shape in editor
            Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.3f);
            switch (settings.shape)
            {
                case ZoneShape.Box:
                    Gizmos.DrawCube(transform.position, settings.boxSize);
                    break;
                case ZoneShape.Sphere:
                    Gizmos.DrawSphere(transform.position, settings.sphereRadius);
                    break;
                case ZoneShape.Custom:
                    // Draw bounds of custom colliders
                    foreach (var collider in customColliders)
                    {
                        if (collider != null)
                        {
                            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Initialization
        protected virtual void InitializeCollider()
        {
            // Remove existing collider if any
            if (zoneCollider != null)
            {
                Destroy(zoneCollider);
            }

            if (settings.useCustomCollider)
            {
                // Use existing colliders
                customColliders.AddRange(GetComponents<Collider>());
                foreach (var collider in customColliders)
                {
                    collider.isTrigger = true;
                }
                return;
            }

            // Create appropriate collider based on shape
            switch (settings.shape)
            {
                case ZoneShape.Box:
                    BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                    boxCollider.size = settings.boxSize;
                    boxCollider.isTrigger = true;
                    zoneCollider = boxCollider;
                    break;

                case ZoneShape.Sphere:
                    SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
                    sphereCollider.radius = settings.sphereRadius;
                    sphereCollider.isTrigger = true;
                    zoneCollider = sphereCollider;
                    break;

                case ZoneShape.Custom:
                    // Custom shape should be set up manually
                    Debug.LogWarning("Custom shape selected but no custom collider setup. Please add colliders manually.");
                    break;
            }
        }
        #endregion

        #region Object Detection
        protected virtual void HandleObjectEnter(GameObject obj)
        {
            if (!CanTrigger(obj)) return;
            
            objectsInZone.Add(obj);
            OnObjectEnter?.Invoke(obj);
            
            // 使用新的事件系统
            EventManager.Publish(EventNames.ZONE_ENTER, 
                new ZoneEnterEvent(this, obj));
            
            CheckZoneActivation();
        }

        protected virtual void HandleObjectExit(GameObject obj)
        {
            if (!objectsInZone.Contains(obj)) return;
            
            objectsInZone.Remove(obj);
            OnObjectExit?.Invoke(obj);
            
            // 使用新的事件系统
            EventManager.Publish(EventNames.ZONE_EXIT, 
                new ZoneExitEvent(this, obj));
            
            if (objectsInZone.Count == 0)
            {
                DeactivateZone();
            }
        }

        protected virtual bool CanTrigger(GameObject obj)
        {
            if (!isInteractable) return false;
            
            if (settings.triggerOnce && hasTriggered) return false;
            
            if (Time.time - lastTriggerTime < settings.cooldownTime) return false;
            
            if ((settings.targetLayers.value & (1 << obj.layer)) == 0) return false;
            
            // Check required tags
            if (settings.requiredTags != null && settings.requiredTags.Length > 0)
            {
                bool hasRequiredTag = false;
                foreach (string tag in settings.requiredTags)
                {
                    if (obj.CompareTag(tag))
                    {
                        hasRequiredTag = true;
                        break;
                    }
                }
                if (!hasRequiredTag) return false;
            }
            
            return true;
        }
        #endregion

        #region Zone Activation
        protected virtual void CheckZoneActivation()
        {
            if (objectsInZone.Count > 0)
            {
                ActivateZone();
            }
        }

        protected virtual void ActivateZone()
        {
            if (settings.triggerOnce && hasTriggered) return;
            
            hasTriggered = true;
            lastTriggerTime = Time.time;
            
            List<GameObject> activeObjects = new List<GameObject>(objectsInZone);
            OnZoneActivated?.Invoke(activeObjects);
            
            // 使用新的事件系统
            EventManager.Publish(EventNames.ZONE_ACTIVATED, 
                new ZoneActivatedEvent(this, activeObjects));
        }

        protected virtual void DeactivateZone()
        {
            if (settings.persistTriggerState) return;
            
            OnZoneDeactivated?.Invoke();
            
            // 使用新的事件系统
            EventManager.Publish(EventNames.ZONE_DEACTIVATED, 
                new ZoneDeactivatedEvent(this));
        }
        #endregion

        #region Configuration
        public virtual void SetZoneShape(ZoneShape shape, Vector3? size = null)
        {
            settings.shape = shape;
            if (size.HasValue)
            {
                switch (shape)
                {
                    case ZoneShape.Box:
                        settings.boxSize = size.Value;
                        break;
                    case ZoneShape.Sphere:
                        settings.sphereRadius = size.Value.x;
                        break;
                }
            }
            InitializeCollider();
        }

        public virtual void SetTargetLayers(LayerMask layers)
        {
            settings.targetLayers = layers;
        }

        public virtual void SetRequiredTags(string[] tags)
        {
            settings.requiredTags = tags;
        }

        public virtual void SetTriggerOnce(bool triggerOnce)
        {
            settings.triggerOnce = triggerOnce;
        }

        public virtual void SetPersistTriggerState(bool persist)
        {
            settings.persistTriggerState = persist;
        }

        public virtual void SetCooldownTime(float time)
        {
            settings.cooldownTime = time;
        }

        public virtual void Reset()
        {
            hasTriggered = false;
            lastTriggerTime = 0f;
            objectsInZone.Clear();
        }
        #endregion

        #region Getters
        public ZoneSettings GetSettings() => settings;
        public bool HasTriggered() => hasTriggered;
        public float GetLastTriggerTime() => lastTriggerTime;
        public List<GameObject> GetObjectsInZone() => new List<GameObject>(objectsInZone);
        public int GetObjectCount() => objectsInZone.Count;
        #endregion
    }
}
