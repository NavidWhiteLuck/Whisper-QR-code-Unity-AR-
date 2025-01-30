using UnityEngine;
using TMPro;
using DG.Tweening;

public class CommentsUI : MonoBehaviour
{

    // comment ha hanooz kambud dare
    // Like
    // DisLike
    public GameObject commentsPanel;
    public TextMeshProUGUI commentsText;
    public float animationDuration = 0.3f;
    // animation ra bayad tose bedam
    // unity anim


    public void ShowComments(string comments)
    {
        commentsText.text = comments;
        commentsPanel.transform.localScale = Vector3.zero;
        commentsPanel.SetActive(true);
        commentsPanel.transform.DOScale(1, animationDuration).SetEase(Ease.OutBack);
    }
    // kit ui ezafe konam hal mishe
    // ezafe shod ***

    public void HideComments()
    {
        commentsPanel.transform.DOScale(0, animationDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            commentsPanel.SetActive(false);
        });
        // baad az comment panel Hide beshe
    }
}
