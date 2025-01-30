using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QRCodeGenerator : MonoBehaviour
{
    public Toggle publicToggle; // public bushe ya nabushe

    public void GenerateQRCode()
    {
        string uniqueId = System.Guid.NewGuid().ToString();
        string qrCodeUrl = "https://p7z.ir/qrcode/" + uniqueId;
        bool isPublic = publicToggle.isOn; // dastresi baraye hame (hame comment ha ra bekhunan)

        SaveQRCodeToPlayFab(uniqueId, qrCodeUrl, isPublic);
    }

    private void SaveQRCodeToPlayFab(string id, string url, bool isPublic)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "QRCode_" + id, url },
                { "QRCode_Owner_" + id, PlayFabSettings.TitleId }, // sazande qr code
                { "QRCode_Access_" + id, isPublic ? "public" : "private" } // sat dastresi (qr code +remove +change)
                // bayad sath dastresi taqir konad
                // remove ra hazf mikonam ta soo estefade nashavad
            }
        };

        PlayFabClientAPI.UpdateUserData(request, result =>
        {
            Debug.Log("QR Code saved with access level: " + (isPublic ? "Public" : "Private"));
        },
        error => Debug.LogError("Failed to save QR Code: " + error.ErrorMessage));
    }
}