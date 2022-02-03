using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class infoScript : MonoBehaviour
{
    public GameObject selProp;
    public GameObject nameText, fillBg, fill, timeText, incomeText, incomeHeader, time, money, custHeader, tenantsIcon, tenantsText, xpIcon, xpText, consfillBg, consfill, constimeText, cons, instantBuild, constHeader;
    public Sprite largeBg, smallBg, constructBg, tenantsSprite, bonusSprite;
    public long instantCost;

    public GameObject highlightedProp;

    public int totalDecoBonus = 0;

    // Update is called once per frame
    private void Start()
    {
        nameText.GetComponent<Text>().text = "";
        timeText.GetComponent<Text>().text = "";
        fill.SetActive(false);
        fillBg.SetActive(false);
    }

    public long calcInstant(Property pp, TimeSpan time)
    {
        long calc = 0;
        if (pp.Card.cost.Contains("Gold"))
        {
            int temp = int.Parse(pp.Card.cost.Remove(pp.Card.cost.Length - 5));
            calc = 60000 * temp;
        }
        else
        {
            calc = long.Parse(pp.Card.cost);
        }
        double perten = time.TotalMinutes / 10;
        calc = (long)( (95/6) * (calc / 1000) * perten);
        //print("calc is " + calc);
        
        return calc;
    }

    public void initInfo()
    {
        selProp.GetComponent<SpriteRenderer>().material.color = new Color(35f / 255f, 206f / 255f, 241f / 255f, 255f / 255f);
        print("initing info again");

        if (selProp.GetComponent<Property>().Card.type == "House")
        {
            print("showing house info");
            
            nameText.SetActive(true); fillBg.SetActive(true); fill.SetActive(true); timeText.SetActive(true); incomeText.SetActive(true); incomeHeader.SetActive(false); time.SetActive(true); money.SetActive(true); xpIcon.SetActive(true); xpText.SetActive(true);
            consfill.SetActive(false); consfillBg.SetActive(false); constimeText.SetActive(false); cons.SetActive(false); instantBuild.SetActive(false); constHeader.SetActive(false);
            custHeader.SetActive(true); tenantsText.SetActive(true); tenantsIcon.SetActive(true);
            this.GetComponent<Image>().sprite = largeBg;
            nameText.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            custHeader.GetComponent<Text>().text = "Tenants";
            incomeHeader.GetComponent<Text>().text = "Income";
            
            incomeHeader.GetComponent<Text>().fontSize = 22;
            tenantsIcon.GetComponent<Image>().sprite = tenantsSprite;
            if (selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime != "notsigned")
            {
                var diff = DateTime.Parse(selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime) - System.DateTime.Now;

                if (diff > TimeSpan.Zero)
                {
                    TimeSpan fullSpan = DateTime.Parse(selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime) - DateTime.Parse(selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signCreationTime);
                    TimeSpan remainingSpan = DateTime.Parse(selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime) - System.DateTime.Now;
                    fill.SetActive(true);
                    fillBg.SetActive(true);
                    fill.GetComponent<Image>().fillAmount = (float)(remainingSpan.TotalSeconds / fullSpan.TotalSeconds);

                    if (diff.Days != 0)
                    {
                        timeText.GetComponent<Text>().text = string.Format("{0:D2} Day {1:D2} hr", diff.Days, diff.Hours);
                    }
                    else if (diff.Hours != 0)
                    {
                        timeText.GetComponent<Text>().text = string.Format("{0:D2} Hour {1:D2} min", diff.Hours, diff.Minutes);
                    }
                    else if (diff.Minutes != 0)
                    {
                        timeText.GetComponent<Text>().text = string.Format("{0:D2} min {1:D2} sec", diff.Minutes, diff.Seconds);
                    }
                    else if (diff.Seconds != 0)
                    {
                        timeText.GetComponent<Text>().text = string.Format("{0:D2} seconds", diff.Seconds);
                    }
                    int xpVal = 0;
                    long tempIncome;
                    switch (selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex)
                    {
                        case 0: tempIncome = (long)selProp.GetComponent<Property>().Card.threemins; xpVal = selProp.GetComponent<Property>().Card.xpthreemins; break;
                        case 1: tempIncome = (long)selProp.GetComponent<Property>().Card.thirtymins; xpVal = selProp.GetComponent<Property>().Card.xpthirtymins; break;
                        case 2: tempIncome = (long)selProp.GetComponent<Property>().Card.onehour; xpVal = selProp.GetComponent<Property>().Card.xponehour; break;
                        case 3: tempIncome = (long)selProp.GetComponent<Property>().Card.fourhours; xpVal = selProp.GetComponent<Property>().Card.xpfourhours; break;
                        case 4: tempIncome = (long)selProp.GetComponent<Property>().Card.eighthours; xpVal = selProp.GetComponent<Property>().Card.xpeighthours; break;
                        case 5: tempIncome = (long)selProp.GetComponent<Property>().Card.twelvehours; xpVal = selProp.GetComponent<Property>().Card.xptwelvehours; break;
                        case 6: tempIncome = (long)selProp.GetComponent<Property>().Card.oneday; xpVal = selProp.GetComponent<Property>().Card.xponeday; break;
                        case 7: tempIncome = (long)selProp.GetComponent<Property>().Card.twodays; xpVal = selProp.GetComponent<Property>().Card.xptwodays; break;
                        case 8: tempIncome = (long)selProp.GetComponent<Property>().Card.threedays; xpVal = selProp.GetComponent<Property>().Card.xpthreedays; break;
                        default: tempIncome = (long)selProp.GetComponent<Property>().Card.threemins; xpVal = selProp.GetComponent<Property>().Card.xpthreemins; break;
                    }

                    // ---- Checking what houses it affects ---------
                    selProp.transform.GetChild(2).gameObject.SetActive(true);
                    selProp.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
                    List<Collider2D> infList = selProp.transform.GetChild(2).GetComponent<detectDecoInf>().returnHighlights();
                    selProp.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(35f / 255f, 206f / 255f, 241f / 255f, 125f / 255f);
                    selProp.transform.GetChild(2).gameObject.SetActive(false);
                    //------------------------------------------------
                    totalDecoBonus = 0;
                    foreach (Collider2D item in infList)
                    {
                        totalDecoBonus += GameObject.Find(item.name).GetComponent<Property>().Card.decoBonus;
                    }
                    float percent = 1 + (((float)totalDecoBonus) / 100);
                    long finalProfit = (long)((float)tempIncome * percent);
                    float wonderPercent = 1 + (((float)(GameObject.Find("Stats").GetComponent<Statistics>().wonderBonus)) / 100);
                    finalProfit = (long)((float)finalProfit * wonderPercent);
                    print("profit is " + tempIncome + ", final profit is " + finalProfit);

                    if (finalProfit >= 100000000 && remainingSpan > TimeSpan.Zero)
                    {
                        // Contract ongoing and income is more than 100M
                        string temp = finalProfit.ToString("#,##0");
                        incomeText.GetComponent<Text>().text = "$" + temp.Substring(0, temp.Length - 8) + "M";
                    }
                    else if (finalProfit > 0 && remainingSpan > TimeSpan.Zero)
                    {
                        // Contract ongoing and income is less than 100M
                        incomeText.GetComponent<Text>().text = "$" + finalProfit.ToString("#,##0");
                    }
                    int Tenants = (selProp.GetComponent<Property>().Card.tenants) * (selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex + 1);
                    tenantsText.GetComponent<Text>().text = Tenants.ToString();
                    xpText.GetComponent<Text>().text = xpVal.ToString("#,##0") + " XP";
                }
                else
                {
                    // Income waiting to be collected, contract fulfilled
                    this.gameObject.SetActive(false);
                }
            }
            selProp.GetComponent<Property>().bonus = totalDecoBonus;
        }
        else if (selProp.GetComponent<Property>().Card.type == "Commerce" && selProp.GetComponent<Property>().constructEnd == "na")
        {
            nameText.SetActive(true); fillBg.SetActive(true); fill.SetActive(true); timeText.SetActive(true); incomeText.SetActive(true); incomeHeader.SetActive(true); time.SetActive(true); money.SetActive(true); xpIcon.SetActive(false); xpText.SetActive(false);
            this.GetComponent<Image>().sprite = largeBg;
            nameText.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            custHeader.GetComponent<Text>().text = "Customers";
            incomeHeader.GetComponent<Text>().text = "Income";
            custHeader.SetActive(true); tenantsText.SetActive(true); tenantsIcon.SetActive(true);
            consfill.SetActive(false); consfillBg.SetActive(false); constimeText.SetActive(false); cons.SetActive(false); instantBuild.SetActive(false); constHeader.SetActive(false);
            tenantsIcon.GetComponent<Image>().sprite = tenantsSprite;
            incomeHeader.GetComponent<Text>().fontSize = 22;
            List<Collider2D> infList = selProp.gameObject.transform.GetChild(0).GetComponent<influence>().housesInfluenced;
            long finalIncome = 0;
            int finalTenants = 0;
            foreach (Collider2D item in infList)
            {
                GameObject obj = GameObject.Find(item.name);
                //print("adding "+item.name +" with signTime " + GameObject.Find(item.name).transform.GetChild(0).GetComponent<contractScript>().signTime);
                if (obj.transform.GetChild(0).GetComponent<contractScript>().signTime != "notsigned")
                {

                    switch (obj.transform.GetChild(0).GetComponent<contractScript>().signIndex)
                    {
                        case 1: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 2) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        case 2: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 3) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        case 3: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 4) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        case 4: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 5) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        case 5: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 6) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        case 6: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 7) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        case 7: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 8) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        case 8: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 9) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        default: finalIncome += (long)obj.GetComponent<Property>().Card.tenants * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                    }
                    finalTenants += (obj.GetComponent<Property>().Card.tenants * (obj.transform.GetChild(0).GetComponent<contractScript>().signIndex + 1));
                    //print("added " + (obj.GetComponent<Property>().Card.tenants * (obj.transform.GetChild(0).GetComponent<contractScript>().signIndex + 1)) + "tenants from " + obj);

                }
            }
            var diff = DateTime.Parse(selProp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime) - System.DateTime.Now;
            if (diff > TimeSpan.Zero)
            {
                TimeSpan fullSpan = DateTime.Parse(selProp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime) - DateTime.Parse(selProp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signCreationTime);
                TimeSpan remainingSpan = DateTime.Parse(selProp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime) - System.DateTime.Now;
                fill.SetActive(true);
                fillBg.SetActive(true);
                fill.GetComponent<Image>().fillAmount = (float)(remainingSpan.TotalSeconds / fullSpan.TotalSeconds);
                float percent = 1 + (((float)GameObject.Find("Stats").GetComponent<Statistics>().wonderCommerceBonus) / 10);
                long finalProfit = (long)((float)finalIncome * percent);
                incomeText.GetComponent<Text>().text = "$" + finalProfit.ToString("#,##0");
                if (diff.Minutes != 0)
                {
                    timeText.GetComponent<Text>().text = string.Format("{0:D1} min {1:D2} sec", diff.Minutes, diff.Seconds);
                }
                else if (diff.Seconds != 0)
                {
                    timeText.GetComponent<Text>().text = string.Format("{0:D2} seconds", diff.Seconds);
                }
            }
            else
            {
                timeText.GetComponent<Text>().text = "Commerce Fulfilled";
                incomeText.GetComponent<Text>().text = "";
                fill.SetActive(false);
                fillBg.SetActive(false);
            }
            tenantsText.GetComponent<Text>().text = finalTenants.ToString();
        }
        else if (selProp.GetComponent<Property>().Card.type == "Deco")
        {
            nameText.SetActive(false); fillBg.SetActive(false); fill.SetActive(false); timeText.SetActive(false); incomeText.SetActive(false); time.SetActive(false); money.SetActive(false); xpIcon.SetActive(false); xpText.SetActive(false);
            this.GetComponent<Image>().sprite = smallBg;
            tenantsText.GetComponent<Text>().text = "+" + selProp.GetComponent<Property>().Card.decoBonus + " %";
            custHeader.GetComponent<Text>().text = "Houses Bonus";
            custHeader.SetActive(true); tenantsText.SetActive(true); tenantsIcon.SetActive(true);
            incomeHeader.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            incomeHeader.SetActive(true);
            incomeHeader.GetComponent<Text>().fontSize = 28;
            consfill.SetActive(false); consfillBg.SetActive(false); constimeText.SetActive(false); cons.SetActive(false); instantBuild.SetActive(false); constHeader.SetActive(false);
            tenantsIcon.GetComponent<Image>().sprite = bonusSprite;
        }
        else
        {
            //info script showing non houses
            nameText.SetActive(false); fillBg.SetActive(false); fill.SetActive(false); timeText.SetActive(false); incomeText.SetActive(false); time.SetActive(false); money.SetActive(false); xpIcon.SetActive(false); xpText.SetActive(false);
            instantBuild.SetActive(false);
            this.GetComponent<Image>().sprite = smallBg;
            if (selProp.GetComponent<Property>().Card.wonderBonus >= 100)
            {
                tenantsText.GetComponent<Text>().text = (((float)selProp.GetComponent<Property>().Card.wonderBonus) / 100).ToString() + "% Double Rent";
            }
            else if (selProp.GetComponent<Property>().Card.wonderBonus > 10)
            {
                tenantsText.GetComponent<Text>().text = "+ " + (((float)selProp.GetComponent<Property>().Card.wonderBonus) / 10).ToString() + "% Commerce";
            }
            else
            {
                tenantsText.GetComponent<Text>().text = "+ " + selProp.GetComponent<Property>().Card.wonderBonus.ToString() + "% Houses";
            }
            custHeader.GetComponent<Text>().text = "Wonder Bonus";
            custHeader.SetActive(true); tenantsText.SetActive(true); tenantsIcon.SetActive(true);
            incomeHeader.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            incomeHeader.SetActive(true);
            incomeHeader.GetComponent<Text>().fontSize = 28;
            consfill.SetActive(false); consfillBg.SetActive(false); constimeText.SetActive(false); cons.SetActive(false); constHeader.SetActive(false);
            tenantsIcon.GetComponent<Image>().sprite = bonusSprite;
        }

        // Change if its still constructin ---
        if (selProp.GetComponent<Property>().constructEnd != "na")
        {
            print("showing const version");
            nameText.SetActive(false); fillBg.SetActive(false); fill.SetActive(false); timeText.SetActive(false); incomeText.SetActive(false); time.SetActive(false); money.SetActive(false); xpIcon.SetActive(false); xpText.SetActive(false);
            this.GetComponent<Image>().sprite = constructBg;
            tenantsText.SetActive(false);
            custHeader.SetActive(true); custHeader.GetComponent<Text>().text = "Cost: $-";
            constHeader.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            incomeHeader.SetActive(false);
            consfill.SetActive(true); consfillBg.SetActive(true); constimeText.SetActive(true); cons.SetActive(true);
            tenantsIcon.SetActive(false); instantBuild.SetActive(true); constHeader.SetActive(true);

            if (selProp.GetComponent<Property>().Card.type == "House")
            {
                // ---- Checking what houses it affects ---------
                selProp.transform.GetChild(2).gameObject.SetActive(true);
                selProp.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
                List<Collider2D> infList = selProp.transform.GetChild(2).GetComponent<detectDecoInf>().returnHighlights();
                selProp.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(35f / 255f, 206f / 255f, 241f / 255f, 125f / 255f);
                selProp.transform.GetChild(2).gameObject.SetActive(false);
                //------------------------------------------------
                totalDecoBonus = 0;
                foreach (Collider2D item in infList)
                {
                    totalDecoBonus += GameObject.Find(item.name).GetComponent<Property>().Card.decoBonus;
                }
                selProp.GetComponent<Property>().bonus = totalDecoBonus;
            }
        }
    }

    void Update()
    {
        if (highlightedProp == null)
        {
            highlightedProp = selProp;
        }
        if (highlightedProp.name != selProp.name)
        {
            highlightedProp.GetComponent<SpriteRenderer>().material.color = Color.white;
            highlightedProp = selProp;
        }
        if (selProp.GetComponent<Property>().Card.type == "House")
        {
            this.GetComponent<Image>().sprite = largeBg;
            nameText.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            custHeader.GetComponent<Text>().text = "Tenants";
            incomeHeader.GetComponent<Text>().text = "Income";
            
            nameText.SetActive(true); fillBg.SetActive(true); fill.SetActive(true); timeText.SetActive(true); incomeText.SetActive(true); incomeHeader.SetActive(false); time.SetActive(true); money.SetActive(true); xpIcon.SetActive(true); xpText.SetActive(true);
            incomeHeader.GetComponent<Text>().fontSize = 22;
            tenantsIcon.GetComponent<Image>().sprite = tenantsSprite;
            tenantsText.SetActive(true); tenantsIcon.SetActive(true);
            custHeader.SetActive(true);
            consfill.SetActive(false); consfillBg.SetActive(false); constimeText.SetActive(false); cons.SetActive(false); instantBuild.SetActive(false); constHeader.SetActive(false);
            if (selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime != "notsigned")
            {
                var diff = DateTime.Parse(selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime) - System.DateTime.Now;

                if (diff > TimeSpan.Zero)
                {
                    TimeSpan fullSpan = DateTime.Parse(selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime) - DateTime.Parse(selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signCreationTime);
                    TimeSpan remainingSpan = DateTime.Parse(selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime) - System.DateTime.Now;
                    fill.SetActive(true);
                    fillBg.SetActive(true);
                    fill.GetComponent<Image>().fillAmount = 1-(float)(remainingSpan.TotalSeconds / fullSpan.TotalSeconds);

                    if (diff.Days != 0)
                    {
                        timeText.GetComponent<Text>().text = string.Format("{0:D2} Day {1:D2} hr", diff.Days, diff.Hours);
                    }
                    else if (diff.Hours != 0)
                    {
                        timeText.GetComponent<Text>().text = string.Format("{0:D2} Hour {1:D2} min", diff.Hours, diff.Minutes);
                    }
                    else if (diff.Minutes != 0)
                    {
                        timeText.GetComponent<Text>().text = string.Format("{0:D2} min {1:D2} sec", diff.Minutes, diff.Seconds);
                    }
                    else if (diff.Seconds != 0)
                    {
                        timeText.GetComponent<Text>().text = string.Format("{0:D2} seconds", diff.Seconds);
                    }
                }
                else
                {
                    // Income waiting to be collected, contract fulfilled
                    this.gameObject.SetActive(false);
                }
            }

            int xpVal = 0;
            switch (selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex)
            {
                case 0: xpVal = selProp.GetComponent<Property>().Card.xpthreemins; break;
                case 1: xpVal = selProp.GetComponent<Property>().Card.xpthirtymins; break;
                case 2: xpVal = selProp.GetComponent<Property>().Card.xponehour; break;
                case 3: xpVal = selProp.GetComponent<Property>().Card.xpfourhours; break;
                case 4: xpVal = selProp.GetComponent<Property>().Card.xpeighthours; break;
                case 5: xpVal = selProp.GetComponent<Property>().Card.xptwelvehours; break;
                case 6: xpVal = selProp.GetComponent<Property>().Card.xponeday; break;
                case 7: xpVal = selProp.GetComponent<Property>().Card.xptwodays; break;
                case 8: xpVal = selProp.GetComponent<Property>().Card.xpthreedays; break;
                default: xpVal = selProp.GetComponent<Property>().Card.xpthreemins; break;
            }
            xpText.GetComponent<Text>().text = xpVal.ToString("#,##0") + " XP";
        }
        else if (selProp.GetComponent<Property>().Card.type == "Commerce" && selProp.GetComponent<Property>().constructEnd == "na")
        {
            consfill.SetActive(false); consfillBg.SetActive(false); constimeText.SetActive(false); cons.SetActive(false); instantBuild.SetActive(false); constHeader.SetActive(false); tenantsIcon.SetActive(true);
            this.GetComponent<Image>().sprite = largeBg;
            nameText.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            custHeader.GetComponent<Text>().text = "Customers";
            incomeHeader.GetComponent<Text>().text = "Income";
            nameText.SetActive(true); fillBg.SetActive(true); fill.SetActive(true); timeText.SetActive(true); incomeText.SetActive(true); incomeHeader.SetActive(true); time.SetActive(true); money.SetActive(true); xpIcon.SetActive(false); xpText.SetActive(false);
            tenantsIcon.GetComponent<Image>().sprite = tenantsSprite;
            incomeHeader.GetComponent<Text>().fontSize = 22;
            List<Collider2D> infList = selProp.gameObject.transform.GetChild(0).GetComponent<influence>().housesInfluenced;
            long finalIncome = 0;
            int finalTenants = 0;
            foreach (Collider2D item in infList)
            {
                GameObject obj = GameObject.Find(item.name);
                //print("adding "+item.name +" with signTime " + GameObject.Find(item.name).transform.GetChild(0).GetComponent<contractScript>().signTime);
                if (obj.transform.GetChild(0).GetComponent<contractScript>().signTime != "notsigned")
                {

                    switch (obj.transform.GetChild(0).GetComponent<contractScript>().signIndex)
                    {
                        case 1: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 2) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        case 2: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 3) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        case 3: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 4) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        case 4: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 5) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        case 5: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 6) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        case 6: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 7) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        case 7: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 8) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        case 8: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 9) * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                        default: finalIncome += (long)obj.GetComponent<Property>().Card.tenants * selProp.GetComponent<Property>().Card.rentPerTenant; break;
                    }
                    finalTenants += (obj.GetComponent<Property>().Card.tenants * (obj.transform.GetChild(0).GetComponent<contractScript>().signIndex + 1));
                    //print("added " + (obj.GetComponent<Property>().Card.tenants * (obj.transform.GetChild(0).GetComponent<contractScript>().signIndex + 1)) + "tenants from " + obj);

                }
            }
            var diff = DateTime.Parse(selProp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime) - System.DateTime.Now;
            if (diff > TimeSpan.Zero)
            {
                TimeSpan fullSpan = DateTime.Parse(selProp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime) - DateTime.Parse(selProp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signCreationTime);
                TimeSpan remainingSpan = DateTime.Parse(selProp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime) - System.DateTime.Now;
                fill.SetActive(true);
                fillBg.SetActive(true);
                fill.GetComponent<Image>().fillAmount = 1-(float)(remainingSpan.TotalSeconds / fullSpan.TotalSeconds);
                float percent = 1 + (((float)GameObject.Find("Stats").GetComponent<Statistics>().wonderCommerceBonus) / 10);
                long finalProfit = (long)((float)finalIncome * percent);
                incomeText.GetComponent<Text>().text = "$" + finalProfit.ToString("#,##0");
                if (diff.Minutes != 0)
                {
                    timeText.GetComponent<Text>().text = string.Format("{0:D1} min {1:D2} sec", diff.Minutes, diff.Seconds);
                }
                else if (diff.Seconds != 0)
                {
                    timeText.GetComponent<Text>().text = string.Format("{0:D2} seconds", diff.Seconds);
                }
            }
            else
            {
                this.gameObject.SetActive(false);
            }
            tenantsText.GetComponent<Text>().text = finalTenants.ToString();
        }
        else if (selProp.GetComponent<Property>().Card.type == "Deco")
        {
            this.GetComponent<Image>().sprite = smallBg;
            tenantsText.GetComponent<Text>().text = "+" + selProp.GetComponent<Property>().Card.decoBonus + " %";
            custHeader.GetComponent<Text>().text = "Houses Bonus";
            incomeHeader.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            incomeHeader.SetActive(true);
            incomeHeader.GetComponent<Text>().fontSize = 28;
            nameText.SetActive(false); fillBg.SetActive(false); fill.SetActive(false); timeText.SetActive(false); incomeText.SetActive(false); time.SetActive(false); money.SetActive(false); xpIcon.SetActive(false); xpText.SetActive(false);
            tenantsIcon.GetComponent<Image>().sprite = bonusSprite;
            consfill.SetActive(false); consfillBg.SetActive(false); constimeText.SetActive(false); cons.SetActive(false); instantBuild.SetActive(false); constHeader.SetActive(false); tenantsIcon.SetActive(true);
        }
        else
        {
            //info script showing non houses
            instantBuild.SetActive(false);
            this.GetComponent<Image>().sprite = smallBg;
            if (selProp.GetComponent<Property>().Card.wonderBonus >= 100)
            {
                tenantsText.GetComponent<Text>().text = (((float)selProp.GetComponent<Property>().Card.wonderBonus) / 100).ToString() + "% Double Rent";
            }
            else if (selProp.GetComponent<Property>().Card.wonderBonus > 10)
            {
                tenantsText.GetComponent<Text>().text = "+ " + (((float)selProp.GetComponent<Property>().Card.wonderBonus) / 10).ToString() + "% Commerce";
            }
            else
            {
                tenantsText.GetComponent<Text>().text = "+ " + selProp.GetComponent<Property>().Card.wonderBonus.ToString() + "% Houses";
            }
            custHeader.GetComponent<Text>().text = "Wonder Bonus";
            incomeHeader.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            incomeHeader.SetActive(true);
            incomeHeader.GetComponent<Text>().fontSize = 28;
            nameText.SetActive(false); fillBg.SetActive(false); fill.SetActive(false); timeText.SetActive(false); incomeText.SetActive(false); time.SetActive(false); money.SetActive(false); xpIcon.SetActive(false); xpText.SetActive(false);
            tenantsIcon.GetComponent<Image>().sprite = bonusSprite;
            consfill.SetActive(false); consfillBg.SetActive(false); constimeText.SetActive(false); cons.SetActive(false); constHeader.SetActive(false); tenantsIcon.SetActive(true);
        }

        if (selProp.GetComponent<Property>().constructEnd != "na")
        {
            //yprint("showing const version");
            nameText.SetActive(false); fillBg.SetActive(false); fill.SetActive(false); timeText.SetActive(false); incomeText.SetActive(false); time.SetActive(false); money.SetActive(false); xpIcon.SetActive(false); xpText.SetActive(false);
            this.GetComponent<Image>().sprite = constructBg;
            tenantsText.SetActive(false);
            custHeader.SetActive(true);
            consfill.SetActive(true); consfillBg.SetActive(true); constimeText.SetActive(true); cons.SetActive(true); constHeader.SetActive(true);
            constHeader.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            incomeHeader.SetActive(false);
            tenantsIcon.SetActive(false);
            instantBuild.SetActive(true);
            var diff = DateTime.Parse(selProp.GetComponent<Property>().constructEnd) - System.DateTime.Now;

            if (diff > TimeSpan.Zero)
            {
                TimeSpan fullSpan = DateTime.Parse(selProp.GetComponent<Property>().constructEnd) - DateTime.Parse(selProp.GetComponent<Property>().constructStart);
                TimeSpan remainingSpan = DateTime.Parse(selProp.GetComponent<Property>().constructEnd) - System.DateTime.Now;
                consfill.GetComponent<Image>().fillAmount = 1-(float)(remainingSpan.TotalSeconds / fullSpan.TotalSeconds);

                if (diff.Days != 0)
                {
                    constimeText.GetComponent<Text>().text = string.Format("{0:D2} Day {1:D2} hr", diff.Days, diff.Hours);
                }
                else if (diff.Hours != 0)
                {
                    constimeText.GetComponent<Text>().text = string.Format("{0:D2} Hour {1:D2} min", diff.Hours, diff.Minutes);
                }
                else if (diff.Minutes != 0)
                {
                    constimeText.GetComponent<Text>().text = string.Format("{0:D2} min {1:D2} sec", diff.Minutes, diff.Seconds);
                }
                else if (diff.Seconds != 0)
                {
                    constimeText.GetComponent<Text>().text = string.Format("{0:D2} seconds", diff.Seconds);
                    if (diff.Seconds < 2)
                    {
                        this.gameObject.SetActive(false);
                    }
                }
            }
            instantCost = calcInstant(selProp.GetComponent<Property>(), diff);
            custHeader.GetComponent<Text>().text = "Cost: $" + instantCost.ToString("#,##0");
        }
    }
}
