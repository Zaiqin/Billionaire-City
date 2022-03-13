using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using PolyAndCode.UI;

public class shopButton : MonoBehaviour
{
    public GameObject ShopMenu, ppDrag;
    public RecyclableScrollRect rect;
    public Toggle shopToggle;

    public bool requireReload = false;

    public void OnButtonClick()
    {
        if (shopToggle.isOn == true && ppDrag.activeSelf == true)
        {
            shopToggle.isOn = false;
            closePanel();
        }

        if (ShopMenu.activeSelf == false && shopToggle.isOn == true)
        {
            print("opening shop");
            ShopMenu.SetActive(true);
            ShopMenu.transform.localScale = Vector2.zero;
            ShopMenu.transform.LeanScale(new Vector2(73.9463f, 73.9463f), 0.2f).setEaseOutBack();
            if (requireReload == true)
            {
                rect.ReloadData();
                requireReload = false;
            }
            
        } else if (ShopMenu.activeSelf == true && shopToggle.isOn == false)
        {
            closePanel();
        }
        
    }

    public void closePanel()
    {
        ShopMenu.transform.LeanScale(Vector2.zero, 0.2f).setEaseInBack();
        Invoke("setInactive", 0.2f);
    }

    void setInactive()
    {
        ShopMenu.SetActive(false);
        ShopMenu.transform.localScale = new Vector2(73.9463f, 73.9463f);
    }
}
