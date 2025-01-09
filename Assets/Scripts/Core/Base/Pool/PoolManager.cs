using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Base.Pool
{
    /// <summary>
    /// Manages multiple object pools for efficient object reuse
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        #region Singleton
        private static PoolManager instance;
        public static PoolManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<PoolManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("PoolManager");
                        instance = go.AddComponent<PoolManager>();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Pool Configuration
        [Serializable]
        public class PoolConfig
        {
            public string poolId;
            public GameObject prefab;
            public int initialSize;
            public bool expandable;
            public int maxSize;
        }

        [SerializeField]
        private List<PoolConfig> poolConfigs = new List<PoolConfig>();
        #endregion

        #region Private Fields
        private Transform poolContainer;
        private Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();
        private Dictionary<string, HashSet<GameObject>> activeObjects = new Dictionary<string, HashSet<GameObject>>();
        private Dictionary<string, Queue<GameObject>> pooledObjects = new Dictionary<string, Queue<GameObject>>();
        #endregion

        #region Pool Management
        public event Action<string, GameObject> OnObjectSpawned;
        public event Action<string, GameObject> OnObjectDespawned;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        #endregion

        #region Initialization
        private void Initialize()
        {
            poolContainer = new GameObject("PoolContainer").transform;
            poolContainer.SetParent(transform);

            foreach (var config in poolConfigs)
            {
                CreatePool(config);
            }
        }

        private void CreatePool(PoolConfig config)
        {
            if (pools.ContainsKey(config.poolId))
            {
                Debug.LogWarning($"Pool with ID {config.poolId} already exists!");
                return;
            }

            GameObject container = new GameObject($"Pool_{config.poolId}");
            container.transform.SetParent(poolContainer);

            ObjectPool pool = new ObjectPool(
                config.prefab,
                container.transform,
                config.initialSize,
                config.expandable,
                config.maxSize
            );

            pools.Add(config.poolId, pool);
        }
        #endregion

        #region Pool Operations
        public GameObject Spawn(string poolId, Vector3 position, Quaternion rotation)
        {
            if (!pools.TryGetValue(poolId, out ObjectPool pool))
            {
                Debug.LogError($"Pool with ID {poolId} not found!");
                return null;
            }

            GameObject obj = pool.Get();
            if (obj != null)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                OnObjectSpawned?.Invoke(poolId, obj);
            }
            return obj;
        }

        public void Despawn(string poolId, GameObject obj)
        {
            if (!pools.TryGetValue(poolId, out ObjectPool pool))
            {
                Debug.LogError($"Pool with ID {poolId} not found!");
                return;
            }

            pool.Release(obj);
            OnObjectDespawned?.Invoke(poolId, obj);
        }

        public void DespawnAll(string poolId)
        {
            if (!pools.TryGetValue(poolId, out ObjectPool pool))
            {
                Debug.LogError($"Pool with ID {poolId} not found!");
                return;
            }

            pool.ReleaseAll();
        }

        public void DespawnAllPools()
        {
            foreach (var pool in pools.Values)
            {
                pool.ReleaseAll();
            }
        }
        #endregion

        #region Pool Information
        public int GetActiveCount(string poolId)
        {
            return pools.TryGetValue(poolId, out ObjectPool pool) ? pool.ActiveCount : 0;
        }

        public int GetInactiveCount(string poolId)
        {
            return pools.TryGetValue(poolId, out ObjectPool pool) ? pool.InactiveCount : 0;
        }

        public bool HasPool(string poolId)
        {
            return pools.ContainsKey(poolId);
        }
        #endregion

        private GameObject[] GetActiveObjects()
        {
            return activeObjects.Values.SelectMany(set => set).ToArray();
        }
    }

    /// <summary>
    /// Individual object pool implementation
    /// </summary>
    public class ObjectPool
    {
        private GameObject prefab;
        private Transform container;
        private Stack<GameObject> inactiveObjects;
        private HashSet<GameObject> activeObjects;
        private bool expandable;
        private int maxSize;

        public int ActiveCount => activeObjects.Count;
        public int InactiveCount => inactiveObjects.Count;

        public ObjectPool(GameObject prefab, Transform container, int initialSize, bool expandable, int maxSize)
        {
            this.prefab = prefab;
            this.container = container;
            this.expandable = expandable;
            this.maxSize = maxSize;

            inactiveObjects = new Stack<GameObject>(initialSize);
            activeObjects = new HashSet<GameObject>();

            for (int i = 0; i < initialSize; i++)
            {
                CreateObject();
            }
        }

        private GameObject CreateObject()
        {
            GameObject obj = GameObject.Instantiate(prefab, container);
            obj.SetActive(false);
            inactiveObjects.Push(obj);
            return obj;
        }

        public GameObject Get()
        {
            GameObject obj = null;

            if (inactiveObjects.Count > 0)
            {
                obj = inactiveObjects.Pop();
            }
            else if (expandable && (activeObjects.Count + inactiveObjects.Count) < maxSize)
            {
                obj = CreateObject();
                inactiveObjects.Pop(); // Remove from inactive since we just created it
            }

            if (obj != null)
            {
                activeObjects.Add(obj);
            }

            return obj;
        }

        public void Release(GameObject obj)
        {
            if (activeObjects.Remove(obj))
            {
                obj.SetActive(false);
                obj.transform.SetParent(container);
                inactiveObjects.Push(obj);
            }
        }

        public void ReleaseAll()
        {
            foreach (var obj in activeObjects.ToArray())
            {
                Release(obj);
            }
        }
    }
}
