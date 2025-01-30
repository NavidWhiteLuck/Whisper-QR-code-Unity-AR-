using UnityEngine;
using UnityEngine.UI;

public class QRCodeUI : MonoBehaviour
{
    public Button saveButton;
    public Button shareButton;

    void Start()
    {
        saveButton.onClick.AddListener(() => FindObjectOfType<QRCodeSaver>().SaveQRCode());
        shareButton.onClick.AddListener(() => FindObjectOfType<QRCodeSharer>().ShareQRCode());
    }
}
