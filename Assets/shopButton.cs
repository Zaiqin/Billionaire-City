using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using PolyAndCode.UI;

public class shopButton : MonoBehaviour
{
    public GameObject ShopMenu;
    public RecyclableScrollRect rect;

    public void OnButtonClick()
    {
        if (ShopMenu.activeSelf == false)
        {
            ShopMenu.SetActive(true);
            rect.ReloadData();
            
        } else if (ShopMenu.activeSelf == true)
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
