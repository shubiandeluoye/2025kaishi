using UnityEngine;
using UnityEngine.Assertions;

namespace Core.Scene.Tests
{
    public class SceneVerification : BaseManager
    {
        protected override void RegisterEvents()
        {
            EventManager.Subscribe<VerificationRequestEvent>(EventNames.VERIFICATION_REQUEST, OnVerificationRequest);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Unsubscribe<VerificationRequestEvent>(EventNames.VERIFICATION_REQUEST, OnVerificationRequest);
        }

        private void OnVerificationRequest(VerificationRequestEvent evt)
        {
            VerifySceneSetup();
        }

        private void VerifySceneSetup()
        {
            try 
            {
                // Verify essential game objects exist
                Assert.IsNotNull(GameObject.FindGameObjectWithTag("Ground"), "Floor is missing from the scene");
                
                GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
                Assert.IsTrue(walls.Length >= 4, "Scene should have at least 4 boundary walls");
                
                GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
                Assert.IsTrue(targets.Length > 0, "Scene should have target objects");

                // Verify physics setup
                foreach (GameObject wall in walls)
                {
                    VerifyWallSetup(wall);
                }

                VerifyFloorSetup(GameObject.FindGameObjectWithTag("Ground"));
                
                foreach (GameObject target in targets)
                {
                    VerifyTargetSetup(target);
                }

                Debug.Log("Scene verification completed successfully!");

                EventManager.Publish(EventNames.VERIFICATION_COMPLETE, 
                    new VerificationCompleteEvent(true, "场景验证成功"));
            }
            catch (System.Exception e)
            {
                EventManager.Publish(EventNames.VERIFICATION_COMPLETE, 
                    new VerificationCompleteEvent(false, e.Message));
            }
        }

        private void VerifyWallSetup(GameObject wall)
        {
            BoxCollider collider = wall.GetComponent<BoxCollider>();
            Assert.IsNotNull(collider, $"Wall {wall.name} is missing BoxCollider");
            Assert.IsFalse(collider.isTrigger, $"Wall {wall.name} collider should not be a trigger");
            Assert.IsTrue(wall.layer == 9, $"Wall {wall.name} should be on layer 9");
        }

        private void VerifyFloorSetup(GameObject floor)
        {
            BoxCollider collider = floor.GetComponent<BoxCollider>();
            Assert.IsNotNull(collider, "Floor is missing BoxCollider");
            Assert.IsFalse(collider.isTrigger, "Floor collider should not be a trigger");
            Assert.IsTrue(floor.layer == 10, "Floor should be on layer 10");
        }

        private void VerifyTargetSetup(GameObject target)
        {
            BoxCollider collider = target.GetComponent<BoxCollider>();
            Assert.IsNotNull(collider, $"Target {target.name} is missing BoxCollider");
            Assert.IsTrue(collider.isTrigger, $"Target {target.name} collider should be a trigger");
            Assert.IsTrue(target.layer == 11, $"Target {target.name} should be on layer 11");
        }
    }
}
