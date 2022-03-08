using System.Collections.Generic;
using UnityEngine;
using PolyAndCode.UI;
using UnityEngine.UI;

/// <summary>
/// Demo controller class for Recyclable Scroll Rect. 
/// A controller class is responsible for providing the scroll rect with datasource. Any class can be a controller class. 
/// The only requirement is to inherit from IRecyclableScrollRectDataSource and implement the interface methods
/// </summary>

public class RecyclableScrollerDemo : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    [SerializeField]
    private CSVReader CSVObject;

    [SerializeField]
    private TabGroup tabGroup;

    PropertyCard[][] pCardArrays;

    //Recyclable scroll rect's data source must be assigned in Awake.
    private void Awake()
    {
        _recyclableScrollRect.DataSource = this;
    }

    public void initCSV()
    {
        StartCoroutine(CSVObject.ReadCSV());
        pCardArrays = CSVObject.propertyCardArrays;
    }

    #region DATA-SOURCE

    /// <summary>
    /// Data source method. return the list length.
    /// </summary>
    public int GetItemCount()
    {
        //print("retrieving no of entries");
        switch (tabGroup.selectedTab.buttonName)
        {
            case "houseTab":
                return pCardArrays[0].Length;
            case "commerceTab":
                return pCardArrays[1].Length;
            case "decoTab":
                return pCardArrays[2].Length;
            case "wonderTab":
                return pCardArrays[3].Length;
            default: return pCardArrays[0].Length;
        }
    }

    /// <summary>
    /// Data source method. Called for a cell every time it is recycled.
    /// Implement this method to do the necessary cell configuration.
    /// </summary>
    public void SetCell(ICell cell, int index)
    {
        //Casting to the implemented Cell
        //print("in recycablescrolldemo setting cell");
        //print("in recycablescrolldemo selected tab now is " + tabGroup.selectedTab.buttonName);
        var item = cell as DemoCell;
        switch (tabGroup.selectedTab.buttonName)
        {
            case "houseTab":
                item.ConfigureCell(pCardArrays[0][index], index, "House"); break;
            case "commerceTab":
                item.ConfigureCell(pCardArrays[1][index], index, "Commerce"); break;
            case "decoTab":
                item.ConfigureCell(pCardArrays[2][index], index, "Deco"); break;
            case "wonderTab":
                item.ConfigureCell(pCardArrays[3][index], index, "Wonder"); break;
            default:
                item.ConfigureCell(pCardArrays[0][index], index, "House"); break;
        }
        item.infoParent.SetActive(false);
    }

    #endregion
}