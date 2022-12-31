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

    public Sprite ronaldImage, xpStar, me;

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
        if (_cellIndex == 1)
        {
            GameObject.Find("neighbourParent").GetComponent<neighbourScript>().OnButtonClick();
            GameObject.Find("ExternalAudioPlayer").GetComponent<AudioSource>().PlayOneShot(touchSound);
        }
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(int cellIndex)
    {
        Statistics stats = GameObject.Find("Stats").GetComponent<Statistics>();
        _cellIndex = cellIndex;
        switch (cellIndex)
        {
            case 0: itemTitle.text = stats.cityName;
                profile.GetComponent<Image>().sprite = me;
                string temp = stats.coyValue.ToString("#.##0");
                if (stats.coyValue >= 1000000000000)
                {
                    coyValue.text = "$" + (stats.coyValue / (double)1000000000).ToString("G5") + "B";
                } else if (stats.coyValue >= 1000000000)
                {
                    coyValue.text = "$" + (stats.coyValue / (double)1000000000).ToString("G3") + "B";
                } else if (stats.coyValue >= 1000000)
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
                level.text = stats.level.ToString(); break;
            case 1: itemTitle.text = "Ronald"; coyValue.text = "$100M"; level.text = "23"; break;
            default: itemTitle.text = "Player"; coyValue.text = "$919K"; level.text = "1"; break;
        }
    }

}
