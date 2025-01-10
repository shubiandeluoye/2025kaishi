using UnityEngine;
using System;
using System.Collections.Generic;
using Core.Movement.Base;
using Core.Base.Event;

namespace Core.Movement.Types
{
    /// <summary>
    /// Implements path-based movement with support for different path types
    /// (Linear, Curve, Bezier, and Custom paths)
    /// </summary>
    public class PathMovement : BaseMovement
    {
        public enum PathType
        {
            Linear,
            Curve,
            Bezier,
            Custom
        }

        [Serializable]
        public class PathPoint
        {
            public Vector3 position;
            public Vector3 controlPoint1; // For Bezier curves
            public Vector3 controlPoint2; // For Bezier curves
            public float waitTime;
            
            public PathPoint(Vector3 pos, float wait = 0f)
            {
                position = pos;
                waitTime = wait;
            }
        }

        [SerializeField]
        private PathType pathType = PathType.Linear;
        
        [SerializeField]
        private bool loopPath = false;
        
        [SerializeField]
        private bool localSpace = true;
        
        [SerializeField]
        private float pathSpeed = 5f;
        
        [SerializeField]
        private float reachDistance = 0.1f;
        
        private List<PathPoint> pathPoints = new List<PathPoint>();
        private int currentPointIndex;
        private float currentWaitTime;
        private bool isWaiting;
        private bool isMovingForward = true;

        #region Path Management
        public void SetPath(List<PathPoint> points, PathType type)
        {
            pathPoints = points;
            pathType = type;
            ResetPath();
        }

        public void AddPathPoint(Vector3 position, float waitTime = 0f)
        {
            pathPoints.Add(new PathPoint(position, waitTime));
        }

        public void ClearPath()
        {
            pathPoints.Clear();
            ResetPath();
        }

        private void ResetPath()
        {
            currentPointIndex = 0;
            currentWaitTime = 0f;
            isWaiting = false;
        }
        #endregion

        #region Movement Update
        protected override void UpdateMovement()
        {
            if (pathPoints.Count < 2) return;

            if (isWaiting)
            {
                UpdateWaitTime();
                return;
            }

            Vector3 targetPosition = GetNextPosition();
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            
            // Update movement using base class
            SetMoveDirection(moveDirection);
            
            // Check if reached current target
            if (Vector3.Distance(transform.position, targetPosition) < reachDistance)
            {
                OnReachPathPoint();
            }
        }

        private void UpdateWaitTime()
        {
            if (currentWaitTime > 0)
            {
                currentWaitTime -= Time.deltaTime;
                if (currentWaitTime <= 0)
                {
                    isWaiting = false;
                }
            }
        }

        private void OnReachPathPoint()
        {
            PathPoint currentPoint = pathPoints[currentPointIndex];
            
            if (currentPoint.waitTime > 0)
            {
                isWaiting = true;
                currentWaitTime = currentPoint.waitTime;
                SetMoveDirection(Vector3.zero);
            }
            
            UpdatePathIndex();
            
            var data = new EventManager.PathMovementEventData(
                transform.position,
                moveDirection,
                currentVelocity.magnitude,
                isGrounded,
                currentPointIndex,
                pathType,
                loopPath,
                currentPoint.waitTime,
                GetNextPosition()
            );
            EventManager.Publish(EventManager.EventNames.PATH_POINT_REACHED, data);
        }

        private void UpdatePathIndex()
        {
            if (isMovingForward)
            {
                currentPointIndex++;
                if (currentPointIndex >= pathPoints.Count)
                {
                    if (loopPath)
                    {
                        currentPointIndex = 0;
                    }
                    else
                    {
                        currentPointIndex = pathPoints.Count - 1;
                        isMovingForward = false;
                    }
                }
            }
            else
            {
                currentPointIndex--;
                if (currentPointIndex < 0)
                {
                    if (loopPath)
                    {
                        currentPointIndex = pathPoints.Count - 1;
                    }
                    else
                    {
                        currentPointIndex = 0;
                        isMovingForward = true;
                    }
                }
            }
        }
        #endregion

        #region Path Position Calculation
        private Vector3 GetNextPosition()
        {
            switch (pathType)
            {
                case PathType.Linear:
                    return GetLinearPosition();
                case PathType.Curve:
                    return GetCurvePosition();
                case PathType.Bezier:
                    return GetBezierPosition();
                case PathType.Custom:
                    return GetCustomPosition();
                default:
                    return GetLinearPosition();
            }
        }

        private Vector3 GetLinearPosition()
        {
            return pathPoints[currentPointIndex].position;
        }

        private Vector3 GetCurvePosition()
        {
            // Simple curve interpolation using three points
            int prevIndex = Mathf.Max(0, currentPointIndex - 1);
            int nextIndex = Mathf.Min(pathPoints.Count - 1, currentPointIndex + 1);
            
            Vector3 prev = pathPoints[prevIndex].position;
            Vector3 current = pathPoints[currentPointIndex].position;
            Vector3 next = pathPoints[nextIndex].position;
            
            float t = Vector3.Distance(transform.position, current) / 
                     Vector3.Distance(prev, current);
            
            return Vector3.Lerp(
                Vector3.Lerp(prev, current, t),
                Vector3.Lerp(current, next, t),
                t
            );
        }

        private Vector3 GetBezierPosition()
        {
            PathPoint currentPoint = pathPoints[currentPointIndex];
            PathPoint nextPoint = pathPoints[(currentPointIndex + 1) % pathPoints.Count];
            
            float t = Vector3.Distance(transform.position, currentPoint.position) / 
                     Vector3.Distance(currentPoint.position, nextPoint.position);
            
            return CalculateBezierPoint(
                currentPoint.position,
                currentPoint.controlPoint1,
                nextPoint.controlPoint2,
                nextPoint.position,
                t
            );
        }

        private Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float u = 1f - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;
            
            Vector3 point = uuu * p0;
            point += 3f * uu * t * p1;
            point += 3f * u * tt * p2;
            point += ttt * p3;
            
            return point;
        }

        private Vector3 GetCustomPosition()
        {
            // Override this method in derived classes for custom path interpolation
            return GetLinearPosition();
        }
        #endregion

        #region Configuration
        public void SetPathType(PathType type)
        {
            pathType = type;
        }

        public void SetLooping(bool loop)
        {
            loopPath = loop;
        }

        public void SetLocalSpace(bool local)
        {
            localSpace = local;
        }

        public void SetPathSpeed(float speed)
        {
            pathSpeed = speed;
            settings.maxSpeed = speed;
        }

        public void SetReachDistance(float distance)
        {
            reachDistance = distance;
        }
        #endregion

        #region Getters
        public PathType GetPathType() => pathType;
        public bool IsLooping() => loopPath;
        public bool IsLocalSpace() => localSpace;
        public float GetPathSpeed() => pathSpeed;
        public float GetReachDistance() => reachDistance;
        public List<PathPoint> GetPathPoints() => new List<PathPoint>(pathPoints);
        public int GetCurrentPointIndex() => currentPointIndex;
        public bool IsWaiting() => isWaiting;
        #endregion
    }
}
