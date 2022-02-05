using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using PolyAndCode.UI;

public class storageToggle : MonoBehaviour
{
    public GameObject StorageMenu;
    //public RecyclableScrollRect rect;
    public Toggle storageTog;

    public void OnButtonClick()
    {
        if (StorageMenu.activeSelf == false)
        {
            storageTog.isOn = true;
            StorageMenu.SetActive(true);

        }
        else if (StorageMenu.activeSelf == true)
        {
            print("offing");
            storageTog.isOn = false;
            StorageMenu.SetActive(false);
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
