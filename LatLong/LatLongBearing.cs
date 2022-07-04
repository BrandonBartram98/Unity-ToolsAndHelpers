using System;
using UnityEngine;

public class LatLongBearing
{
    public float GetBearing(float lat1, float lon1, float lat2, float lon2)
    {
        double φ1 = lat1 * Math.PI / 180; // φ, λ in radians
        double φ2 = lat2 * Math.PI / 180;
        double λ1 = lon1 * Math.PI / 180;
        double λ2 = lon2 * Math.PI / 180;

        // where φ1,λ1 is the start point, φ2,λ2 the end point (Δλ is the difference in longitude)
        double y = Math.Sin(λ2 - λ1) * Math.Cos(φ2);
        double x = Math.Cos(φ1) * Math.Sin(φ2) -
                    Math.Sin(φ1) * Math.Cos(φ2) * Math.Cos(λ2 - λ1);
        double θ = Math.Atan2(y, x);
        double brng = (θ * 180 / Math.PI + 360) % 360; // in degrees

        return (float)brng;
    }
}
