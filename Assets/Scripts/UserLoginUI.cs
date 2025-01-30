using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class UserLoginUI : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public GameObject loginPanel;
    public GameObject mainAppPanel;
    public TextMeshProUGUI welcomeText;

    void Start()
    {
        CheckForExistingUser(); // baresi aggar vojood dasht erja be playfab
    }

    private void CheckForExistingUser()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            string username = result.AccountInfo.TitleInfo.DisplayName;
            if (!string.IsNullOrEmpty(username))
            {
                SetUserInterface(username);
            }
            else
            {
                loginPanel.SetActive(true);
            }
        },
        error => loginPanel.SetActive(true));
    } 
    // login panel dar service google motasel nashod (moshkel ip)
    // bayad fix konam ***

    public void SetUsername()
    {
        string newUsername = usernameInput.text;
        if (string.IsNullOrEmpty(newUsername)) return;

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newUsername
        }, result =>
        {
            SetUserInterface(result.DisplayName);
        },
        error => Debug.LogError("Error setting username: " + error.ErrorMessage));
    }
    // user name motoqayer bushad hata pas az login
    // kami dochare bug mishe dakhele playfab

    private void SetUserInterface(string username)
    {
        welcomeText.text = "Khosh amadid, " + username + "!";
        loginPanel.SetActive(false);
        mainAppPanel.SetActive(true);
    }
    // bayad dakhele unity shakhsi sazi konam (moshkele farsi)
}
