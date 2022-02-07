using System.Collections.Generic;
using UnityEngine;
using PolyAndCode.UI;
using UnityEngine.UI;

/// <summary>
/// Demo controller class for Recyclable Scroll Rect. 
/// A controller class is responsible for providing the scroll rect with datasource. Any class can be a controller class. 
/// The only requirement is to inherit from IRecyclableScrollRectDataSource and implement the interface methods
/// </summary>

public class RecyclableScrollerStorage : CSVReader, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    [SerializeField]
    private RecyclableScrollRect storageRect;

    public GameObject shopMenu, saveObj;

    [SerializeField]
    private CSVReader CSVObject;
    private Dictionary<string, PropertyCard> database;

    public string inputName;

    public List<string> storageList = new List<string>() {};

    //Recyclable scroll rect's data source must be assigned in Awake.
    private void Awake()
    {
        database = CSVObject.CardDatabase;
        _recyclableScrollRect.DataSource = this;

    }

    public void userReloadData()
    {
        storageRect.ReloadData();
    }

    public void deleteFromStorage(string s)
    {
        storageList.Remove(s);
        saveObj.GetComponent<saveloadsystem>().saveStorage();
    }

    [ContextMenu("Add to Storage List")]
    public void addIntoStorage()
    {
        print("Added " + inputName + " into storage");
        storageList.Add(inputName);
        saveObj.GetComponent<saveloadsystem>().saveStorage();
    }

    [ContextMenu("Check Storage List")]
    public void checkstorage()
    {
        foreach (var item in storageList)
        {
            print("there is " + item + " in the storage");
        }
    }

    #region DATA-SOURCE

    /// <summary>
    /// Data source method. return the list length.
    /// </summary>
    public int GetItemCount()
    {
        return storageList.Count;
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
        var item = cell as StorageCell;

        item.ConfigureCell(index, database[storageList[index]]);
    }

    #endregion
}