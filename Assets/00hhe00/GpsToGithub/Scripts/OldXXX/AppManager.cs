using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using UnityEngine.Networking;

public class AppManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Message;

    private GPSLocationCompass gpsLocationCompass;
    void Start()
    {
        Message.text = "Start";

        gpsLocationCompass = GameObject.Find("GPS Location Manager").GetComponent<GPSLocationCompass>();
        gpsLocationCompass.Start_GPS();
        gpsLocationCompass.Start_Compass();
        Message.text = "Invoke";

        Invoke("Get_GPS", 3f);
    }

    private void Get_GPS()
    {
        string myText = "Latitude: " + gpsLocationCompass.Latitude.ToString("0.0000") + "\n" +
        "Longitude: " + gpsLocationCompass.Longitude.ToString("0.0000") + "";

        Message.text = myText;

        File.Create(Application.persistentDataPath + "/GPSData.txt").Close();
        File.WriteAllText(Application.persistentDataPath + "/GPSData.txt", myText);
        Message.text = "File created: " + Application.persistentDataPath + "/GPSData.txt";
        StartCoroutine(UploadFileData());
    }

    IEnumerator UploadFileData()
    {
        Message.text = "Kilroy";
        using (var uwr = new UnityWebRequest("https://github.com/HaraldHeide/Public/raw/main/GPSData.txt", UnityWebRequest.kHttpVerbPUT))
        {
            uwr.uploadHandler = new UploadHandlerFile(Application.persistentDataPath + "/GPSData.txt");
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
                Message.text = "Error: " + uwr.error;
            else
            {
                Message.text = "File sent OK!";
            }
        }
    }


    private void OnDisable()
    {
        gpsLocationCompass.Stopp_GPS();
        gpsLocationCompass.Stopp_Compass();
    }

    //void Update()
    //{
    //    Message.text = "Latitude: " + gpsLocationCompass.Latitude.ToString("0.0000") + "\n" +
    //                    "Longitude: " + gpsLocationCompass.Longitude.ToString("0.0000") + "";
    //}
}