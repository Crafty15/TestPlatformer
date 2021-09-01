using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayGameStats : MonoBehaviour
{
    List<GameObject> StolenItems;
    [SerializeField] private Text headLine;
    [SerializeField] private Text articleText;
    private Article levelNews;
    //[SerializeField] private Image stolenItem1;
    // Start is called before the first frame update
    void Start()
    {
        StolenItems = GlobalController.Instance.GetPickedItems();
        if (StolenItems.Count > 0) {
            levelNews = GlobalController.GenerateArticle(StolenItems.Count);
        }
        else {
            levelNews = GlobalController.GenerateArticle(0);
        }

        headLine.text = levelNews.GetHeadLine();
        articleText.text = levelNews.GetContent(); ;

    }

}
