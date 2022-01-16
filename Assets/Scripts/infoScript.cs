using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class infoScript : MonoBehaviour
{
    public GameObject selProp;
    public GameObject nameText, fillBg, fill, timeText, incomeText, incomeHeader, time, money, custHeader, tenantsIcon, tenantsText, xpIcon, xpText;
    public Sprite largeBg, smallBg, tenantsSprite, bonusSprite;

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

    public void initInfo()
    {
        selProp.GetComponent<SpriteRenderer>().material.color = new Color(35f / 255f, 206f / 255f, 241f / 255f, 255f / 255f);
        print("initing info again");
        if (selProp.GetComponent<Property>().Card.type == "House")
        {
            print("showing house info");
            
            nameText.SetActive(true); fillBg.SetActive(true); fill.SetActive(true); timeText.SetActive(true); incomeText.SetActive(true); incomeHeader.SetActive(false); time.SetActive(true); money.SetActive(true); xpIcon.SetActive(true); xpText.SetActive(true);
            this.GetComponent<Image>().sprite = largeBg;
            nameText.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            custHeader.GetComponent<Text>().text = "Tenants";
            incomeHeader.GetComponent<Text>().text = "Income";
            xpText.GetComponent<Text>().text = (selProp.GetComponent<Property>().Card.XP * (selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex + 1)) + " XP";
            
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

                    // ---- Checking what houses it affects ---------
                    selProp.transform.GetChild(2).gameObject.SetActive(true);
                    selProp.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
                    List<Collider2D> infList = selProp.transform.GetChild(2).GetComponent<detectDecoInf>().returnHighlights();
                    selProp.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(35f / 255f, 206f / 255f, 241f / 255f, 125f / 255f);
                    selProp.transform.GetChild(2).gameObject.SetActive(false);
                    //------------------------------------------------

                    long tempIncome;
                    switch (selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex)
                    {
                        case 0: tempIncome = (long)selProp.GetComponent<Property>().Card.threemins; break;
                        case 1: tempIncome = (long)selProp.GetComponent<Property>().Card.thirtymins; break;
                        case 2: tempIncome = (long)selProp.GetComponent<Property>().Card.onehour; break;
                        case 3: tempIncome = (long)selProp.GetComponent<Property>().Card.fourhours; break;
                        case 4: tempIncome = (long)selProp.GetComponent<Property>().Card.eighthours; break;
                        case 5: tempIncome = (long)selProp.GetComponent<Property>().Card.twelvehours; break;
                        case 6: tempIncome = (long)selProp.GetComponent<Property>().Card.oneday; break;
                        case 7: tempIncome = (long)selProp.GetComponent<Property>().Card.twodays; break;
                        case 8: tempIncome = (long)selProp.GetComponent<Property>().Card.threedays; break;
                        default: tempIncome = (long)selProp.GetComponent<Property>().Card.threemins; break;
                    }

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
                }
                else
                {
                    // Income waiting to be collected, contract fulfilled
                    this.gameObject.SetActive(false);
                }
            }
            selProp.GetComponent<Property>().bonus = totalDecoBonus;
        }
        else if (selProp.GetComponent<Property>().Card.type == "Commerce")
        {
            nameText.SetActive(true); fillBg.SetActive(true); fill.SetActive(true); timeText.SetActive(true); incomeText.SetActive(true); incomeHeader.SetActive(true); time.SetActive(true); money.SetActive(true); xpIcon.SetActive(false); xpText.SetActive(false);
            this.GetComponent<Image>().sprite = largeBg;
            nameText.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            custHeader.GetComponent<Text>().text = "Customers";
            incomeHeader.GetComponent<Text>().text = "Income";
            
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
                incomeText.GetComponent<Text>().text = "$" + finalIncome.ToString("#,##0");
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
            incomeHeader.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            incomeHeader.SetActive(true);
            incomeHeader.GetComponent<Text>().fontSize = 28;
            
            tenantsIcon.GetComponent<Image>().sprite = bonusSprite;
        }
        else
        {
            //info script showing non houses
            nameText.SetActive(false); fillBg.SetActive(false); fill.SetActive(false); timeText.SetActive(false); incomeText.SetActive(false); time.SetActive(false); money.SetActive(false); xpIcon.SetActive(false); xpText.SetActive(false);
            this.GetComponent<Image>().sprite = smallBg;
            tenantsText.GetComponent<Text>().text = "+ " + selProp.GetComponent<Property>().Card.wonderBonus.ToString() + " %";
            custHeader.GetComponent<Text>().text = "Wonder Bonus";
            incomeHeader.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            incomeHeader.SetActive(true);
            incomeHeader.GetComponent<Text>().fontSize = 28;
            
            tenantsIcon.GetComponent<Image>().sprite = bonusSprite;
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
            xpText.GetComponent<Text>().text = (selProp.GetComponent<Property>().Card.XP * (selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex + 1)) + " XP";
            nameText.SetActive(true); fillBg.SetActive(true); fill.SetActive(true); timeText.SetActive(true); incomeText.SetActive(true); incomeHeader.SetActive(false); time.SetActive(true); money.SetActive(true); xpIcon.SetActive(true); xpText.SetActive(true);
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
                }
                else
                {
                    // Income waiting to be collected, contract fulfilled
                    this.gameObject.SetActive(false);
                }
            }
        }
        else if (selProp.GetComponent<Property>().Card.type == "Commerce")
        {
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
                fill.GetComponent<Image>().fillAmount = (float)(remainingSpan.TotalSeconds / fullSpan.TotalSeconds);
                incomeText.GetComponent<Text>().text = "$" + finalIncome.ToString("#,##0");
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
            this.GetComponent<Image>().sprite = smallBg;
            tenantsText.GetComponent<Text>().text = "+" + selProp.GetComponent<Property>().Card.decoBonus + " %";
            custHeader.GetComponent<Text>().text = "Houses Bonus";
            incomeHeader.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            incomeHeader.SetActive(true);
            incomeHeader.GetComponent<Text>().fontSize = 28;
            nameText.SetActive(false); fillBg.SetActive(false); fill.SetActive(false); timeText.SetActive(false); incomeText.SetActive(false); time.SetActive(false); money.SetActive(false); xpIcon.SetActive(false); xpText.SetActive(false);
            tenantsIcon.GetComponent<Image>().sprite = bonusSprite;
        }
        else
        {
            //info script showing non houses
            this.GetComponent<Image>().sprite = smallBg;
            tenantsText.GetComponent<Text>().text = "+ " + selProp.GetComponent<Property>().Card.wonderBonus.ToString() + " % ";
            custHeader.GetComponent<Text>().text = "Wonder Bonus";
            incomeHeader.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
            incomeHeader.SetActive(true);
            incomeHeader.GetComponent<Text>().fontSize = 28;
            nameText.SetActive(false); fillBg.SetActive(false); fill.SetActive(false); timeText.SetActive(false); incomeText.SetActive(false); time.SetActive(false); money.SetActive(false); xpIcon.SetActive(false); xpText.SetActive(false);
            tenantsIcon.GetComponent<Image>().sprite = bonusSprite;
        }
    }
}
