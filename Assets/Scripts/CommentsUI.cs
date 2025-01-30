using UnityEngine;
using TMPro;
using DG.Tweening;

public class CommentsUI : MonoBehaviour
{
    public GameObject commentsPanel;
    public TextMeshProUGUI commentsText;
    public float animationDuration = 0.3f;

    public void ShowComments(string comments)
    {
        commentsText.text = comments;
        commentsPanel.transform.localScale = Vector3.zero;
        commentsPanel.SetActive(true);
        commentsPanel.transform.DOScale(1, animationDuration).SetEase(Ease.OutBack);
    }

    public void HideComments()
    {
        commentsPanel.transform.DOScale(0, animationDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            commentsPanel.SetActive(false);
        });
    }
}
