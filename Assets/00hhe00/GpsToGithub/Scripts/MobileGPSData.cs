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
    [SerializeField]
    private TMP_Text UpdateFrequency;
    [SerializeField]
    private TMP_Text HeadingTara;
    [SerializeField]
    private TMP_Text HeightTara;

    private GPSLocationCompass gpsLocationCompass => GetComponent<GPSLocationCompass>();

    private string GPSData = "";

    private bool isConnecting = false;

    #region UI
    private int uiUpdateFrequence = 30;
    private int uiHeadingTara = 0;
    private int uiHeightTara = 0;

    public void uiUpdateFrequence_Decrease()//Called from UI Button
    {
        uiUpdateFrequence -= 5;
        if (uiUpdateFrequence < 0) uiUpdateFrequence = 0;
        UpdateFrequency.text = uiUpdateFrequence.ToString();
    }
    public void uiUpdateFrequence_Increase()//Called from UI Button
    {
        uiUpdateFrequence += 5;
        UpdateFrequency.text = uiUpdateFrequence.ToString();
    }
    public void uiHeadingTara_Decrease()//Called from UI Button
    {
        uiHeadingTara -= 1;
        HeadingTara.text = uiHeadingTara.ToString();
    }
    public void uiHeadingTara_Increase()//Called from UI Button
    {
        uiHeadingTara += 1;
        HeadingTara.text = uiHeadingTara.ToString();
    }
    public void uiHeightTara_Decrease()//Called from UI Button
    {
        uiHeightTara -= 1;
        HeightTara.text = uiHeightTara.ToString();
    }
    public void uiHeightTara_Increase()//Called from UI Button
    {
        uiHeightTara += 1;
        HeightTara.text = uiHeightTara.ToString();
    }



    #endregion

    void Start()
    {
        gpsLocationCompass.Start_GPS();
        gpsLocationCompass.Start_Compass();
        Connect_PUN2();

        #region Parameters"
        uiUpdateFrequence = 20;
        uiHeadingTara = 0;
        uiHeightTara = 0;

        UpdateFrequency.text = uiUpdateFrequence.ToString();
        HeadingTara.text = uiHeadingTara.ToString();
        HeightTara.text = uiHeightTara.ToString();
        #endregion

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

    #region PUN Callbacks
    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            //Message.text = "OnConnectedToMaster";
            PhotonNetwork.NickName = "GpsDataServer";
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
        //Message.text = "OnJoinRandomFailed() ";

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom("GpsRoom", new RoomOptions { MaxPlayers = 20 });
    }

    public override void OnJoinedRoom()
    {
        //Message.text = "JoinedRoom: " + PhotonNetwork.NickName;
        StartCoroutine(Get_GPS());
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Message.text = "Player " + PhotonNetwork.NickName + " Entered. Players in room: " + PhotonNetwork.CurrentRoom.PlayerCount;
    }
    #endregion

    IEnumerator Get_GPS()
    {
        string s1, s2, s3, s4, myGpsData;

        while (true)
        {
            s1 = gpsLocationCompass.Latitude.ToString("0.000000");
            s2 = gpsLocationCompass.Longitude.ToString("0.000000");

            float height = gpsLocationCompass.Altitude + uiHeightTara;
            s3 = height.ToString("0.00");

            float heading = gpsLocationCompass.TrueHeading + uiHeadingTara;
            s4 = heading.ToString("0.00");

            Message.text = "Longitude: " + s1 + "\n" + "Latitude: " + s2 + "\n" + "Altitude: " + s3 + "\n" + "Heading: " + s4;
            myGpsData = s1 + ";" + s2 + ";" + s3 + ";" + s4 + ";";
            photonView.RPC("ShareGpsData", RpcTarget.All, myGpsData);
            yield return new WaitForSeconds(uiUpdateFrequence);
        }
    }

    [PunRPC]
    void ShareGpsData(string gpsData)
    {
        GPSData = gpsData;
    }
    #endregion
}