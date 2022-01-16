using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelUp : MonoBehaviour
{
    public GameObject csvObj;
    public int noOfCards;
    public List<Sprite> spriteList = new List<Sprite>();
    public 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void calcLevelUp(int oldLevel, int newLevel)
    {
        noOfCards = 0;
        spriteList.Clear();
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
                    if (entry.Value.level == i)
                    {
                        print("unlocked " + entry.Value.displayName);
                        spriteList.Add(entry.Value.bgImage);
                        noOfCards += 1;
                    }
                }
                
            } else
            {
                print("1 level incremented");
                if (entry.Value.level == newLevel)
                {
                    print("unlocked " + entry.Value.displayName);
                    spriteList.Add(entry.Value.bgImage);
                    noOfCards += 1;
                }
            }
        }
    }
}
