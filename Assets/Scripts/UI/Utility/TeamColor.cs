using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TeamColor
{
    public static Color GetTeamColor(int teamNumber, float alpha)
    {
        Color c = Color.grey;
        switch(teamNumber){
            case 1: c = Color.red;
            break;
            case 2: c = Color.blue;
            break;
            case 3: c = Color.green;
            break;
            case 4: c = Color.yellow;
            break;
            default: break;
        }
        c.a = alpha;
        return c;
    }
}
