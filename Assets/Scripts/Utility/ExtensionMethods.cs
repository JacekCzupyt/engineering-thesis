using System.Collections.Generic;
using System.Linq;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using Random = System.Random;

namespace Utility {
    public static class ExtensionMethods {
        private static Random rng = new Random();
        
        public static bool In<T>(this T x, params T[] set) {
            return set.Contains(x);
        }
        
        public static bool In<T>(this T x, IEnumerable<T> list) {
            return list.Contains(x);
        }

        public static T RandomElement<T>(this IList<T> list) {
            return list[rng.Next(list.Count)];
        }

        public static int GetRandomInt(int amount){
            return rng.Next(1, amount+1);
        }

        public static float GetHorizontalFov(this Camera cam) {
            var radAngle = cam.fieldOfView * Mathf.Deg2Rad;
            var radHfov = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2) * cam.aspect);
            return Mathf.Rad2Deg * radHfov;
        }

        public static void SetHorizontalFov(this Camera cam, float hFov) {
            var radAngle = hFov * Mathf.Deg2Rad;
            var radHfov = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2) / cam.aspect);
            cam.fieldOfView = Mathf.Rad2Deg * radHfov;
        }

        public static ClientRpcParams OwnerClientParams(this NetworkBehaviour script) {
            return new ClientRpcParams {
                Send = new ClientRpcSendParams {
                    TargetClientIds = NetworkManager.Singleton.ConnectedClientsList.Where(c => c.ClientId == script.OwnerClientId)
                        .Select(c => c.ClientId).ToArray()
                }
            };
        }
        
        public static ClientRpcParams NonOwnerClientParams(this NetworkBehaviour script) {
            return new ClientRpcParams {
                Send = new ClientRpcSendParams {
                    TargetClientIds = NetworkManager.Singleton.ConnectedClientsList.Where(c => c.ClientId != script.OwnerClientId)
                        .Select(c => c.ClientId).ToArray()
                }
            };
        }
    }
}
