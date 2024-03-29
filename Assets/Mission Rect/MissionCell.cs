﻿using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using System;

//Cell class for demo. A cell in Recyclable Scroll Rect must have a cell class inheriting from ICell.
//The class is required to configure the cell(updating UI elements etc) according to the data during recycling of cells.
//The configuration of a cell is done through the DataSource SetCellData method.
//Check RecyclableScrollerDemo class
public class MissionCell : MonoBehaviour, ICell
{
    //UI
    public Button readMoreButton;
    public Text missionName;
    public Image bgImage;
    public Image missionImg;

    public AudioClip touchSound;

    public Sprite type0, type1, type2, type3, type4, type5, type6, typeError;

    //Model
    private int _cellIndex;

    private void Start()
    {
        readMoreButton.GetComponent<Button>().onClick.AddListener(readListener);
    }


    private void readListener()
    {
        GameObject.Find("ExternalAudioPlayer").GetComponent<AudioSource>().PlayOneShot(touchSound);
        GameObject.Find("missionParent").GetComponent<missionParent>().toggleDesc(_cellIndex);
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(int cellIndex, string text, bool pending, float type)
    {
        _cellIndex = cellIndex;
        missionName.text = text;
        if (pending == true)
        {
            bgImage.color = Color.green;
        } else
        {
            bgImage.color = Color.white;
        }
        
        switch (Math.Truncate(type))
        {
            case 0: missionImg.sprite = type0; break;
            case 1: missionImg.sprite = type1; break;
            case 2: missionImg.sprite = type2; break;
            case 3: missionImg.sprite = type3; break;
            case 4: missionImg.sprite = type4; break;
            case 5: missionImg.sprite = type5; break;
            case 6: missionImg.sprite = type6; break;
            default: missionImg.sprite = typeError; break;
        }
    }

}
