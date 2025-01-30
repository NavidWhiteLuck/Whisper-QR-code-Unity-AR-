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
    } // id yekta ijad shode ba url site khodam

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
            // user ha bar asase role 
            // sabt dar playfab

            GetCurrentUserId();
        },
        error => Debug.LogError("Error fetching access level: " + error.ErrorMessage));
    }
    // moshkel dar etesal va sabt
    // debug nevisi
    // fix shod
    private void GetCurrentUserId()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            currentUserId = result.AccountInfo.PlayFabId;
            FetchComments();
        },
        error => Debug.LogError("Error getting user ID: " + error.ErrorMessage));
    } // etelaat daryaft shod az playfab

    private void FetchComments()
    {
        if (accessLevel == "private" && currentUserId != qrOwner)
        {
            Debug.Log("Shoma nemitavanid comment hara bebinid!");
            return;
        }
        // dastresi namayesh comment baraye hame
        // toggle --> bayad sakhte beshe
        // baraye namayesh ya adame namayesh
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
