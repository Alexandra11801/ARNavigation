using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locationf
{
    private static float bigSemiAxis = 6378137;
    private static float smallSemiAxis = 6356752.3142f;
    
    public static float DistanceIgnoringHeight(Vector3 coordinates1, Vector3 coordinates2)
    {
        float dLat = DegreesToRadians(coordinates1.x - coordinates2.x) / 2;
        float dLong = DegreesToRadians(coordinates1.z - coordinates2.z) / 2;
        float a = Mathf.Pow(Mathf.Sin(dLat), 2) + 
                  Mathf.Cos(DegreesToRadians(coordinates2.x)) * 
                  Mathf.Cos(DegreesToRadians(coordinates1.x)) * 
                  Mathf.Pow(Mathf.Sin(dLong), 2);
        float r = GetEarthRadius(coordinates2.x);
        float d = 2 * r * Mathf.Asin(Mathf.Sqrt(a));
        return d;
    }
    
    public static float DegreesToRadians(float angle)
    {
        return angle * Mathf.PI / 180;
    }

    public static float GetEarthRadius(float latitude)
    {
        float num = Mathf.Pow(bigSemiAxis * bigSemiAxis * Mathf.Cos(latitude), 2) +
                    Mathf.Pow(smallSemiAxis * smallSemiAxis * Mathf.Sin(latitude), 2);
        float denom = Mathf.Pow(bigSemiAxis * Mathf.Cos(latitude), 2) +
                      Mathf.Pow(smallSemiAxis  * Mathf.Sin(latitude), 2);
        return Mathf.Sqrt(num / denom);
    }
    
    public static float Distance(Vector3 coordinates1, Vector3 coordinates2)
    {
        float dist = DistanceIgnoringHeight(coordinates1, coordinates2);
        float height = Mathf.Abs(coordinates1.y - coordinates2.y);
        return Mathf.Sqrt(dist * dist + height * height);
    }

    public static Vector3 DirectionIgnoringHeight(Vector3 coordinates1, Vector3 coordinates2)
    {
        float x = Distance(new Vector3(coordinates1.x, 0, 0),
            new Vector3(coordinates2.x, 0, 0));
        float z = Distance(new Vector3(0, 0, coordinates1.z),
            new Vector3(0, 0, coordinates2.z));
        return new Vector3(x, 0, z);
    }
    
    public static Vector3 Direction(Vector3 coordinates1, Vector3 coordinates2)
    {
        Vector3 dir = DirectionIgnoringHeight(coordinates1, coordinates2);
        Vector3 upDir = Vector3.up * Mathf.Abs(coordinates1.y - coordinates2.y);
        return dir + upDir;
    }

}
