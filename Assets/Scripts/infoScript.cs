using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class infoScript : MonoBehaviour
{
    public GameObject selProp, nameText, timeText, fill, incomeText, fillBg;

    // Update is called once per frame
    private void Start()
    {
        nameText.GetComponent<Text>().text = "";
        timeText.GetComponent<Text>().text = "";
        fill.SetActive(false);
        fillBg.SetActive(false);
    }

    void Update()
    {
        nameText.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
        if (selProp.GetComponent<Property>().Card.type == "House")
        {
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
                    if (tempIncome >= 100000000 && remainingSpan > TimeSpan.Zero)
                    {
                        // Contract ongoing and income is more than 100M
                        string temp = tempIncome.ToString("#,##0");
                        incomeText.GetComponent<Text>().text = "$" + temp.Substring(0, temp.Length - 8) + "M";
                    }
                    else if (tempIncome > 0 && remainingSpan > TimeSpan.Zero)
                    {
                        // Contract ongoing and income is less than 100M
                        incomeText.GetComponent<Text>().text = "$" + tempIncome.ToString("#,##0");
                    }
                }
                else
                {
                    // Income waiting to be collected, contract fulfilled
                    timeText.GetComponent<Text>().text = "Contract Fulfilled";
                    incomeText.GetComponent<Text>().text = "";
                    fill.SetActive(false);
                    fillBg.SetActive(false);
                }
            }
            else
            {
                // No Contract Signed
                timeText.GetComponent<Text>().text = "No Contract Signed";
                fill.SetActive(false);
                fillBg.SetActive(false);
                incomeText.GetComponent<Text>().text = "No income to earn";
            }
        }
        else if (selProp.GetComponent<Property>().Card.type == "Commerce")
        {
            List<Collider2D> infList = selProp.gameObject.transform.GetChild(0).GetComponent<influence>().housesInfluenced;
            long finalIncome = 0;
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

                    print("added " + (obj.GetComponent<Property>().Card.tenants * (obj.transform.GetChild(0).GetComponent<contractScript>().signIndex + 1)) + "tenants from " + obj);

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
            } else
            {
                timeText.GetComponent<Text>().text = "Commerce Fulfilled";
                incomeText.GetComponent<Text>().text = "";
                fill.SetActive(false);
                fillBg.SetActive(false);
            }
        } else {
            //info script showing non houses
            timeText.GetComponent<Text>().text = "";
            incomeText.GetComponent<Text>().text = "";
            fill.SetActive(false);
            fillBg.SetActive(false);
        }
    }
}
