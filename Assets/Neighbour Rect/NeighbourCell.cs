using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

//Cell class for demo. A cell in Recyclable Scroll Rect must have a cell class inheriting from ICell.
//The class is required to configure the cell(updating UI elements etc) according to the data during recycling of cells.
//The configuration of a cell is done through the DataSource SetCellData method.
//Check RecyclableScrollerDemo class
public class NeighbourCell : MonoBehaviour, ICell
{
    //UI
    public Button profile;
    public Text itemTitle;
    public Text coyValue;
    public Image levelBg;
    public Text level;
    public Image border;

    public Sprite ronaldImage, xpStar, me, blank;

    [SerializeField]
    private AudioClip touchSound;

    //Model
    private int _cellIndex;

    private void Start()
    {
        profile.GetComponent<Button>().onClick.AddListener(touchListener);
    }

    private void touchListener()
    {
        print("touched neighbour in cellindex " + _cellIndex);
        if (itemTitle.text == "Ronald")
        {
            GameObject.Find("neighbourParent").GetComponent<neighbourScript>().OnButtonClick();
            GameObject.Find("ExternalAudioPlayer").GetComponent<AudioSource>().PlayOneShot(touchSound);
        }
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(int cellIndex, int total)
    {
        Statistics stats = GameObject.Find("Stats").GetComponent<Statistics>();
        _cellIndex = cellIndex;
        if (stats.coyValue > 100000000)
        {
            if (cellIndex == 0)
            {
                itemTitle.text = stats.cityName;
                levelBg.gameObject.SetActive(true);
                profile.GetComponent<Image>().sprite = me;
                string temp = stats.coyValue.ToString("#.##0");
                if (stats.coyValue >= 1000000000000)
                {
                    coyValue.text = "$" + (stats.coyValue / (double)1000000000).ToString("G5") + "B";
                }
                else if (stats.coyValue >= 1000000000)
                {
                    coyValue.text = "$" + (stats.coyValue / (double)1000000000).ToString("G3") + "B";
                }
                else if (stats.coyValue >= 1000000)
                {
                    coyValue.text = "$" + (stats.coyValue / (double)1000000).ToString("G3") + "M";
                }
                else if (stats.coyValue >= 1000)
                {
                    coyValue.text = "$" + (stats.coyValue / (double)1000).ToString("G3") + "K";
                }
                else
                {
                    coyValue.text = "$" + stats.coyValue;
                }
                level.text = stats.level.ToString();
            }
            else if (cellIndex == 1)
            {
                itemTitle.text = "Ronald";
                coyValue.text = "$100M";
                level.text = "23";
                levelBg.gameObject.SetActive(true);
                border.gameObject.SetActive(false);
            }
            else
            {
                profile.GetComponent<Image>().sprite = blank;
                itemTitle.text = "";
                coyValue.text = "";
                level.text = "";
                levelBg.gameObject.SetActive(false);
                border.gameObject.SetActive(false);
            }
        } else
        {
            if (cellIndex == 0)
            {
                itemTitle.text = "Ronald";
                coyValue.text = "$100M";
                level.text = "23";
                levelBg.gameObject.SetActive(true);
                border.gameObject.SetActive(false);
            }
            else if (cellIndex == 1)
            {
                itemTitle.text = stats.cityName;
                levelBg.gameObject.SetActive(true);
                profile.GetComponent<Image>().sprite = me;
                string temp = stats.coyValue.ToString("#.##0");
                if (stats.coyValue >= 1000000000000)
                {
                    coyValue.text = "$" + (stats.coyValue / (double)1000000000).ToString("G5") + "B";
                }
                else if (stats.coyValue >= 1000000000)
                {
                    coyValue.text = "$" + (stats.coyValue / (double)1000000000).ToString("G3") + "B";
                }
                else if (stats.coyValue >= 1000000)
                {
                    coyValue.text = "$" + (stats.coyValue / (double)1000000).ToString("G3") + "M";
                }
                else if (stats.coyValue >= 1000)
                {
                    coyValue.text = "$" + (stats.coyValue / (double)1000).ToString("G3") + "K";
                }
                else
                {
                    coyValue.text = "$" + stats.coyValue;
                }
                level.text = stats.level.ToString();
            }
            else
            {
                profile.GetComponent<Image>().sprite = blank;
                itemTitle.text = "";
                coyValue.text = "";
                level.text = "";
                levelBg.gameObject.SetActive(false);
                border.gameObject.SetActive(false);
            }
        }
        
        
    }

}
