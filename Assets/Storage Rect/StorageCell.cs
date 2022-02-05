using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using System;
using System.Collections.Generic;

//Cell class for demo. A cell in Recyclable Scroll Rect must have a cell class inheriting from ICell.
//The class is required to configure the cell(updating UI elements etc) according to the data during recycling of cells.
//The configuration of a cell is done through the DataSource SetCellData method.
//Check RecyclableScrollerDemo class
public class StorageCell : MonoBehaviour, ICell
{
    //UI
    public Button useButton;

    [SerializeField]
    private AudioClip touchSound;

    //Model
    private int _cellIndex;

    private void Start()
    {
        useButton.GetComponent<Button>().onClick.AddListener(useListener);
    }

    private void useListener()
    {
        print("used reedemable");
        GameObject.Find("ExternalAudioPlayer").GetComponent<AudioSource>().PlayOneShot(touchSound);
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(int cellIndex)
    {
        _cellIndex = cellIndex;
    }

}
