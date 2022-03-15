using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using System;

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
    public bool muted;
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

    public GameObject saveloadobj, shopButton, HQ, missionsPanel;

    private Dictionary<int, long> levelValues;
    [SerializeField] private GameObject csvObj, levelUpScreen, externalAudioPlayer, inputText, muteSwitch;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private AudioClip levelUp;

    public List<Mission> typeZero = new List<Mission>();

    [ContextMenu("Input Values into Game")]
    public void inputValues()
    {
        updateStats(inputMoney, inputGold, inputXP);
    }

    // Start is called before the first frame update
    public void initStats()
    {
        levelValues = csvObj.GetComponent<CSVReader>().levelValues;
        print("init stats");
        if (muted == true)
        {
            muteSwitch.GetComponent<Toggle>().isOn = true;
            Camera.main.GetComponent<AudioSource>().volume = 0f;
            externalAudioPlayer.GetComponent<AudioSource>().volume = 0f;
        }
        updateStats();
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
                    levelUpScreen.transform.localScale = Vector2.zero;
                    levelUpScreen.transform.LeanScale(new Vector2(68f,68f), 0.2f).setEaseOutBack();
                    levelUpScreen.transform.GetChild(3).GetComponent<RecyclableScrollRect>().ReloadData();
                    levelUpScreen.transform.GetChild(0).GetComponent<Text>().text = "Level " + level.ToString();
                    if (this.GetComponent<levelUp>().noOfCards == 1)
                    {
                        levelUpScreen.transform.GetChild(1).GetComponent<Text>().text = "1 item unlocked";
                    }
                    else
                    {
                        levelUpScreen.transform.GetChild(1).GetComponent<Text>().text = this.GetComponent<levelUp>().noOfCards + " items unlocked";
                    }
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
        xpText.text = xp.ToString("#,##0");
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

        // Type 0 Missions --------------
        if (missionsPanel.GetComponent<missionParent>().missionList != null)
        {
            foreach (var mission in typeZero)
            {
                print("cheching " + mission.msnName +"which type " + mission.msnType);
                if (mission.msnType == (float)0.1 && mission.msnPending == false)
                {
                    print("checking type 0.1 with meta " + long.Parse(mission.msnMetadata));
                    HQ.GetComponent<HQstats>().calcHQ();
                    if (HQ.GetComponent<HQstats>().totalLong >= long.Parse(mission.msnMetadata))
                    {
                        missionsPanel.GetComponent<missionParent>().completeMission(mission);
                    }
                }
                if (mission.msnType == (float)0.2 && money >= long.Parse(mission.msnMetadata) && mission.msnPending == false)
                {
                    print("checking type 0.2 with meta " + long.Parse(mission.msnMetadata));
                    missionsPanel.GetComponent<missionParent>().completeMission(mission);
                }
            }

            /*
            foreach (var item in missionsPanel.GetComponent<missionParent>().missionList)
            {
                if (item.msnType == 0 && item.msnPending == false)
                {
                    if (item.msnName == "Cash Is King I" && money >= 1000000)
                    {
                        missionsPanel.GetComponent<missionParent>().completeMission(item);
                    }
                    else if (item.msnName == "Cash Is King II" && money >= 10000000)
                    {
                        missionsPanel.GetComponent<missionParent>().completeMission(item);
                    }
                    else if (item.msnName == "The One" && money >= 295000000)
                    {
                        missionsPanel.GetComponent<missionParent>().completeMission(item);
                    }
                    else if (item.msnName == "Youre a Millionaire I")
                    {
                        HQ.GetComponent<HQstats>().calcHQ();
                        if (HQ.GetComponent<HQstats>().totalLong >= 1000000)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                    }
                    else if (item.msnName == "Youre a Millionaire II")
                    {
                        HQ.GetComponent<HQstats>().calcHQ();
                        if (HQ.GetComponent<HQstats>().totalLong >= 5000000)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                    }
                    else if (item.msnName == "Youre a Millionaire III")
                    {
                        HQ.GetComponent<HQstats>().calcHQ();
                        if (HQ.GetComponent<HQstats>().totalLong >= 10000000)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                    }
                    else if (item.msnName == "Youre a Millionaire IV")
                    {
                        HQ.GetComponent<HQstats>().calcHQ();
                        if (HQ.GetComponent<HQstats>().totalLong >= 375000000)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                    }
                    else if (item.msnName == "Youre a Millionaire V")
                    {
                        HQ.GetComponent<HQstats>().calcHQ();
                        if (HQ.GetComponent<HQstats>().totalLong >= 1000000000)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                    }
                    else if (item.msnName == "Youre a Millionaire VI")
                    {
                        HQ.GetComponent<HQstats>().calcHQ();
                        if (HQ.GetComponent<HQstats>().totalLong >= 2000000000)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                    }
                    else if (item.msnName == "Beat Ronald Goldtooth")
                    {
                        HQ.GetComponent<HQstats>().calcHQ();
                        if (HQ.GetComponent<HQstats>().totalLong >= 100000000)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                    }
                    else if (item.msnName == "The Sheik Challenge")
                    {
                        HQ.GetComponent<HQstats>().calcHQ();
                        if (HQ.GetComponent<HQstats>().totalLong >= 900000000)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                    }
                    else if (item.msnName == "Tesla to the Moon")
                    {
                        HQ.GetComponent<HQstats>().calcHQ();
                        if (HQ.GetComponent<HQstats>().totalLong >= 137000000000)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                    }
                }
            }
            */
        }
    }

    public void updateName()
    {
        cityName = inputText.GetComponent<Text>().text;
        // Type 3 Missions --------------
        if (missionsPanel.GetComponent<missionParent>().missionList != null)
        {
            foreach (var item in missionsPanel.GetComponent<missionParent>().missionList)
            {
                if (item.msnType == 3 && item.msnPending == false)
                {
                    if (item.msnName == "Name It" && cityName != "Chocolate Fields")
                    {
                        missionsPanel.GetComponent<missionParent>().completeMission(item);
                    }
                }
            }
        }
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
