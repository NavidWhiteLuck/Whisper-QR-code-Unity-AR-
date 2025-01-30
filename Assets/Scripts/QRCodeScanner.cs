using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class QRCodeScanner : MonoBehaviour
{
    public RawImage cameraFeed;
    public Text resultText;
    private WebCamTexture camTexture;
    private BarcodeReader reader;

    void Start()
    {
        reader = new BarcodeReader();
        camTexture = new WebCamTexture();
        cameraFeed.texture = camTexture;
        camTexture.Play();
    }

    void Update()
    {
        ScanQRCode();
    }

    private void ScanQRCode()
    {
        if (camTexture.width > 100)
        {
            var pixels = camTexture.GetPixels32();
            var result = reader.Decode(pixels, camTexture.width, camTexture.height);

            if (result != null)
            {
                camTexture.Stop();
                resultText.text = "QR Code Found: " + result.Text;
                string qrId = ExtractQRCodeID(result.Text);
                if (!string.IsNullOrEmpty(qrId))
                {
                    FetchQRCodeData(qrId);
                }
            }
        }
    }

    private string ExtractQRCodeID(string url)
    {
        if (url.StartsWith("https://p7z.ir/qrcode/"))
        {
            return url.Replace("https://p7z.ir/qrcode/", "");
        }
        return null;
    }

    private void FetchQRCodeData(string qrId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("QRCode_" + qrId))
            {
                string qrInfo = result.Data["QRCode_" + qrId].Value;
                resultText.text = "QR Code Data: " + qrInfo;
                FetchComments(qrId);
            }
            else
            {
                resultText.text = "QR Code Not Found!";
            }
        },
        error => resultText.text = "Error fetching QR Code: " + error.ErrorMessage);
    }

    private void FetchComments(string qrId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("Comments_" + qrId))
            {
                resultText.text += "\nComments:\n" + result.Data["Comments_" + qrId].Value;
            }
            else
            {
                resultText.text += "\nNo Comments Yet!";
            }
        },
        error => resultText.text += "\nError fetching comments: " + error.ErrorMessage);
    }
}
