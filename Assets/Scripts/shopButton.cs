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
            ShopMenu.SetActive(false);
        }

        if (ShopMenu.activeSelf == false && shopToggle.isOn == true)
        {
            ShopMenu.SetActive(true);
            if (requireReload == true)
            {
                rect.ReloadData();
                requireReload = false;
            }
            
        } else if (ShopMenu.activeSelf == true && shopToggle.isOn == false)
        {
            ShopMenu.SetActive(false);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
