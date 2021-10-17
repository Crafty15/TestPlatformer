using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public static GlobalController Instance;

    public List<Article> articleList = new List<Article>();

    public List<GameObject> pickedItems = new List<GameObject>(); //Picked up item list

    static int level = 0;

    private void Awake() {
        //DontDestroyOnLoad(gameObject);
        if (Instance != null) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //Debug.Log("Picked items 0: " + pickedItems[0]);
    }

    //Generate an article to display based on the game objects collected
    //Prototype uses hard coded messages. Once i can achieve object persistence accross 
    //level specific articles read from json/yaml file?
    //NOTE: Nested switches for different levels
    public static Article GenerateArticle(int numItems) {
        Article news = new Article();
        switch (numItems) {
            case 1:
                news.SetHeadine("Fight between citizens over stolen items");
                news.SetContent("A neighborhood dispute has made the headlines across the city after " +
                    "another day of extremely slow news. The local constable declined to comment, stating " +
                    "that he doesn't care for the tabloids as reading is for dandies. More to come....");
                break;
            case 2:
                news.SetHeadine("Citizens mildly inconvenienced!");
                news.SetContent("The citizens of the city were left mildy disgruntled after several thefts " +
                    "were reported overnight. When asked for comment, a local constable investigating the " +
                    "incidents said that it wasn't him. More to come....");
                break;
            case 3:
                news.SetHeadine("City is crawling with theives and hooligans!");
                news.SetContent("A spate of city wide thefts and disturbances has left citizens and police on high alert. " +
                    "A local constable investigating the incident released a press briefing said that if his " +
                    "lunch is missing he will be right miffed. More to come....");
                break;
            case 4:
                news.SetHeadine("Ol Springheel terrorizes the streets!");
                news.SetContent("The city is in a state of panic as police hunt for a hooded figure who has accosted " +
                    "several citizens and left police baffled. Reports of the figures super human abilites have some rumours " +
                    "spreading that this is some kind of supernatural entity. When asked for comment, a local constable investigating the " +
                    "incident stated \"Crafty bugger stole me lunch, innit?\". More to come.... ");
                break;
            default:
                news.SetHeadine("Man wearing bed linens seen leaving sewer");
                news.SetContent("Citizens witnessed a man wrapped in a soiled bedsheet leaving " +
                    "the sewer today. When asked for comment, the local police department claim it " +
                    "was probably due to the recent curry festival. More to come...");
                break;
        }
        return news;
    }

    public void SetPickedItems(List<GameObject> items) {
        pickedItems = items;
    }

    public List<GameObject> GetPickedItems() {
        return pickedItems;
    }

    //track levels complete
    public static void IncrementLevel() {
        level++;
    }

    public static int GetLevel() {
        return level;
    }

}

//Struct for generating articles
public struct Article {
    string headline;
    string content;

    public void SetHeadine(string hl) {
        headline = hl;
    }

    public void SetContent(string cont) {
        content = cont;
    }

    public string GetHeadLine() {
        return headline;
    }

    public string GetContent() {
        return content;
    }
}






