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
public class StorageCell : MonoBehaviour, ICell
{
    //UI
    public Button useButton;
    public Image itemImage;
    public Text itemText;

    public Tilemap map;
    public TileBase greenGrass;
    public TileBase tileGrass;

    [SerializeField]
    private AudioClip touchSound;

    //Model
    private int _cellIndex;
    private PropertyCard _propertyCard;

    private void Start()
    {
        useButton.GetComponent<Button>().onClick.AddListener(useListener);
    }

    private void useListener()
    {
        print("used reedemable");
        GameObject.Find("ExternalAudioPlayer").GetComponent<AudioSource>().PlayOneShot(touchSound);
        GameObject shopMenu = GameObject.Find("Storage Scroll Controller").GetComponent<RecyclableScrollerStorage>().shopMenu;
        GameObject storageMenu = GameObject.Find("storageParent");
        storageMenu.SetActive(false);
        shopMenu.GetComponent<PurchaseController>().purchaseProperty(_propertyCard);
        // --------------------- Swapping to green border grass -------------
        map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        map.SwapTile(tileGrass, greenGrass);
        // ------------------------------------------------------------------
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(int cellIndex, PropertyCard card)
    {
        _cellIndex = cellIndex;
        _propertyCard = card;
        itemImage.sprite = card.propImage;
        itemText.text = card.displayName;
    }

}
