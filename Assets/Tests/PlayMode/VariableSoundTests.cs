using System.Collections;
using System.Collections.Generic;
using Audio;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode {
    public class VariableSoundTests {
        private VariableSound script;
        private AudioSource sound;
        private GameObject obj;

        [UnitySetUp]
        public IEnumerator Setup() {
            obj = new GameObject();
            script = obj.AddComponent<VariableSound>();
            sound = obj.AddComponent<AudioSource>();

            script.sounds = new List<AudioClip> {Resources.Load("Tests/Gun1") as AudioClip};

            yield return null;
        }
        
        [UnityTest]
        public IEnumerator SoundIsPlaying() {
            script.Play();
            yield return null;
            Assert.AreSame(sound.clip, script.sounds[0]);
            Assert.IsTrue(sound.isPlaying);
        }
        
        [UnityTest]
        public IEnumerator SoundIsStopped() {
            script.Play();
            yield return null;
            script.Stop();
            yield return null;
            Assert.AreSame(sound.clip, script.sounds[0]);
            Assert.IsFalse(sound.isPlaying);
        }

        [UnityTearDown]
        public IEnumerator TearDown() {
            Object.Destroy(obj);
            yield return null;
        }
    }
}
