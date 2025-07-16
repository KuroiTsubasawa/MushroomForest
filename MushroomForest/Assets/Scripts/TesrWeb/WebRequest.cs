using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetData("http://localhost/projects/MushroomForest/Backend/GetDate.php"));
    }

    IEnumerator GetData(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError ||
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Received: " + www.downloadHandler.text);
        }
    }
}
