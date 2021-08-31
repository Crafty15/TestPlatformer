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
        
        StolenItems = GlobalController.Instance.getPickedItems();
        Debug.Log("Stolen items 0 = " + StolenItems[0].ToString());
        Debug.Log("List size is: " + StolenItems.Count);
        levelNews = GlobalController.GenerateArticle(StolenItems.Count);
        // Article art = GlobalController.Instance.Ge


        headLine.text = levelNews.getHeadLine();
        articleText.text = levelNews.getContent(); ;

    }


    // Update is called once per frame
/*    void Update()
    {
        
        

    }*/
}
