using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayGameStats : MonoBehaviour
{
    List<GameObject> StolenItems;
    [SerializeField] private Text headLine;
    [SerializeField] private Text articleText;
    //[SerializeField] private Image stolenItem1;
    // Start is called before the first frame update
    void Start()
    {
        StolenItems = GlobalController.Instance.getPickedItems();
        Debug.Log("Stolen items 0 = " + StolenItems[0].ToString());
        Debug.Log("List size is: " + StolenItems.Count);
       // Article art = GlobalController.Instance.Ge


        headLine.text = "Blah blah blah";
        articleText.text = "TEST TEXT";

    }


    // Update is called once per frame
/*    void Update()
    {
        
        

    }*/
}
