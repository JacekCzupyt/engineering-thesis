using System;
using System.IO;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Visuals {
    
    [ExecuteInEditMode]
    public class RadialAlphaBlend : MonoBehaviour {
        [SerializeField] private int width = 512;
        [SerializeField] private int height = 512;
        [SerializeField] private float exponent = 2;
        [SerializeField] private float startBlend = 0.8f;
        [SerializeField] private float endBlend = 1.2f;
        [SerializeField] private bool generate = false;

        private void OnValidate() {
            if (generate) {
                generate = false;
                SaveTexture(GenerateTexture());
            }
        }

        private Texture2D GenerateTexture() {
            var mask = new Texture2D(width, height, TextureFormat.RGBA32, true);

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    var distanceModifier = DistanceModifier(x, y);
                    var blend = Mathf.Clamp(Mathf.InverseLerp(startBlend, endBlend, distanceModifier), 0, 1);
                    mask.SetPixel(x, y, new Color(1, 1, 1, blend));
                }
            }

            return mask;
        }

        private void SaveTexture(Texture2D texture) {
            var bytes = texture.EncodeToPNG();
            var dirPath = Application.dataPath + "/Images/";
            if(!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }
            File.WriteAllBytes(dirPath + "DamageOverlay" + ".png", bytes);
        }

        private float DistanceModifier(int x, int y) {
            var viewPoint = new Vector2(((float) x / width) - 0.5f, ((float) y / height) - 0.5f) * 2;
            return Mathf.Pow(Mathf.Abs(viewPoint.x), exponent) + Mathf.Pow(Mathf.Abs(viewPoint.y), exponent);
        }
    }
    
}
