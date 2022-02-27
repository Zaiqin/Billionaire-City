using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorialParent : MonoBehaviour
{
    public int tutorialPage = 0;
    public Text descText, title;
    public GameObject button, cover, dailyBonus, arrow;
    public GameObject shopToggle, plotToggle, roadToggle, deleteToggle, ronaldToggle, storageToggle, luckToggle, signContractorToggle, moveToggle;
    public ParticleSystem particle;

    List<string> tutorialList = new List<string>() {
     "",
     "In Billionaire City, you can buy and rent houses to gain money and experience at the same time as building a beautiful city. \n\nThis is where your Headquarters (HQ) is located in the map",
     "This is the Buy Plots tool. Before you can build a house you need to buy some plots",
     "This is the Shop tool. This is where you can find all the properties you can buy in the game. \n\nThere are many properties from Houses, Commerces, Decorations and Wonders for you to choose from.",
     "This is the Road tool. All Houses, Commerces and Decorations need to be connected to your HQ. \n\nConnect them by using this tool to build roads to you HQ.",
     "This is the Delete tool. Use this to delete Properties in your city. \n\nDo note that you will only get 35% of your money in cash back, not the full refund for destroying your Houses.",
     "This is the Missions tool. This is where you see all the missions that you can work on. \n\nTip: Progress faster in the game by completing these missions and claiming the rewards.",
     "This is the Storage tool. This is where you can see all the items that are in your storage. These items are free to use.",
     "This is the Lucky Draw panel. You can spin the wheel for 1 Gold each and win various prizes!",
     "This is the Signing Contractor tool. This is where you can sign all the houses in your city in one click. \n\nNote that it takes time to calculate and has a taxation of 20% of the signing cost and 1 Gold",
     "This is the Move tool. \n\nUse this tool to move properties around in your city.",
     "Congratulations for finishing the tutorial! You have now mastered the basics of money making! \n\nGood Luck and all the best!"
    };

    List<string> tutorialListTitle = new List<string>() {
     "",
     "Headquarters",
     "Plots Tool",
     "Shop Tool",
     "Road Tool",
     "Delete Tool",
     "Missions Tool",
     "Storage Tool",
     "Lucky Draw Panel",
     "Contractor Tool",
     "Move Tool",
     "Congratulations!"
    };

    // Start is called before the first frame update
    void Start()
    {
        arrow.transform.position = new Vector3(0, (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0))).y + arrow.GetComponent<BoxCollider2D>().bounds.size.y, 0); arrow.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextPage()
    {
        tutorialPage++;
        print("at page " + tutorialPage + " where there are " + tutorialList.Count + " pages");
        if (tutorialList.Count > tutorialPage)
        {
            descText.text = tutorialList[tutorialPage];
            title.text = tutorialListTitle[tutorialPage];
        } else
        {
            this.gameObject.SetActive(false);
            dailyBonus.SetActive(true);
            cover.SetActive(false);
            
        }
        if (tutorialPage+1 == tutorialList.Count)
        {
            particle.Play();
        }
        if (tutorialPage == 1)
        {
            this.gameObject.transform.position = new Vector3((Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0))).x - this.GetComponent<BoxCollider2D>().bounds.size.x/2, this.gameObject.transform.position.y, 0);
        } else
        {
            this.gameObject.transform.localPosition = new Vector3(0, -20, 0);
        }

        arrow.transform.eulerAngles = new Vector3(0, 0, 0);
        switch (tutorialPage)
        {
            case 1: arrow.transform.position = new Vector3(GameObject.Find("HQ").gameObject.transform.position.x - GameObject.Find("HQ").GetComponent<BoxCollider2D>().bounds.size.x, GameObject.Find("HQ").gameObject.transform.position.y - GameObject.Find("HQ").GetComponent<BoxCollider2D>().bounds.size.y/2, 0); arrow.transform.eulerAngles = new Vector3(0, 180, 0); break;
            case 2: arrow.transform.position = new Vector3(plotToggle.transform.position.x + arrow.GetComponent<BoxCollider2D>().bounds.size.x + plotToggle.GetComponent<CircleCollider2D>().bounds.size.x/4, plotToggle.transform.position.y, 0); break;
            case 3: arrow.transform.position = new Vector3(shopToggle.transform.position.x + arrow.GetComponent<BoxCollider2D>().bounds.size.x + plotToggle.GetComponent<CircleCollider2D>().bounds.size.x / 4, shopToggle.transform.position.y, 0); break;
            case 4: arrow.transform.position = new Vector3(roadToggle.transform.position.x + arrow.GetComponent<BoxCollider2D>().bounds.size.x + plotToggle.GetComponent<CircleCollider2D>().bounds.size.x / 4, roadToggle.transform.position.y, 0); break;
            case 5: arrow.transform.position = new Vector3(deleteToggle.transform.position.x + arrow.GetComponent<BoxCollider2D>().bounds.size.x + plotToggle.GetComponent<CircleCollider2D>().bounds.size.x / 4, deleteToggle.transform.position.y, 0); break;
            case 6: arrow.transform.position = new Vector3(ronaldToggle.transform.position.x + arrow.GetComponent<BoxCollider2D>().bounds.size.x + plotToggle.GetComponent<CircleCollider2D>().bounds.size.x / 4, ronaldToggle.transform.position.y, 0); break;
            case 7: arrow.transform.position = new Vector3(storageToggle.transform.position.x - arrow.GetComponent<BoxCollider2D>().bounds.size.x - plotToggle.GetComponent<CircleCollider2D>().bounds.size.x / 4, storageToggle.transform.position.y, 0);
                    arrow.transform.eulerAngles = new Vector3(0, 180, 0); break;
            case 8: arrow.transform.position = new Vector3(luckToggle.transform.position.x - arrow.GetComponent<BoxCollider2D>().bounds.size.x - plotToggle.GetComponent<CircleCollider2D>().bounds.size.x / 4, luckToggle.transform.position.y, 0);
                    arrow.transform.eulerAngles = new Vector3(0, 180, 0); break;
            case 9: arrow.transform.position = new Vector3(signContractorToggle.transform.position.x - arrow.GetComponent<BoxCollider2D>().bounds.size.x - plotToggle.GetComponent<CircleCollider2D>().bounds.size.x / 4, signContractorToggle.transform.position.y, 0);
                    arrow.transform.eulerAngles = new Vector3(0, 180, 0); break;
            case 10: arrow.transform.position = new Vector3(moveToggle.transform.position.x - arrow.GetComponent<BoxCollider2D>().bounds.size.x - plotToggle.GetComponent<CircleCollider2D>().bounds.size.x / 4, moveToggle.transform.position.y, 0);
                     arrow.transform.eulerAngles = new Vector3(0, 180, 0); break;
            default: arrow.transform.position = new Vector3(0, (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0))).y + arrow.GetComponent<BoxCollider2D>().bounds.size.y, 0); arrow.transform.eulerAngles = new Vector3(0, 0, 0); break;
        }
    }
}
