using System.Collections;
using Game_Systems;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode {
    public class CheckDistanceTests {
        private GameObject obj;
        private GameObject ui;
        private CheckDistance checkDistance;
        
        [UnitySetUp]
        public IEnumerator Setup() {
            obj = new GameObject();
            ui = new GameObject();
            ui.SetActive(false);
            checkDistance = obj.AddComponent<CheckDistance>();

            checkDistance.warningUI = ui;
            
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator NoWarningWhenInsideRadius() {
            obj.transform.position = Vector3.zero;

            yield return null;
            
            Assert.IsFalse(ui.activeSelf);
        }
        
        [UnityTest]
        public IEnumerator WarningWhenNotInsideRadius() {
            obj.transform.position = Vector3.up * 200;

            yield return null;
            
            Assert.IsTrue(ui.activeSelf);
        }

        [UnityTearDown]
        public IEnumerator TearDown() {
            Object.Destroy(obj);
            Object.Destroy(ui);
            yield return null;
        }
    }
}
