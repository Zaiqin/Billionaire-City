using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HQstats : MonoBehaviour
{
    [SerializeField]
    private GameObject hqMenu, shopMenu, infoPanel, externalAudioPlayer, dragButtons, PropertiesParent, coyValue, coyName;

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

    public void calcHQ()
    {
        print("calculating hq");

        stats.updateName();
        money.text = "$" + stats.money.ToString("#,##0");
        long goldvalue = stats.gold * 60000;
        gold.text = "$" + goldvalue.ToString("#,##0");
        long propValue = 0;
        foreach (Transform child in PropertiesParent.transform)
        {
            if (child.gameObject.GetComponent<Property>() != null)
            {
                print("calculating: " + child.gameObject.name);
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
        totalLong = (stats.money + goldvalue + (stats.GetComponent<Statistics>().noOfPlots * 1000) + propValue + stats.GetComponent<Statistics>().expCost);
        total.text = "$" + (totalLong).ToString("#,##0");
        coyValue.GetComponent<Text>().text = total.text;
        coyName.GetComponent<Text>().text = stats.cityName;
        field.text = stats.cityName;
        print("money: " + stats.money.ToString("#,##0") + "gold:" + goldvalue.ToString("#,##0")+"propValue:"+ propValue.ToString("#,##0")+"plots:"+ (stats.GetComponent<Statistics>().noOfPlots * 1000) + "expCost:"+ stats.GetComponent<Statistics>().expCost);
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

