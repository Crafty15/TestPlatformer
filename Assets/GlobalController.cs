using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public static GlobalController Instance;

    public List<Article> articleList = new List<Article>();

    public List<GameObject> pickedItems = new List<GameObject>(); //Picked up item list


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
    //
/*    private void Update() {
        if (pickedItems.Count <= 0) {
            Debug.Log("List empty");
        }
        else {
            Debug.Log("List size is: "+ pickedItems.Count);
            Debug.Log("List has item: " + pickedItems[0]);
        }
    }*/


    //Generate an article to display based on the game objects collected
    //Prototype uses hard coded messages. Once i can achieve object persistence accross 
    //level specific articles read from json/yaml file?
    public static Article GenerateArticle(int numItems) {
        Article news = new Article();
        switch (numItems) {
            case 1:
                news.setHeadine("Fight between citizens over stolen items");
                news.setContent("A neighborhood dispute has made the headlines across the city after " +
                    "another day of extremely slow news. The local constable declined to comment, stating " +
                    "that he doesn't care for the tabloids as reading is for dandies. More to come....");
                break;
            case 2:
                news.setHeadine("Citizens mildly inconvenienced!");
                news.setContent("The citizens of the city were left mildy disgruntled several thefts " +
                    "were reported overnight. When asked for comment, a local constable investigating the " +
                    "incidents said that it wasn't him. More to come....");
                break;
            case 3:
                news.setHeadine("City is crawling with theives and hooligans!");
                news.setContent("A spate of city wide thefts and disturbances has left citizens and police on high alert. " +
                    "A local constable investigating the incident released a press briefing said that if his " +
                    "lunch is missing he will be right miffed. More to come....");
                break;
            case 4:
                news.setHeadine("Ol Springheel terrorizes the streets!");
                news.setContent("The city is in a state of panic as police hunt for a hooded figure who has accosted " +
                    "several citizens and left police baffled. Reports of the figures super human abilites have some rumours " +
                    "spreading that this is some kind of supernatural entity. When asked for comment, a local constable investigating the " +
                    "incident stated \"Crafty bugger stole me lunch, innit?\". More to come.... ");
                break;
            default:
                news.setHeadine("Man wearing bed linens seen leaving sewer");
                news.setContent("Citizens witnessed a man wrapped in a soiled bedsheet leaving " +
                    "the sewer today. When asked for comment, the local police department claim it " +
                    "was probably due to the recent curry festival. More to come...");
                break;
        }
        return news;
    }

    public void setPickedItems(List<GameObject> l) {
        this.pickedItems = l;
    }

    public List<GameObject> getPickedItems() {
        return this.pickedItems;
    }


}

//Struct for generating articles
public struct Article {
    string headline;
    string content;

    public void setHeadine(string hl) {
        headline = hl;
    }

    public void setContent(string cont) {
        content = cont;
    }

    public string getHeadLine() {
        return headline;
    }

    public string getContent() {
        return content;
    }
}




