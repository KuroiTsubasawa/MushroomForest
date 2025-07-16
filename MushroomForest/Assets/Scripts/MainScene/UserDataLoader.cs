using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

[System.Serializable]
public class UserData
{
    public int level;
    public int gold;
    public int silver;
    public int exp;
    public int xp_required;
}

[System.Serializable]
public class UserDataResponse
{
    public bool success;
    public string message;
    public UserData data;
}


public class UserDataLoader : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text levelText;
    public TMP_Text goldText;
    public TMP_Text silverText;
    public TMP_Text expText;

    public string getUserDataURL = "http://localhost/projects/MushroomForest/Backend/GetUserData.php";

    void Start()
    {
        string username = PlayerPrefs.GetString("username", "");
        Debug.Log("Pobrany username z PlayerPrefs: " + username);

        if (!string.IsNullOrEmpty(username))
        {
            usernameText.text = username;
            StartCoroutine(AutoRefreshUserData(username));
        }
        else
        {
            Debug.LogError("Brak username w PlayerPrefs");
        }
    }

    IEnumerator LoadUserData(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);

        UnityWebRequest www = UnityWebRequest.Post(getUserDataURL, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Błąd sieci: " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;

            try
            {
                UserDataResponse response = JsonUtility.FromJson<UserDataResponse>(json);
                if (response.success)
                {
                    levelText.text = response.data.level.ToString();
                    goldText.text = response.data.gold.ToString();
                    silverText.text = response.data.silver.ToString();
                    expText.text = $"{response.data.exp}/{response.data.xp_required}";
                }
                else
                {
                    Debug.LogError(response.message);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Błąd parsowania JSON: " + e.Message);
            }
        }
    }

    IEnumerator AutoRefreshUserData(string username)
    {
        while (true)
        {
            yield return StartCoroutine(LoadUserData(username));
            yield return new WaitForSeconds(1f);
        }
    }
}
