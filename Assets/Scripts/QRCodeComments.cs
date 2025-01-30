// role pak kardan comment shakhsi bazi oqat bug dara (bayad fix konam)
// dasti nabayad playfab storage sakhte beshe
//Api amal nakarde
// bug dare

using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class QRCodeComments : MonoBehaviour
{
    public InputField commentInput;
    public Text commentSection;
    private string qrId;
    private string currentUsername;

    void Start()
    {
        GetCurrentUsername();
    }

    private void GetCurrentUsername()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            currentUsername = result.AccountInfo.TitleInfo.DisplayName;
        },
        error => Debug.LogError("Error getting username: " + error.ErrorMessage));
    }

    public void SetQRCodeID(string id)
    {
        qrId = id;
        FetchComments();
    }

    public void SubmitComment()
    {
        if (string.IsNullOrEmpty(qrId) || string.IsNullOrEmpty(commentInput.text))
        {
            Debug.LogError("Invalid QR ID or Empty Comment!");
            return;
        }

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            string existingComments = result.Data.ContainsKey("Comments_" + qrId) ? result.Data["Comments_" + qrId].Value : "";
            string newComment = currentUsername + ": " + commentInput.text + "\n";
            string updatedComments = existingComments + newComment;

            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> { { "Comments_" + qrId, updatedComments } }
            };

            PlayFabClientAPI.UpdateUserData(request, res =>
            {
                Debug.Log("Comment Saved!");
                FetchComments();
            },
            error => Debug.LogError("Error saving comment: " + error.ErrorMessage));
        },
        error => Debug.LogError("Error fetching comments: " + error.ErrorMessage));
    }
    // fix shod vali dar payam ba tedad bala bug dasht
    // bayad fix konam ***
    private void FetchComments()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("Comments_" + qrId))
            {
                commentSection.text = result.Data["Comments_" + qrId].Value;
            }
            else
            {
                commentSection.text = "No comments yet!";
            }
        },
        error => Debug.LogError("Error fetching comments: " + error.ErrorMessage));
    }

    public void DeleteMyComment()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("Comments_" + qrId))
            {
                string allComments = result.Data["Comments_" + qrId].Value;
                string[] commentList = allComments.Split('\n');
                List<string> updatedComments = new List<string>();

                foreach (string comment in commentList)
                {
                    if (!comment.StartsWith(currentUsername + ":"))
                    {
                        updatedComments.Add(comment);
                    }
                }

                string newCommentData = string.Join("\n", updatedComments);

                var request = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string> { { "Comments_" + qrId, newCommentData } }
                };

                PlayFabClientAPI.UpdateUserData(request, res =>
                {
                    Debug.Log("Comment Deleted!");
                    FetchComments();
                },
                error => Debug.LogError("Error deleting comment: " + error.ErrorMessage));
            }
        }, // etesal be dastresi mojood baraye pak kardan comment
        // faqat karbar sade betune comment khodesho pak kone
        // fix shod ***
        error => Debug.LogError("Error fetching comments: " + error.ErrorMessage));
    }
}