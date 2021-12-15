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
        
        public static  List<Vector3> generatePoints(int num, int sphereRad = 10)
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