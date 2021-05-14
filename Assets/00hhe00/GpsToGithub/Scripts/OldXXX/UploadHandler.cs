using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


public class UploadHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Message;

    void Start()
    {
        StartCoroutine(UploadFileData());
    }

    //https://github.com/HaraldHeide/Public/raw/main/GPSData.txt
    IEnumerator UploadFileData()
    {
        yield return new WaitForSeconds(15f);
        Message.text = "Kilroy";
        using (var uwr = new UnityWebRequest("https://github.com/HaraldHeide/Public/raw/main/GPSData.txt", UnityWebRequest.kHttpVerbPUT))
        {
            uwr.uploadHandler = new UploadHandlerFile(Application.persistentDataPath + "/GPSData.txt");
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
                Debug.LogError(uwr.error);
            else
            {
                Debug.LogError("File sent OK!");
            }
        }
    }
    //IEnumerator UploadFileData()
    //{
    //    using (var uwr = new UnityWebRequest("http://yourwebsite.com/upload", UnityWebRequest.kHttpVerbPUT))
    //    {
    //        uwr.uploadHandler = new UploadHandlerFile("/path/to/file");
    //        yield return uwr.SendWebRequest();
    //        if (uwr.result != UnityWebRequest.Result.Success)
    //            Debug.LogError(uwr.error);
    //        else
    //        {
    //            // file data successfully sent
    //        }
    //    }
    //}
}