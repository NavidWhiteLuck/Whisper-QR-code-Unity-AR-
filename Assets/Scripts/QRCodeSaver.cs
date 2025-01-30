using UnityEngine;
using System.IO;
using System.Collections;

public class QRCodeSaver : MonoBehaviour
{
    public GameObject qrCodeImage; // qr code dar ui 

    public void SaveQRCode()
    {
        StartCoroutine(CaptureAndSave());
    }

    private IEnumerator CaptureAndSave()
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(256, 256, TextureFormat.RGB24, false);
        Rect rect = new Rect(0, 0, 256, 256);
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        string path = Path.Combine(Application.persistentDataPath, "QRCode.png");
        File.WriteAllBytes(path, bytes);

        Debug.Log("QR Code saved to: " + path);
    }
}
