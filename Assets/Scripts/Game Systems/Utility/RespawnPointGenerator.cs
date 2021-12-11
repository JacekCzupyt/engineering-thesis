using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game_Systems.Utility
{

    public static class RespawnPointGenerator
    {
        public static System.Random rnd = new System.Random();
        public static  List<Vector3> generatePoints(int num, int sphereRad = 50)
        {
            List<Vector3> points = new List<Vector3>();
            double phi = Math.PI * (3 - Math.Sqrt(5));
            double y, x, z, r, theta, newRad, norm;
            for (int i = 0; i < num; i++)
            {
                y = 1 - (i / (num - 1)) * 2;
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
    }

}