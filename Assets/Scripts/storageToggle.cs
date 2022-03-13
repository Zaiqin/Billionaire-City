using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using PolyAndCode.UI;

public class storageToggle : MonoBehaviour
{
    public GameObject StorageMenu;
    public GameObject storageController;
    public Text count;
    //public RecyclableScrollRect rect;
    public Toggle storageTog;

    public void OnButtonClick()
    {
        if (StorageMenu.activeSelf == false)
        {
            storageTog.isOn = true;
            StorageMenu.SetActive(true);
            StorageMenu.transform.localScale = Vector2.zero;
            StorageMenu.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
            if (storageController.GetComponent<RecyclableScrollerStorage>().storageList.Count == 0)
            {
                count.text = "No items";
            }
            else if (storageController.GetComponent<RecyclableScrollerStorage>().storageList.Count == 1)
            {
                count.text = "1 item";
            }
            else
            {
                count.text = storageController.GetComponent<RecyclableScrollerStorage>().storageList.Count + " items";
            }

        }
        else if (StorageMenu.activeSelf == true)
        {
            print("offing");
            storageTog.isOn = false;
            StorageMenu.transform.LeanScale(Vector2.zero, 0.2f).setEaseInBack();
            Invoke("setInactive", 0.2f);
        }

    }

    public void closePanel()
    {
        print("clsoe luck");
        StorageMenu.transform.LeanScale(Vector2.zero, 0.2f).setEaseInBack();
        Invoke("setInactive", 0.2f);
    }

    void setInactive()
    {
        StorageMenu.SetActive(false);
        StorageMenu.transform.localScale = Vector2.one;
    }
}
