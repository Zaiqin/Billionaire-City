using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class signController : MonoBehaviour
{
    public GameObject selProperty, saveObj, stats, contractsParent, extAudio, cover, signToggle;
    public AudioClip contractSound;

    public void signer(int i)
    {
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
        if (stats.GetComponent<Statistics>().money >= cost)
        {
            cover.SetActive(false);
            signToggle.GetComponent<Toggle>().isOn = false;
            print("signing property " + selProperty.name + "with sign index: " + i);
            selProperty.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            closePanel();
            string datetime = theTime.ToString("yyyy/MM/dd HH:mm:ss");
            selProperty.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime = datetime;
            selProperty.transform.GetChild(0).gameObject.GetComponent<contractScript>().signCreationTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            print("sign time is " + datetime);
            stats.GetComponent<Statistics>().updateStats(diffmoney: -cost);
            saveObj.GetComponent<saveloadsystem>().saveGame();
            extAudio.GetComponent<AudioSource>().PlayOneShot(contractSound);

            GameObject value = Instantiate(Resources.Load<GameObject>("floatingParent"), new Vector3(selProperty.transform.position.x + (float.Parse(selProperty.GetComponent<Property>().Card.space.Substring(0, 1))) / 2, selProperty.transform.position.y + 2.8f, -5f), Quaternion.identity) as GameObject;
            value.transform.GetChild(0).GetComponent<TextMesh>().text = "- $" + cost;
            value.transform.GetChild(0).GetComponent<TextMesh>().color = new Color(255f / 255f, 76f / 255f, 76f / 255f);

            GameObject contractPopup = Instantiate(Resources.Load<GameObject>("contractValue"), new Vector3(selProperty.transform.position.x + (float.Parse(selProperty.GetComponent<Property>().Card.space.Substring(0, 1))) / 2, selProperty.transform.position.y + ((float.Parse(selProperty.GetComponent<Property>().Card.space.Substring(selProperty.GetComponent<Property>().Card.space.Length-1)))/2), -5f), Quaternion.identity) as GameObject;

        } else
        {
            print("no money to sign contract");
        }
    }

    public void closePanel()
    {
        contractsParent.transform.LeanScale(Vector2.zero, 0.2f).setEaseInBack();
        Invoke("setInactive", 0.2f);
    }

    void setInactive()
    {
        contractsParent.gameObject.SetActive(false);
        contractsParent.gameObject.transform.localScale = Vector2.one;
    }
}
