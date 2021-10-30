using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    public static GlobalController Instance;

    public List<Article> articleList = new List<Article>();

    public List<GameObject> pickedItems = new List<GameObject>(); //Picked up item list

    static int level = 0;
    //audio
/*    [SerializeField]
    public AudioClip[] music;
    public AudioSource audioPlayer;*/

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
/*        string name = SceneManager.GetActiveScene().name;
        if (name.Equals("Level1")) {
            audioPlayer.clip = music[0];
            audioPlayer.Play();
        }
        else if (name.Equals("Level2")) {
            audioPlayer.clip = music[1];
            audioPlayer.Play();
        }
        else if (name.Equals("Level3")) {
            audioPlayer.clip = music[3];
            audioPlayer.Play();
        }*/

    }

    //Generate an article to display based on the game objects collected
    //Prototype uses hard coded messages. Once i can achieve object persistence accross 
    //level specific articles read from json/yaml file?
    //NOTE: Nested switches for different levels
    public static Article GenerateArticle(int numItems) {
        Article news = new Article();
        if (level == 1) {
            //city level
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
        }
        else if (level == 2) {
            //cemetary level
            switch (numItems) {
                case 1:
                    news.SetHeadine("Please leave the dead alone");
                    news.SetContent("Police are asking the citizen or citizens who keep helping themselves" +
                        " to grave offerings to please turn themselves in at their nearest convenience. More to come....");
                    break;
                case 2:
                    news.SetHeadine("Cemetary workers threaten union action");
                    news.SetContent("The local grave robbers union released a statement today saying that" +
                        " if their tools keep going missing they will call a city wide strike. When asked for comment, local police" +
                        " claimed they did not speak english. More to come....");
                    break;
                case 3:
                    news.SetHeadine("Cemetary Raid!");
                    news.SetContent("The grave robbers are spooked and relatives of the deceased left horrified after items have been stolen from" +
                        " crypts. \"Someone should do something about this.\" agreed the police chief. More to come....");
                    break;
                case 4:
                    news.SetHeadine("Ol Springheel returns!");
                    news.SetContent("A city wide lockdown has been called as the entity known as Springheel returned a night after the previous" +
                        " scares that sent citizens into a panic and caused police to leave the station.  \"I just hope he doesn't make his way to" +
                        " the foundry.\" said the police chief. More to come.... ");
                    break;
                default:
                    news.SetHeadine("Man wearing bed linens seen jumping about in cemetary");
                    news.SetContent("The man in the bedsheets is back again. Now he has been seen playing silly buggers" +
                        " in the city cemetary and scaring the workers. Police ask that the man please leave the grave robbers in peace. More to come...");
                    break;
            }
        }
        else{
            //foundry level
            switch (numItems) {
                case 1:
                    news.SetHeadine("Foundry investigation!");
                    news.SetContent("Foundry management says workers found to be stealing items will be given" +
                        " a stern talking too. More to come....");
                    break;
                case 2:
                    news.SetHeadine("Foundry workers union action!");
                    news.SetContent("The foundry workers union threatened action after stating that they suspect managers" +
                        " are embezzling funds meant for wages and tools. More to come....");
                    break;
                case 3:
                    news.SetHeadine("Foundry management in hot water!");
                    news.SetContent("The foundry managers are under investigation after wages meant for employees were stolen over night." +
                        " When asked for comment, the police cheif shooed reporters away with a large sack of money. More to come....");
                    break;
                case 4:
                    news.SetHeadine("Springheel frees factory slaves!");
                    news.SetContent("The entity known as Springheel has been hailed a hero by the towns people after causing an uprising" +
                        " that saw workers take over management after the managers mysteriously disapeared. When asked for comment the" +
                        " police Chief said \"They are definitely not in the furnaces.\". More to come.... ");
                    break;
                default:
                    news.SetHeadine("The bedsheet man is just a foundry worker");
                    news.SetContent("Citizens who saw the bedsheet man claim he is just a new foundry employee. \"Probably....\" said the police chief" +
                        ". More to come...");
                    break;
            }
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

    public static void SetLevel(int lastLevel) {
        level = lastLevel;
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






