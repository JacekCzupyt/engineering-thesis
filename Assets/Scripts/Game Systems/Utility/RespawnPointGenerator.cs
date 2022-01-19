using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;


namespace Game_Systems.Utility
{

    public static class RespawnPointGenerator
    {
        public static System.Random rnd = new System.Random();
        public static float collideSpawnRange = 0.15f;
        public static float verticalSpawnRange = (float)Math.PI/4; //90 degrees

        public static List<Vector3> generatePoints(int num, float sphereRad = 10f)
        {
            List<Vector3> points = new List<Vector3>();
            if (num == 1)
            {
                points.Add(new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f)));
                return points;
            }
            double phi = Math.PI * (3 - Math.Sqrt(5));
            double y, x, z, r, theta, newRad, norm;
            for (int i = 0; i < num; i++)
            {
                y = 1 - ((float)i / (num - 1)) * 2;
                r = Math.Sqrt(1 - y * y);
                theta = phi * i;
                x = Math.Cos(theta) * r;
                z = Math.Sin(theta) * r;
                norm = Vector3.Magnitude(new Vector3((float)x, (float)y, (float)z));
                newRad = sphereRad / norm;
                points.Add(new Vector3((float)(x*newRad), (float)(y * newRad), (float)(z * newRad)));        
            }
            return points;
        }
        
        public static List<List<Vector3>> GenerateListsOfDividedPoints(int numOfSides, int numOfPoints, float radius)
        {
            List<List<Vector3>> points = new List<List<Vector3>>();

            for(int i = 0; i < numOfSides; i++)
            {
                List<Vector3> pointsPerSide = GeneratePointsPerSide(numOfSides, i, numOfPoints, radius);
                points.Add(pointsPerSide);
            }

            return points;
        }

        public static List<Vector3> GeneratePointsPerSide(int sideCount, int sideNo, int pointsNo, float radius)
        {
            List<Vector3> points = new List<Vector3>();

            float pi = (float)Math.PI;
            float sideDiv = pi/sideCount;

            float minRange = sideNo * sideDiv;
            float maxRange = (sideNo + 1) * sideDiv;

            float gamma;
            double x, y, z;


            foreach(float theta in GenerateRandomUniquePoints(pointsNo, minRange, maxRange))
            {
                //Gamma defines position in the vertical axis of the spawning. 
                //Its set depending on the vertical spawn range variable
                gamma = UnityEngine.Random.Range(-verticalSpawnRange, verticalSpawnRange);

                x = radius * Math.Cos(theta);
                z = radius * Math.Sin(theta);
                y = radius * Math.Cos(gamma);

                points.Add(new Vector3((float)x, (float)y, (float)z));
            }

            return points;
        }

        public static void TestPoints()
        {
            int i = 1;
            foreach(var list in GenerateListsOfDividedPoints(10, 1, 70f))
            {
                Debug.Log("List " + i);
                foreach(var point in list)
                {
                    Debug.Log("x: " + point.x + ", y: " + point.y + ", z: " + point.z);
                }
                i++;
            }
        }

        public static List<float> GenerateRandomUniquePoints(int num, float max, float min)
        {
            List<float> points = new List<float>();

            for(int i = 0; i < num; i++)
            {
                float newValue = GenerateRandomUniquePoint(max, min, points);
                points.Add(newValue);
            }

            return points;
        }

        public static float GenerateRandomUniquePoint(float max, float min, List<float> values)
        {
            float valueToAdd = UnityEngine.Random.Range(min, max);

            foreach(float value in values)
            {
                while(IsInRange(valueToAdd, value, collideSpawnRange))
                {
                    valueToAdd = UnityEngine.Random.Range(min, max);
                }
            }

            return valueToAdd;
        }

        public static bool IsInRange(float valueToCheck, float valueChecking, float range)
        {
            return Math.Abs(valueToCheck - valueChecking) <= range;
        }

        // public static  List<Vector3> generatePoints(int num, int sphereRad = 10)
        // {
        //     List<Vector3> points = new List<Vector3>();
        //     if (num == 1)
        //     {
        //         points.Add(new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f)));
        //         return points;
        //     }
        //     for (int i = 0; i < num; i++) {
        //         points.Add(sphereRad * new Vector3(Mathf.Sin(i*2*Mathf.PI/num), 0, Mathf.Cos(i*2*Mathf.PI/num)));
        //     }
        //     return points;
        // }
    }

}