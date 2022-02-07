using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorialParent : MonoBehaviour
{
    public int tutorialPage = 0;
    public Text descText;
    public GameObject button, cover, dailyBonus;
    public ParticleSystem particle;

    List<string> tutorialList = new List<string>() {
     "Empty",
     "In Billionaire City, you can buy and rent houses to gain money and experience at the same time as building a beautiful city. \n\nThis is where your Headquarters (HQ) is located in the map",
     "This is the Buy Plots tool. Before you can build a house you need to buy some plots",
     "This is the Shop tool. This is where you can find all the properties you can buy in the game. \n\nThere are many properties from Houses, Commerces, Decorations and Wonders for you to choose from.",
     "This is the Road tool. All Houses, Commerces and Decorations need to be connected to your HQ. \n\nConnect them by using this tool to build roads to you HQ.",
     "This is the Delete tool. Use this to delete Properties in your city. \n\nDo note that you will only get 35% of your money in cash back, not the full refund for destroying your Houses.",
     "This is the Missions tool. This is where you see all the missions that you can work on. \n\nTip: Progress faster in the game by completing these missions and claiming the rewards.",
     "This is the Storage tool. This is where you can see all the items that are in your storage. These items are free to use.",
     "Congratulations for finishing the tutorial! You have now mastered the basics of money making! \n\nGood Luck and all the best!"
    };

    // Start is called before the first frame update
    void Start()
    {
        
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
        } else
        {
            this.gameObject.SetActive(false);
            dailyBonus.SetActive(true);
            cover.SetActive(false);
            particle.Play();
        }
    }
}
