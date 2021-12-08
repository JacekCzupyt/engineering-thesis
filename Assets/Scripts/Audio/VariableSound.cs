using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Random = System.Random;

namespace Audio {
    public class VariableSound : MonoBehaviour {
        private AudioSource audioSource;
        [SerializeField] private List<AudioClip> sounds;
        private Random rnd = new Random();

        private void Start() {
            audioSource = GetComponent<AudioSource>();
        }

        public void Play() {
            audioSource.clip = sounds.RandomElement();
            audioSource.Play();
        }

        private void OnDisable() {
            audioSource.Stop();
        }
    }
}
