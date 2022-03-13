using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class luckToggle : MonoBehaviour
{
    public GameObject luckPanel, csvObj;
    public Image rew1, rew3, rew5, rew6, rew7;
    public void OnButtonClick()
    {
        luckPanel.SetActive(true);
        luckPanel.transform.localScale = Vector2.zero;
        luckPanel.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();

        rew1.sprite = csvObj.GetComponent<CSVReader>().CardDatabase["Three Storey"].propImage;
        rew3.sprite = csvObj.GetComponent<CSVReader>().CardDatabase["Bungalow Luxury"].propImage;
        rew5.sprite = csvObj.GetComponent<CSVReader>().CardDatabase["Coffee Shop"].propImage;
        rew6.sprite = csvObj.GetComponent<CSVReader>().CardDatabase["Orange Tree"].propImage;
        rew7.sprite = csvObj.GetComponent<CSVReader>().CardDatabase["Apartment Block"].propImage;
    }

    public void closeLuckPanel()
    {
        print("clsoe luck");
        luckPanel.transform.LeanScale(Vector2.zero, 0.2f).setEaseInBack();
        Invoke("setInactive", 0.2f);
    }

    void setInactive()
    {
        luckPanel.SetActive(false);
        luckPanel.transform.localScale = Vector2.one;
    }
}
