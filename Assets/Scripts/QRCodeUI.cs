using UnityEngine;
using UnityEngine.UI;

public class QRCodeUI : MonoBehaviour
{
    public Button saveButton;
    public Button shareButton;
    // bayad share bishtar ezafe konam halate default android

    void Start()
    {
        saveButton.onClick.AddListener(() => FindObjectOfType<QRCodeSaver>().SaveQRCode());
        shareButton.onClick.AddListener(() => FindObjectOfType<QRCodeSharer>().ShareQRCode());
    } // be play fab motasel shod ***
}
