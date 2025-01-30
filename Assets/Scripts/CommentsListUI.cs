// bayad ye fekr behtar vase list comment ha konam

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class CommentsListUI : MonoBehaviour
{
    // bayad yek scroll ezafe konam
    // fix nashod
    public GameObject commentPrefab;
    public Transform commentsContainer;

    public void PopulateComments(List<string> comments)
    {
        foreach (Transform child in commentsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (string comment in comments)
        {
            GameObject newComment = Instantiate(commentPrefab, commentsContainer);
            newComment.GetComponent<TextMeshProUGUI>().text = comment; // jaygozin TMPro (fix shod)
        }
    }
}
