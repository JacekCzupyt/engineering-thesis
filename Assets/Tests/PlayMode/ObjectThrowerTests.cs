using System.Collections;
using Game_Systems;
using Game_Systems.Utility;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode {
    public class ObjectThrowerTests
    {
        [UnityTest]
        public IEnumerator ThrowsObject() {
            //var sceneObject = GameObject.Instantiate(new GameObject());
            var sceneObject = new GameObject();
            var sceneManager = sceneObject.AddComponent<SceneManager>();
        
            var so = new SerializedObject(sceneManager);
            so.FindProperty("projectileContainer").objectReferenceValue = new GameObject();
            so.ApplyModifiedProperties();
        
            yield return null;
    
            // sceneManager.ProjectileContainer = new GameObject();
        
            // var gameObject = GameObject.Instantiate(new GameObject());
            var gameObject = new GameObject();
            var prefab = new GameObject();
            prefab.AddComponent<Rigidbody>().useGravity = false;
    
            var thrownObject = ObjectThrower.StaticThrow(gameObject.transform, Vector3.forward, prefab, 1);
    
            yield return null;
        
            Assert.AreEqual(Vector3.forward, thrownObject.GetComponent<Rigidbody>().velocity);
        }
    }
}
