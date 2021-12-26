using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class infoScript : MonoBehaviour
{
    public GameObject selProp, nameText, timeText;

    // Update is called once per frame
    void Update()
    {
        nameText.GetComponent<Text>().text = selProp.GetComponent<Property>().Card.displayName;
        if (selProp.GetComponent<Property>().Card.type == "House")
        {
            var diff = DateTime.Parse(selProp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime) - System.DateTime.Now;
            if (diff > TimeSpan.Zero)
            {
                if (diff.Days != 0)
                {
                    timeText.GetComponent<Text>().text = string.Format("{0:D2} Days {1:D2} hrs", diff.Days, diff.Hours);
                } else if (diff.Hours != 0)
                {
                    timeText.GetComponent<Text>().text = string.Format("{0:D2} Hours {1:D2} mins", diff.Hours, diff.Minutes);
                } else if (diff.Minutes != 0)
                {
                    timeText.GetComponent<Text>().text = string.Format("{0:D2} mins {1:D2} secs", diff.Minutes, diff.Seconds);
                } else if (diff.Seconds != 0)
                {
                    timeText.GetComponent<Text>().text = string.Format("{0:D2} seconds", diff.Seconds);
                }
                
            }
            else
            {
                timeText.GetComponent<Text>().text = "No Contract Signed";
            }
        }
        else
        {
            timeText.GetComponent<Text>().text = "";
        }
    }
}
