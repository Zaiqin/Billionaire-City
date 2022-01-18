using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dailybonusscript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void collectDaily()
    {
        GameObject stats = GameObject.Find("Stats");

        if (this.transform.parent.GetChild(2).gameObject.activeSelf == true)
        {
            if (this.transform.parent.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite.name == "moneySprite")
            {
                print("collectmoney");
                string amount = this.transform.parent.GetChild(1).transform.GetChild(1).GetComponent<Text>().text;
                stats.GetComponent<Statistics>().updateStats(diffmoney: long.Parse(amount.Substring(1)));

            }
            else if (this.transform.parent.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite.name == "goldSprite")
            {
                print("collect gold");
                string amount = this.transform.parent.GetChild(1).transform.GetChild(1).GetComponent<Text>().text;
                stats.GetComponent<Statistics>().updateStats(diffgold: long.Parse(amount.Substring(1)));
            }
            else
            {
                print("collect xp");
                string amount = this.transform.parent.GetChild(1).transform.GetChild(1).GetComponent<Text>().text;
                stats.GetComponent<Statistics>().updateStats(diffxp: long.Parse(amount.Substring(1)));
            }
        }
    }
}
