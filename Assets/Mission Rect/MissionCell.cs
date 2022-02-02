using UnityEngine;
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
    public Image bgImage;
    public Button infoButton, rewardButton;

    //Model
    private int _cellIndex;

    private void Start()
    {
        
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(Sprite bgSprite, int cellIndex)
    {
        _cellIndex = cellIndex;
        bgImage.sprite = bgSprite;
    }

}
