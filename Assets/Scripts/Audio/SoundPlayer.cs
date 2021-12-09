using UnityEngine;

namespace Audio {
    public abstract  class SoundPlayer : MonoBehaviour {
        public abstract void Play();

        public abstract void Stop();
    }
}
