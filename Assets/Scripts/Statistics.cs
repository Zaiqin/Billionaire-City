using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    public long money;
    public long gold;
    public long level;
    public long xp;

    public Text moneyText;
    public Text goldText;
    public Text levelText;
    public Text xpText;

    public Image xpFill;

    [Header("Input Values into Game:")]
    // for inputting into inspector
    public long inputMoney;
    public long inputGold;
    public long inputLevel;
    public long inputXP;

    
    [ContextMenu("Input Values into Game")]
    public void inputValues()
    {
        updateStats(inputMoney, inputGold, inputLevel, inputXP);
    }

    // Start is called before the first frame update
    void Start()
    {
        updateStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public long[] returnStats()
    {
        long[] stats = new long[4] { money, gold, level, xp };
        //print("in stats script, level is" + level);
        return stats;
    }

    public void updateStats(long diffmoney = 0, long diffgold = 0, long difflevel = 0, long diffxp = 0)
    {
        money += diffmoney;
        gold += diffgold;
        level += difflevel;
        xp += diffxp;
        if (money >= 100000000) {
            string temp = money.ToString("#,##0");
            moneyText.text = "$" + temp.Substring(0, temp.Length - 8) + "M";
        } else {
            moneyText.text = "$" + money.ToString("#,##0");
        }
        if (gold >= 100000000) {
            string temp = gold.ToString("#,##0");
            goldText.text = temp.Substring(0, temp.Length - 8) + "M";
        } else {
            goldText.text = gold.ToString("#,##0");
        }
        levelText.text = level.ToString();
        if (xp >= 100000000) {
            string temp = xp.ToString("#,##0");
            xpText.text = temp.Substring(0, temp.Length - 8) + "M";
        } else {
            xpText.text = xp.ToString("#,##0");
        }
        xpFill.fillAmount = 0.5f;
        print("updated stats");
    }
}
