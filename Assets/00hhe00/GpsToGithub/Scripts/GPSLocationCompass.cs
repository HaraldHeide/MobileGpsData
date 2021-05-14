/*
 * 
 * EVRY Fountain         Latitude: 59.899483   Longitude: 10.627930 
 * EVRY Stairs from park Latitude: 59.899345   Longitude: 10.627859 
 * 
 * Lossless convertion float strng float ???
 *  Youtube ZAC
 *  
 *  Distance
 *  http://wirebeings.com/markerless-gps-ar.html 
 *  
 *  https://www.lynda.com/Unity-tutorials/Calculate-distance-using-GPS/5016722/5036267-4.html
 *  
 *  https://docs.unity3d.com/Manual/DirectionDistanceFromOneObjectToAnother.html
 *  
 *  https://forum.unity.com/threads/how-to-convert-a-compass-bearing-sensor-reading-to-a-direction-mark-n-s-w-e.266465/
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GPSLocationCompass : MonoBehaviour
{
    [SerializeField]
    public float LocationCheckInterval = 0.5f;
    [SerializeField]
    public float CompassCheckInterval = 0.5f;

    public float Longitude;
    public float Latitude;
    public float Altitude;

    public float TrueHeading;
    public float MagneticHeading;

    private bool gpsIsRunning = false;
    private bool compassIsRunning = false;

    void Start()
    {
        Start_GPS();
    }

    public void Start_GPS()
    {
        StartCoroutine(Start_Location(LocationCheckInterval));
    }

    public void Start_Compass()
    {
        StartCoroutine(Start_Compass(LocationCheckInterval));
    }

    public void Stopp_GPS()
    {
        gpsIsRunning = false;
    }

    public void Stopp_Compass()
    {
        compassIsRunning = false;
    }

    IEnumerator Start_Location(float LocationCheckInterval)
    {
        gpsIsRunning = true;
        Input.location.Start();
        while (Input.location.status == LocationServiceStatus.Initializing)
        {
            yield return new WaitForSeconds(0.5f);
        }
        while (gpsIsRunning)
        {
            Longitude = Input.location.lastData.longitude;
            Latitude = Input.location.lastData.latitude;
            Altitude = Input.location.lastData.altitude;
            yield return new WaitForSeconds(LocationCheckInterval);
        }
        Input.location.Stop();
        yield break;
    }

    IEnumerator Start_Compass(float CompassCheckInterval)
    {
        compassIsRunning = true;
        Input.compass.enabled = true;
        while (compassIsRunning)
        {
            TrueHeading = Input.compass.trueHeading;
            MagneticHeading = Input.compass.trueHeading;
            yield return new WaitForSeconds(LocationCheckInterval);
        }
        Input.compass.enabled = false;
        yield break;
    }
}