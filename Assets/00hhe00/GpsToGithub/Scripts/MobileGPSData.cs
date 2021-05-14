using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(GPSLocationCompass))]
public class MobileGPSData : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text Message;

    private GPSLocationCompass gpsLocationCompass => GetComponent<GPSLocationCompass>();

    private string GPSData = "";

    private bool isConnecting = false;

    void Start()
    {
        gpsLocationCompass.Start_GPS();
        gpsLocationCompass.Start_Compass();
        Connect_PUN2();
    }

    #region PUN2
    public void Connect_PUN2()
    {
        isConnecting = true;
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            Message.text = "OnConnectedToMaster";

            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Message.text = "OnJoinRandomFailed() ";

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 20 });
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(Get_GPS());
    }


    IEnumerator Get_GPS()
    {
        string s1, s2, s3, s4, myGpsData;

        while (true)
        {
            s1 = gpsLocationCompass.Latitude.ToString("0.000000");
            s2 = gpsLocationCompass.Longitude.ToString("0.000000");
            s3 = gpsLocationCompass.Altitude.ToString("0.00");
            s4 = gpsLocationCompass.TrueHeading.ToString("0.00");
            Message.text = "Longitude: " + s1 + "\n" + "Latitude: " + s2 + "\n" + "Altitude: " + s3 + "\n" + "Heading: " + s4;
            myGpsData = s1 + ";" + s2 + ";" + s3 + ";" + s4 + ";";
            photonView.RPC("ShareGpsData", RpcTarget.AllBuffered, myGpsData);
            yield return new WaitForSeconds(30f);
        }
    }

    [PunRPC]
    void ShareGpsData(string gpsData)
    {
        GPSData = gpsData;
    }
    #endregion
}