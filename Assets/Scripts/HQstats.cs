using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HQstats : MonoBehaviour
{
    [SerializeField]
    private GameObject hqMenu, shopMenu, infoPanel, externalAudioPlayer, dragButtons, PropertiesParent;

    [SerializeField]
    private AudioClip touchSound;

    [SerializeField]
    private Statistics stats;

    [SerializeField]
    private Text money, gold, property, land, total, cityName;

    public long totalLong;

    public InputField field;

    public Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void calcHQ()
    {
        print("calculating hq");
        money.text = "$" + stats.money.ToString("#,##0");
        long goldvalue = stats.gold * 60000;
        gold.text = "$" + goldvalue.ToString("#,##0");
        long propValue = 0;
        foreach (Transform child in PropertiesParent.transform)
        {
            if (child.gameObject.GetComponent<Property>() != null)
            {
                if (child.gameObject.GetComponent<Property>().Card.cost.Contains("Gold"))
                {
                    propValue += int.Parse(child.gameObject.GetComponent<Property>().Card.cost.Remove(child.gameObject.GetComponent<Property>().Card.cost.Length - 5)) * 60000;
                }
                else
                {
                    propValue += int.Parse(child.gameObject.GetComponent<Property>().Card.cost);
                }
            }
        }
        property.text = "$" + propValue.ToString("#,##0");
        land.text = "$" + ((stats.GetComponent<Statistics>().noOfPlots*1000) + stats.GetComponent<Statistics>().expCost).ToString("#,##0");
        totalLong = (stats.money + goldvalue + (stats.GetComponent<Statistics>().noOfPlots * 1000) + propValue);
        total.text = "$" + (stats.money + goldvalue + (stats.GetComponent<Statistics>().noOfPlots *1000) + propValue).ToString("#,##0");
        field.text = stats.cityName;
    }

    public void clickedHQ()
    {
        if (mainCam.GetComponent<CameraMovement>().dragging == false)
        {
            print("open hq");
            if (hqMenu.activeSelf == false && dragButtons.activeSelf == false)
            {
                calcHQ();
                hqMenu.SetActive(true);
                hqMenu.transform.localScale = Vector2.zero;
                hqMenu.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
                externalAudioPlayer.GetComponent<AudioSource>().PlayOneShot(touchSound);
                infoPanel.SetActive(false);
                shopMenu.SetActive(false);
            }
            else
            {
                closePanel();
            }
        }
    }

    public void closePanel()
    {
        hqMenu.transform.LeanScale(Vector2.zero, 0.2f).setEaseInBack();
        Invoke("setInactive", 0.2f);
    }

    void setInactive()
    {
        hqMenu.gameObject.SetActive(false);
        hqMenu.gameObject.transform.localScale = Vector2.one;
    }
}

