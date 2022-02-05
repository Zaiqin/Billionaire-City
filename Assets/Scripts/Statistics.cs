using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
public class Statistics : MonoBehaviour
{
    // --- Saved properties ------
    public long money;
    public long gold;
    public long level;
    public long xp;
    public string cityName;
    public int noOfPlots;
    public int noDailyCollected;
    public string lastDailyCollected;
    // ---------------------------

    public long expCost;
    public int wonderBonus;
    public int wonderCommerceBonus;
    public int doubleChance;
    public List<string> builtWonders;
    public List<string> commerceWonders;

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

    public GameObject saveloadobj, shopButton;

    private Dictionary<int, long> levelValues;
    [SerializeField] private GameObject csvObj, levelUpScreen, externalAudioPlayer, inputText;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private AudioClip levelUp;

    [ContextMenu("Whats the Wonder Bonus")]
    public void func()
    {
        print("Wonder House bonus at " + wonderBonus + "%, Commerce Bonus at " + wonderCommerceBonus + "%");
    }

    [ContextMenu("Input Values into Game")]
    public void inputValues()
    {
        updateStats(inputMoney, inputGold, inputXP);
    }

    // Start is called before the first frame update
    void Start()
    {
        levelValues = csvObj.GetComponent<CSVReader>().levelValues;
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

    public void updateStats(long diffmoney = 0, long diffgold = 0, long diffxp = 0)
    {
        money += diffmoney;
        gold += diffgold;
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
        //print("xp is " + xp);
        for (int i = 1; i <= levelValues.Count; i++)
        {
            //print("checking " + levelValues[i] + " where i is " + i);
            if (xp < levelValues[i])
            {
                if (level != (i - 1)){
                    this.GetComponent<levelUp>().calcLevelUp((int)level, i-1);
                    level = i - 1;
                    levelUpScreen.SetActive(true);
                    levelUpScreen.transform.GetChild(3).GetComponent<RecyclableScrollRect>().ReloadData();
                    levelUpScreen.transform.GetChild(0).GetComponent<Text>().text = "Level " + level.ToString();
                    levelUpScreen.transform.GetChild(1).GetComponent<Text>().text = this.GetComponent<levelUp>().noOfCards + " items unlocked";
                    levelUpScreen.transform.GetChild(1).gameObject.SetActive(true);
                    levelUpScreen.transform.GetChild(4).gameObject.SetActive(false);
                    shopButton.GetComponent<shopButton>().requireReload = true;
                    if (this.GetComponent<levelUp>().noOfCards == 0)
                    {
                        levelUpScreen.transform.GetChild(1).gameObject.SetActive(false);
                        levelUpScreen.transform.GetChild(4).gameObject.SetActive(true);
                    }
                    particle.Play();
                    externalAudioPlayer.GetComponent<AudioSource>().PlayOneShot(levelUp);
                } else
                {
                    level = i - 1;
                }
                break;
            }
        }
        levelText.text = level.ToString();
        if (xp >= 100000000) {
            string temp = xp.ToString("#,##0");
            xpText.text = temp.Substring(0, temp.Length - 8) + "M";
        } else {
            xpText.text = xp.ToString("#,##0");
        }
        long nextVal = levelValues[(int)level + 1];
        
        if (level < 2)
        {
            //print("calc less than level 2");
            xpFill.fillAmount = ((float)xp)/(nextVal);
        }
        else
        {
            //print("calc more than level 2");
            long prevVal = levelValues[(int)level];
            xpFill.fillAmount = ((float)xp-prevVal) / (nextVal-prevVal);
        }
        //print("nextVal is " + nextVal + ", current is " + xp + ", start is " + levelValues[(int)level] + ", fillamt is " + xpFill.fillAmount);
        //print("xpFill is " + xpFill.fillAmount + "nextVal is " + nextVal + "xp is " + xp);
        print("Updated stats");
        saveloadobj.GetComponent<saveloadsystem>().saveStats();
    }

    public void updateName()
    {
        cityName = inputText.GetComponent<Text>().text;
    }

    public void setStats(long m, long g, long l, long x)
    {
        money = m;
        gold = g;
        level = l;
        xp = x;
        if (money >= 100000000)
        {
            string temp = money.ToString("#,##0");
            moneyText.text = "$" + temp.Substring(0, temp.Length - 8) + "M";
        }
        else
        {
            moneyText.text = "$" + money.ToString("#,##0");
        }
        if (gold >= 100000000)
        {
            string temp = gold.ToString("#,##0");
            goldText.text = temp.Substring(0, temp.Length - 8) + "M";
        }
        else
        {
            goldText.text = gold.ToString("#,##0");
        }
        levelText.text = level.ToString();
        if (xp >= 100000000)
        {
            string temp = xp.ToString("#,##0");
            xpText.text = temp.Substring(0, temp.Length - 8) + "M";
        }
        else
        {
            xpText.text = xp.ToString("#,##0");
        }
        xpFill.fillAmount = 0.5f;
        print("set stats");
    }
}
