using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class QRCodeCommentsUserRole: MonoBehaviour
{
    private string qrId;
    private string qrOwner;
    private string accessLevel;
    private string currentUserId;

    public void SetQRCodeID(string id)
    {
        qrId = id;
        GetQRCodeAccessLevel();
    }

    private void GetQRCodeAccessLevel()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data.ContainsKey("QRCode_Access_" + qrId))
            {
                accessLevel = result.Data["QRCode_Access_" + qrId].Value;
            }
            if (result.Data.ContainsKey("QRCode_Owner_" + qrId))
            {
                qrOwner = result.Data["QRCode_Owner_" + qrId].Value;
            }

            GetCurrentUserId();
        },
        error => Debug.LogError("Error fetching access level: " + error.ErrorMessage));
    }

    private void GetCurrentUserId()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            currentUserId = result.AccountInfo.PlayFabId;
            FetchComments();
        },
        error => Debug.LogError("Error getting user ID: " + error.ErrorMessage));
    }

    private void FetchComments()
    {
        if (accessLevel == "private" && currentUserId != qrOwner)
        {
            Debug.Log("You do not have permission to view comments for this QR Code.");
            return;
        }

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data.ContainsKey("Comments_" + qrId))
            {
                Debug.Log("Comments: " + result.Data["Comments_" + qrId].Value);
            }
            else
            {
                Debug.Log("No comments available.");
            }
        },
        error => Debug.LogError("Error fetching comments: " + error.ErrorMessage));
    }
}
