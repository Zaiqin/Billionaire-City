using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class infoScript : MonoBehaviour
{
    public GameObject selProp, nameText, timeText, fill, incomeText;

    // Update is called once per frame
    private void Start()
    {
        nameText.GetComponent<Text>().text = "";
        timeText.GetComponent<Text>().text = "";
        fill.SetActive(false);
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
                    fill.GetComponent<Image>().fillAmount = (float)(remainingSpan.TotalSeconds / fullSpan.TotalSeconds);
                    
                    if (diff.Days != 0)
                    {
                        timeText.GetComponent<Text>().text = string.Format("{0:D2} Days {1:D2} hrs", diff.Days, diff.Hours);
                    }
                    else if (diff.Hours != 0)
                    {
                        timeText.GetComponent<Text>().text = string.Format("{0:D2} Hours {1:D2} mins", diff.Hours, diff.Minutes);
                    }
                    else if (diff.Minutes != 0)
                    {
                        timeText.GetComponent<Text>().text = string.Format("{0:D2} mins {1:D2} secs", diff.Minutes, diff.Seconds);
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
                        string temp = tempIncome.ToString("#,##0");
                        incomeText.GetComponent<Text>().text = "$" + temp.Substring(0, temp.Length - 8) + "M";
                    }
                    else if (tempIncome > 0 && remainingSpan > TimeSpan.Zero)
                    {
                        incomeText.GetComponent<Text>().text = "$" + tempIncome.ToString("#,##0");
                    } else
                    {
                        print("income greater than null");
                        incomeText.GetComponent<Text>().text = "No income to earn";
                    }
                }
                else
                {
                    timeText.GetComponent<Text>().text = "No Contract Signed";
                    fill.SetActive(false);
                }
            }
            else
            {
                timeText.GetComponent<Text>().text = "No Contract Signed";
                fill.SetActive(false);
                incomeText.GetComponent<Text>().text = "No income to earn";
            }
        }
        else
        {
            timeText.GetComponent<Text>().text = "";
            fill.SetActive(false);
        }
    }
}
