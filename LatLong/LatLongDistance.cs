using System;
using UnityEngine;

public class LatLongDistance
{
    public float GetDistance(float lat1, float lon1, float lat2, float lon2)
    {
        // φ is latitude, λ is longitude, R is earth’s radius (mean radius = 6,371km)
        double R = 6371e3; // metres
        double φ1 = lat1 * Math.PI / 180; // φ, λ in radians
        double φ2 = lat2 * Math.PI / 180;
        double Δφ = (lat2 - lat1) * Math.PI / 180;
        double Δλ = (lon2 - lon1) * Math.PI / 180;

        double a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double d = R * c; // in metres

        return (float)d;
    }
}
