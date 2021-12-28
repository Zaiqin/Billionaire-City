using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class signController : MonoBehaviour
{
    public GameObject selProperty, saveObj, stats;

    public void signer(int i)
    {
        print("signing property " + selProperty.name);
        selProperty.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        print("setting index to " + i);
        DateTime theTime; long cost;
        switch (i)
        {
            case 0: theTime = DateTime.Now.AddMinutes(3); cost = 100; selProperty.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = i; break;
            case 1: theTime = DateTime.Now.AddMinutes(30); cost = 580; selProperty.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = i; break;
            case 2: theTime = DateTime.Now.AddHours(1); cost = 870; selProperty.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = i; break;
            case 3: theTime = DateTime.Now.AddHours(4); cost = 1250; selProperty.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = i; break;
            case 4: theTime = DateTime.Now.AddHours(8); cost = 1450; selProperty.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = i; break;
            case 5: theTime = DateTime.Now.AddHours(12); cost = 4350; selProperty.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = i; break;
            case 6: theTime = DateTime.Now.AddDays(1); cost = 5800; selProperty.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = i; break;
            case 7: theTime = DateTime.Now.AddDays(2); cost = 7250; selProperty.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = i; break;
            case 8: theTime = DateTime.Now.AddDays(3); cost = 10000; selProperty.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = i; break;
            default: theTime = DateTime.Now.AddMinutes(3); cost = 100; selProperty.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = i; break;
        }
        string datetime = theTime.ToString("yyyy/MM/dd HH:mm:ss");
        selProperty.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime = datetime;
        selProperty.transform.GetChild(0).gameObject.GetComponent<contractScript>().signCreationTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        print("sign time is " + datetime);
        stats.GetComponent<Statistics>().updateStats(diffmoney: -cost);
        saveObj.GetComponent<saveloadsystem>().saveGame();
    }
}
