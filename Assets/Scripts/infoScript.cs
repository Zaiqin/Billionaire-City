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

                    switch (selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex)
                    {
                        case 0: incomeText.GetComponent<Text>().text = "$" + selProp.GetComponent<Property>().Card.threemins.ToString("#,##0"); break;
                        case 1: incomeText.GetComponent<Text>().text = "$" + selProp.GetComponent<Property>().Card.thirtymins.ToString("#,##0"); break;
                        case 2: incomeText.GetComponent<Text>().text = "$" + selProp.GetComponent<Property>().Card.onehour.ToString("#,##0"); break;
                        case 3: incomeText.GetComponent<Text>().text = "$" + selProp.GetComponent<Property>().Card.fourhours.ToString("#,##0"); break;
                        case 4: incomeText.GetComponent<Text>().text = "$" + selProp.GetComponent<Property>().Card.eighthours.ToString("#,##0"); break;
                        case 5: incomeText.GetComponent<Text>().text = "$" + selProp.GetComponent<Property>().Card.twelvehours.ToString("#,##0"); break;
                        case 6: incomeText.GetComponent<Text>().text = "$" + selProp.GetComponent<Property>().Card.oneday.ToString("#,##0"); break;
                        case 7: incomeText.GetComponent<Text>().text = "$" + selProp.GetComponent<Property>().Card.twodays.ToString("#,##0"); break;
                        case 8: incomeText.GetComponent<Text>().text = "$" + selProp.GetComponent<Property>().Card.threedays.ToString("#,##0"); break;
                        default: incomeText.GetComponent<Text>().text = "$" + selProp.GetComponent<Property>().Card.threemins.ToString("#,##0"); break;
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
            }
        }
        else
        {
            timeText.GetComponent<Text>().text = "";
            fill.SetActive(false);
        }
    }
}
