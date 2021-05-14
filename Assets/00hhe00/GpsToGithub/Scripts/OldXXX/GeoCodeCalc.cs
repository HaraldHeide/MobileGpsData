
using UnityEngine;
using UnityEditor;
using System;


// CALC DISTANCE
public static class GeoCodeCalc
{
    public const double EarthRadiusInMiles = 3956.0;
    public const double EarthRadiusInKilometers = 6367.0;
    public const double EarthRadiusInMetres = 6371000;

    public static double ToRadian(double val) { return val * (Math.PI / 180); }
    public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }

    public static double CalcDistance(double lat1, double lng1, double lat2, double lng2)
    {
        return CalcDistance(lat1, lng1, lat2, lng2, GeoCodeCalcMeasurement.Metre);
    }

    public static double CalcDistance(double lat1, double lng1, double lat2, double lng2, GeoCodeCalcMeasurement m)
    {
        double radius = GeoCodeCalc.EarthRadiusInMiles;

        if (m == GeoCodeCalcMeasurement.Kilometers) { radius = GeoCodeCalc.EarthRadiusInKilometers; }
        if (m == GeoCodeCalcMeasurement.Metre) { radius = GeoCodeCalc.EarthRadiusInMetres; }
        return radius * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0)
            + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
    }

    public static double DistanceZ(double lat1, double lng1, double lat2, double lng2)
    {
        double lngZ = (lng1 + lng2) / 2;
        double radius = GeoCodeCalc.EarthRadiusInMiles;

        radius = GeoCodeCalc.EarthRadiusInMetres * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0)
             + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lngZ, lngZ)) / 2.0), 2.0)))));
        if (lat2 > lat1)
            return radius;
        else
            return -radius;
    }

    public static double DistanceX(double lat1, double lng1, double lat2, double lng2)
    {
        double latX = (lat1 + lat2) / 2;

        double radius = GeoCodeCalc.EarthRadiusInMiles;

        radius = GeoCodeCalc.EarthRadiusInMetres * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(latX, latX)) / 2.0), 2.0)
            + Math.Cos(ToRadian(latX)) * Math.Cos(ToRadian(latX)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
        if (lng1 < lng2)
            return radius;
        else
            return -radius;
    }
}

public enum GeoCodeCalcMeasurement : int
{
    Miles = 0,
    Kilometers = 1,
    Metre = 2
}
