using System.Collections.Generic;
using UnityEngine;
using PolyAndCode.UI;
using UnityEngine.UI;

/// <summary>
/// Demo controller class for Recyclable Scroll Rect. 
/// A controller class is responsible for providing the scroll rect with datasource. Any class can be a controller class. 
/// The only requirement is to inherit from IRecyclableScrollRectDataSource and implement the interface methods
/// </summary>

public class RecyclableScrollerContract : CSVReader, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    [SerializeField]
    Sprite threeMinSprite, thirtyMinSprite, oneHrSprite, fourHrSprite, eightHrSprite, twelveHrSprite, oneDaySprite, twoDaySprite, threeDaySprite;

    public PropertyCard pCard;
    public GameObject selProp;

    [SerializeField]
    private RecyclableScrollRect contractRect;

    //Recyclable scroll rect's data source must be assigned in Awake.
    private void Awake()
    {
        _recyclableScrollRect.DataSource = this;
    }

    public void userReloadData()
    {
        print("resetting data for contracts selection");
        contractRect.ReloadData();
    }

    #region DATA-SOURCE

    /// <summary>
    /// Data source method. return the list length.
    /// </summary>
    public int GetItemCount()
    {
        return 9;
    }

    /// <summary>
    /// Data source method. Called for a cell every time it is recycled.
    /// Implement this method to do the necessary cell configuration.
    /// </summary>
    public void SetCell(ICell cell, int index)
    {
        //Casting to the implemented Cell
        //print("in recycablescrollcontract setting cell");

        //print("Clicked on a " + pCard.displayName + "'s contract");
        var item = cell as ContractCell;
        Sprite setSprite;

        switch (index)
        {
            case 0: setSprite = threeMinSprite; break;
            case 1: setSprite = thirtyMinSprite; break;
            case 2: setSprite = oneHrSprite; break;
            case 3: setSprite = fourHrSprite; break;
            case 4: setSprite = eightHrSprite; break;
            case 5: setSprite = twelveHrSprite; break;
            case 6: setSprite = oneDaySprite; break;
            case 7: setSprite = twoDaySprite; break;
            case 8: setSprite = threeDaySprite; break;
            default: setSprite = threeMinSprite;  break;
        }
        item.ConfigureCell(setSprite, pCard, index, selProp);
    }

    #endregion
}