using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class AuthManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_Text messageText;

    public GameObject emailField;
    public GameObject confirmPasswordField;

    private bool isRegisterMode = false;

    private string registerURL = "http://localhost/projects/MushroomForest/Backend/Register.php";
    private string loginURL = "http://localhost/projects/MushroomForest/Backend/Login.php";
    

    public void Start()
    {
        SwitchToLogin();
    }
    public void SwitchToRegister()
    {
        isRegisterMode = true;
        emailField.SetActive(true);
        confirmPasswordField.SetActive(true);
        messageText.text = "Register";
    }

    public void SwitchToLogin()
    {
        isRegisterMode = false;
        emailField.SetActive(false);
        confirmPasswordField.SetActive(false);
        messageText.text = "Log in";
    }

    public void Submit()
    {
        if (isRegisterMode)
        {
            if (passwordInput.text != confirmPasswordInput.text)
            {
                messageText.text = "Hasła nie są takie same.";
                return;
            }
            StartCoroutine(RegisterCoroutine());
        }
        else
        {

            StartCoroutine(LoginCoroutine());
            PlayerPrefs.SetString("username", usernameInput.text);
        }
    }

    IEnumerator RegisterCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameInput.text);
        form.AddField("email", emailInput.text);
        form.AddField("password", passwordInput.text);

        UnityWebRequest www = UnityWebRequest.Post(registerURL, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            messageText.text = "Błąd: " + www.error;
        else
        {
            ServerResponse response = JsonUtility.FromJson<ServerResponse>(www.downloadHandler.text);
            messageText.text = response.message;
        }
    }

    IEnumerator LoginCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameInput.text);
        form.AddField("password", passwordInput.text);

        UnityWebRequest www = UnityWebRequest.Post(loginURL, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            messageText.text = "Błąd: " + www.error;
        else
        {
            ServerResponse response = JsonUtility.FromJson<ServerResponse>(www.downloadHandler.text);
            messageText.text = response.message;
            if (response.success)
            {
                yield return new WaitForSeconds(1f);
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
            }
        }


    }
}
