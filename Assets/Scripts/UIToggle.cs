using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class UIToggle : MonoBehaviour
{

    [SerializeField]
    GameObject shopToggle, plotToggle, roadToggle, deleteToggle, ronaldToggle, ShopMenu, levelUpScreen, hqMenu, infoPanel, pendingParent, ppDrag, delPanel, contractPanel, expPopup, failExpPopup;

    GameObject selectedToggle;
    public GameObject[] toggles;

    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private TileBase greenGrass, tileGrass;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleToggles(GameObject sel)
    {
        print("Selected " + sel);

        if (sel == shopToggle)
        {
            roadToggle.GetComponent<Toggle>().isOn = false;
            plotToggle.GetComponent<Toggle>().isOn = false;
            deleteToggle.GetComponent<Toggle>().isOn = false;
            ronaldToggle.GetComponent<Toggle>().isOn = false;
            print("disabling grid");
            map.SwapTile(greenGrass, tileGrass); //disable grid
        }
        if (sel == roadToggle)
        {
            shopToggle.GetComponent<Toggle>().isOn = false;
            plotToggle.GetComponent<Toggle>().isOn = false;
            deleteToggle.GetComponent<Toggle>().isOn = false;
            ronaldToggle.GetComponent<Toggle>().isOn = false;
            ShopMenu.SetActive(false);
            if (roadToggle.GetComponent<Toggle>().isOn == true)
            {
                print("enabling grid");
                map.SwapTile(tileGrass, greenGrass); //enable grid
            }
            else
            {
                print("disabling grid");
                map.SwapTile(greenGrass, tileGrass); //disable grid
            }
        }
        if (sel == plotToggle)
        {
            shopToggle.GetComponent<Toggle>().isOn = false;
            roadToggle.GetComponent<Toggle>().isOn = false;
            deleteToggle.GetComponent<Toggle>().isOn = false;
            ronaldToggle.GetComponent<Toggle>().isOn = false;
            ShopMenu.SetActive(false);
            if (plotToggle.GetComponent<Toggle>().isOn == true)
            {
                print("enabling grid");
                map.SwapTile(tileGrass, greenGrass); //enable grid
            }
            else
            {
                print("disabling grid");
                map.SwapTile(greenGrass, tileGrass); //disable grid
            }
        }
        if (sel == deleteToggle)
        {
            roadToggle.GetComponent<Toggle>().isOn = false;
            plotToggle.GetComponent<Toggle>().isOn = false;
            shopToggle.GetComponent<Toggle>().isOn = false;
            ronaldToggle.GetComponent<Toggle>().isOn = false;
            ShopMenu.SetActive(false);
            if (deleteToggle.GetComponent<Toggle>().isOn == true)
            {
                print("enabling grid");
                map.SwapTile(tileGrass, greenGrass); //enable grid
            }
            else
            {
                print("disabling grid");
                map.SwapTile(greenGrass, tileGrass); //disable grid
            }
        }
        if (sel == ronaldToggle)
        {
            roadToggle.GetComponent<Toggle>().isOn = false;
            plotToggle.GetComponent<Toggle>().isOn = false;
            shopToggle.GetComponent<Toggle>().isOn = false;
            shopToggle.GetComponent<Toggle>().isOn = false;
            ShopMenu.SetActive(false);
            print("disabling grid");
            map.SwapTile(greenGrass, tileGrass); //disable grid
        }

        hqMenu.SetActive(false);
        infoPanel.SetActive(false);
        delPanel.SetActive(false);
        contractPanel.SetActive(false);
        expPopup.SetActive(false);
        failExpPopup.SetActive(false);
        if (delPanel.transform.GetChild(2).GetComponent<delConfirm>().selProp != null)
        {
            delPanel.transform.GetChild(2).GetComponent<delConfirm>().selProp.GetComponent<SpriteRenderer>().color = Color.white;
        }

        // Destroy any pending properties
        if (pendingParent.transform.childCount != 0) {
            Destroy(pendingParent.transform.GetChild(0).gameObject);
            ppDrag.SetActive(false);
        }
    }

}
