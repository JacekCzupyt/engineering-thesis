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

        public static List<Vector3> GenerateTeamPoints(int playerCount, int teamCount, int teamId, int sphereRad = 10)
        {
            List<Vector3> points = new List<Vector3>();
            
            for(int i = 0; i < playerCount; i++)
            {
                points.Add(GeneratePointTeam(teamCount, teamId) * sphereRad);
            }
            return points;
        }

        public static Vector3 GeneratePointTeam(int teamCount, int teamId)
        {
            float pi = 2 * (float)Math.PI;
            float divider = pi/teamCount;
            float r, theta, gamma;
            double x, y, z;
            r = UnityEngine.Random.Range(0, 1f);
            theta = UnityEngine.Random.Range((teamId-1)*divider, teamId*divider);
            gamma = UnityEngine.Random.Range(0, pi);
            x = r*Math.Cos(theta);
            z = r*Math.Sin(theta);
            y = r*Math.Cos(gamma);
            return new Vector3((float)x, (float)y, (float)z);
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