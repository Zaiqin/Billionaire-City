using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class UIToggle : MonoBehaviour
{

    [SerializeField]
    GameObject shopToggle, plotToggle, roadToggle, deleteToggle, ronaldToggle, storageToggle, luckToggle, signToggle, moveToggle, settingsToggle, ShopMenu, hqMenu, infoPanel, pendingParent, ppDrag, delPanel, contractPanel, expPopup, failExpPopup, convertMoneyPanel, dailyBonusPanel, missionPanel, storageMenu, instantRentPanel, globalInsuff;

    GameObject selectedToggle;
    public GameObject[] toggles;

    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private TileBase greenGrass, tileGrass;

    public void toggleToggles(GameObject sel)
    {
        print("Selected " + sel);
        if (GameObject.Find("infoPanel") != null)
        {
            GameObject highlightedProp = GameObject.Find("infoPanel").GetComponent<infoScript>().highlightedProp;
            highlightedProp.GetComponent<SpriteRenderer>().material.color = Color.white;
            if (highlightedProp.GetComponent<Property>().Card.type == "House" && highlightedProp.transform.childCount == 5)
            {
                Destroy(highlightedProp.transform.GetChild(4).gameObject);
            }
        }

        if (sel == shopToggle)
        {
            roadToggle.GetComponent<Toggle>().isOn = false;
            plotToggle.GetComponent<Toggle>().isOn = false;
            deleteToggle.GetComponent<Toggle>().isOn = false;
            ronaldToggle.GetComponent<Toggle>().isOn = false;
            storageToggle.GetComponent<Toggle>().isOn = false;
            luckToggle.GetComponent<Toggle>().isOn = false;
            moveToggle.GetComponent<Toggle>().isOn = false;
            storageMenu.SetActive(false);
            print("disabling grid");
            map.SwapTile(greenGrass, tileGrass); //disable grid
        }
        if (sel == roadToggle)
        {
            shopToggle.GetComponent<Toggle>().isOn = false;
            plotToggle.GetComponent<Toggle>().isOn = false;
            deleteToggle.GetComponent<Toggle>().isOn = false;
            ronaldToggle.GetComponent<Toggle>().isOn = false;
            storageToggle.GetComponent<Toggle>().isOn = false;
            luckToggle.GetComponent<Toggle>().isOn = false;
            moveToggle.GetComponent<Toggle>().isOn = false;
            ShopMenu.SetActive(false);
            storageMenu.SetActive(false);
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
            storageToggle.GetComponent<Toggle>().isOn = false;
            luckToggle.GetComponent<Toggle>().isOn = false;
            moveToggle.GetComponent<Toggle>().isOn = false;
            ShopMenu.SetActive(false);
            storageMenu.SetActive(false);
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
            storageToggle.GetComponent<Toggle>().isOn = false;
            luckToggle.GetComponent<Toggle>().isOn = false;
            moveToggle.GetComponent<Toggle>().isOn = false;
            ShopMenu.SetActive(false);
            storageMenu.SetActive(false);
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
            deleteToggle.GetComponent<Toggle>().isOn = false;
            storageToggle.GetComponent<Toggle>().isOn = false;
            luckToggle.GetComponent<Toggle>().isOn = false;
            moveToggle.GetComponent<Toggle>().isOn = false;
            ShopMenu.SetActive(false);
            storageMenu.SetActive(false);
            print("disabling grid");
            map.SwapTile(greenGrass, tileGrass); //disable grid
        }
        if (sel == storageToggle)
        {
            roadToggle.GetComponent<Toggle>().isOn = false;
            plotToggle.GetComponent<Toggle>().isOn = false;
            shopToggle.GetComponent<Toggle>().isOn = false;
            deleteToggle.GetComponent<Toggle>().isOn = false;
            luckToggle.GetComponent<Toggle>().isOn = false;
            moveToggle.GetComponent<Toggle>().isOn = false;
            ShopMenu.SetActive(false);
            print("disabling grid");
            map.SwapTile(greenGrass, tileGrass); //disable grid
        }
        if (sel == luckToggle)
        {
            roadToggle.GetComponent<Toggle>().isOn = false;
            plotToggle.GetComponent<Toggle>().isOn = false;
            shopToggle.GetComponent<Toggle>().isOn = false;
            deleteToggle.GetComponent<Toggle>().isOn = false;
            storageToggle.GetComponent<Toggle>().isOn = false;
            moveToggle.GetComponent<Toggle>().isOn = false;
            ShopMenu.SetActive(false);
            print("disabling grid");
            map.SwapTile(greenGrass, tileGrass); //disable grid
        }
        if (sel == signToggle)
        {
            roadToggle.GetComponent<Toggle>().isOn = false;
            plotToggle.GetComponent<Toggle>().isOn = false;
            shopToggle.GetComponent<Toggle>().isOn = false;
            deleteToggle.GetComponent<Toggle>().isOn = false;
            ronaldToggle.GetComponent<Toggle>().isOn = false;
            storageToggle.GetComponent<Toggle>().isOn = false;
            luckToggle.GetComponent<Toggle>().isOn = false;
            moveToggle.GetComponent<Toggle>().isOn = false;
            storageMenu.SetActive(false);
            ShopMenu.SetActive(false);
            print("disabling grid");
            map.SwapTile(greenGrass, tileGrass); //disable grid
        }
        if (sel == moveToggle)
        {
            roadToggle.GetComponent<Toggle>().isOn = false;
            plotToggle.GetComponent<Toggle>().isOn = false;
            shopToggle.GetComponent<Toggle>().isOn = false;
            deleteToggle.GetComponent<Toggle>().isOn = false;
            ronaldToggle.GetComponent<Toggle>().isOn = false;
            storageToggle.GetComponent<Toggle>().isOn = false;
            luckToggle.GetComponent<Toggle>().isOn = false;
            storageMenu.SetActive(false);
            ShopMenu.SetActive(false);
            print("disabling grid");
            map.SwapTile(greenGrass, tileGrass); //disable grid
        }
        if (sel == settingsToggle)
        {
            roadToggle.GetComponent<Toggle>().isOn = false;
            plotToggle.GetComponent<Toggle>().isOn = false;
            shopToggle.GetComponent<Toggle>().isOn = false;
            deleteToggle.GetComponent<Toggle>().isOn = false;
            storageToggle.GetComponent<Toggle>().isOn = false;
            luckToggle.GetComponent<Toggle>().isOn = false;
            moveToggle.GetComponent<Toggle>().isOn = false;
            ronaldToggle.GetComponent<Toggle>().isOn = false;
            ShopMenu.SetActive(false);
            storageMenu.SetActive(false);
            print("disabling grid");
            map.SwapTile(greenGrass, tileGrass); //disable grid
        }

        hqMenu.SetActive(false);
        infoPanel.SetActive(false);
        delPanel.SetActive(false);
        contractPanel.SetActive(false);
        expPopup.SetActive(false);
        failExpPopup.SetActive(false);
        dailyBonusPanel.SetActive(false);
        convertMoneyPanel.SetActive(false);
        missionPanel.SetActive(false);
        instantRentPanel.SetActive(false);
        globalInsuff.SetActive(false);
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
