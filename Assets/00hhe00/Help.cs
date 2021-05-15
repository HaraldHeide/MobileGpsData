/*
    Gets Longitude Latitude and compass direction from GPS and sends it PunRCP to Random Room app. 
    Feeds Nreal-PointsOfInterest app with GPS data. 


    photonView.RPC("ShareGpsData", RpcTarget.AllBuffered, myGpsData);


    [PunRPC]
    void ShareGpsData(string gpsData)
    {
        GPSData = gpsData;
    }

*/