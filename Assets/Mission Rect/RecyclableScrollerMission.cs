﻿using System.Collections.Generic;
using UnityEngine;
using PolyAndCode.UI;
using UnityEngine.UI;

/// <summary>
/// Demo controller class for Recyclable Scroll Rect. 
/// A controller class is responsible for providing the scroll rect with datasource. Any class can be a controller class. 
/// The only requirement is to inherit from IRecyclableScrollRectDataSource and implement the interface methods
/// </summary>

public class RecyclableScrollerMission : CSVReader, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;
    [SerializeField]
    private RecyclableScrollRect missionRect;

    private string[] missionArray = new string[]{"Pizzalicious", "Pizzalicious Clients I", "Pizzalicious Sales I", "Name It", "Instant Build", "Green Thumb I", "M-City's VIP Club"};

    //Recyclable scroll rect's data source must be assigned in Awake.
    private void Awake()
    {
        _recyclableScrollRect.DataSource = this;
    }

    public void userReloadData()
    {
        missionRect.ReloadData();
    }

    #region DATA-SOURCE

    /// <summary>
    /// Data source method. return the list length.
    /// </summary>
    public int GetItemCount()
    {
        return 7;
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
        var item = cell as MissionCell;

        item.ConfigureCell(index, missionArray[index]);
    }

    #endregion
}