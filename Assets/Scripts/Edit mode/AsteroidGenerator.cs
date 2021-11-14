using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class AsteroidGenerator : MonoBehaviour {
    [SerializeField] public int seed = 0;
    [SerializeField] public List<GameObject> prefabs;

    [SerializeField] public float minDistance = 10f;
    [SerializeField] public float avgDistance = 30f;
    [SerializeField] public float fieldRadius = 150f;

    public void CreateAsteroidField() {
        DestroyAsteroidField();

        Random.InitState(seed);
        float randRange = avgDistance - minDistance;

        for (float x = minDistance - (float) (Math.Ceiling(fieldRadius / avgDistance) * avgDistance); x < fieldRadius; x += avgDistance) {
            for (float y = minDistance - (float) (Math.Ceiling(fieldRadius / avgDistance) * avgDistance); y < fieldRadius; y += avgDistance) {
                for (float z = minDistance - (float) (Math.Ceiling(fieldRadius / avgDistance) * avgDistance);
                    z < fieldRadius;
                    z += avgDistance) {
                    var pos = new Vector3(x, y, z) + new Vector3(Random.value * randRange, Random.value * randRange, Random.value * randRange);
                    if (pos.magnitude <= fieldRadius) {
                        Instantiate(prefabs[Random.Range(0, prefabs.Count)], pos, Random.rotation, transform);
                    }
                }
            }
        }
    }

    public void DestroyAsteroidField() {
        if (Application.isEditor) {
            for (int i = transform.childCount; i > 0; --i)
                DestroyImmediate(this.transform.GetChild(0).gameObject);
        }
        else {
            foreach(GameObject child in transform) {
                Destroy(child);
            }
        }
    } 
}
