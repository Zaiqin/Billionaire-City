using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dailybonusscript : MonoBehaviour
{
    public GameObject stats;
    public GameObject saveloadsystem;
    public Sprite faded;

    // Start is called before the first frame update
    void Start()
    {
        //print("daily collected is " + stats.GetComponent<Statistics>().noDailyCollected + ", doing day" + int.Parse(this.transform.parent.name.Substring(this.transform.parent.name.Length - 1)));
        if (int.Parse(this.transform.parent.name.Substring(this.transform.parent.name.Length - 1)) > (stats.GetComponent<Statistics>().noDailyCollected + 1))
        {
            this.GetComponent<Button>().image.sprite = faded;
            this.GetComponent<Button>().interactable = false;
            this.transform.parent.GetChild(4).gameObject.SetActive(true);
        }
        else if (int.Parse(this.transform.parent.name.Substring(this.transform.parent.name.Length - 1)) <= (stats.GetComponent<Statistics>().noDailyCollected))
        {
            this.transform.parent.GetChild(2).gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            this.transform.parent.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            if (stats.GetComponent<Statistics>().lastDailyCollected != "")
            {
                var diff = System.DateTime.Now - DateTime.Parse(stats.GetComponent<Statistics>().lastDailyCollected);
                //print("diff is " + diff);
                if (diff.Days < 1)
                {
                    this.GetComponent<Button>().image.sprite = faded;
                    this.GetComponent<Button>().interactable = false;
                    this.transform.parent.GetChild(4).gameObject.SetActive(true);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void collectDaily()
    {
        

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
                stats.GetComponent<Statistics>().updateStats(diffgold: long.Parse(amount));
            }
            else
            {
                print("collect xp");
                string amount = this.transform.parent.GetChild(1).transform.GetChild(1).GetComponent<Text>().text;
                stats.GetComponent<Statistics>().updateStats(diffxp: long.Parse(amount));
            }

            stats.GetComponent<Statistics>().noDailyCollected += 1;
            stats.GetComponent<Statistics>().lastDailyCollected = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            saveloadsystem.GetComponent<saveloadsystem>().saveStats();
        }
    }
}
