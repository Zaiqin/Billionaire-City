using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelUp : MonoBehaviour
{
    public GameObject csvObj;
    public int noOfCards;
    public List<PropertyCard> cardList = new List<PropertyCard>();

    public void calcLevelUp(int oldLevel, int newLevel)
    {
        noOfCards = 0;
        cardList.Clear();
        //print("oldLevel is " + oldLevel + ", newLevel is " + newLevel);
        //print("calc for level" + newLevel);
        foreach (KeyValuePair<string, PropertyCard> entry in csvObj.GetComponent<CSVReader>().CardDatabase)
        {
            if (newLevel - oldLevel > 1)
            {
                print(newLevel-oldLevel + " Levels Jump");
                for (int i = oldLevel+1; i <= newLevel; i++)
                {
                    //print("checking for level " + i);
                    if (entry.Value.level == i && entry.Value.limited != "ENDED")
                    {
                        print("unlocked " + entry.Value.displayName);
                        cardList.Add(entry.Value);
                        noOfCards += 1;
                    }
                }
                
            } else
            {
                print("1 level incremented");
                if (entry.Value.level == newLevel)
                {
                    print("unlocked " + entry.Value.displayName);
                    cardList.Add(entry.Value);
                    noOfCards += 1;
                }
            }
        }
    }
}
