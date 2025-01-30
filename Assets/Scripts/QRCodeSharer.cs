using UnityEngine;
using System.IO;

public class QRCodeSharer : MonoBehaviour
{
    public void ShareQRCode()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "QRCode.png");
        if (!File.Exists(filePath))
        {
            Debug.LogError("QR Code file not found!");
            return;
        }

        //new NativeShare().AddFile(filePath)
        //    .SetSubject("My QR Code")
        //    .SetText("Scan this QR Code!")
        //    .Share();

        // moshkel az tarafe google
    }
}
