using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggle : MonoBehaviour
{

    [SerializeField]
    GameObject shopToggle, plotToggle, roadToggle, deleteToggle, ronaldToggle, ShopMenu, levelUpScreen, hqMenu, infoPanel;

    GameObject selectedToggle;
    public GameObject[] toggles;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetToggles(GameObject sel)
    {
        print("resetting toggles");
        print("selected " + sel);

        if (sel != shopToggle)
        {
            shopToggle.GetComponent<Toggle>().isOn = false;
            ShopMenu.SetActive(false);
        }
        if (sel != plotToggle)
        {
            plotToggle.GetComponent<Toggle>().isOn = false;
        }
        if (sel != roadToggle)
        {
            roadToggle.GetComponent<Toggle>().isOn = false;
        }
        if (sel != deleteToggle)
        {
            deleteToggle.GetComponent<Toggle>().isOn = false;
        }
        if (sel != ronaldToggle)
        {
            levelUpScreen.SetActive(false);
            ronaldToggle.GetComponent<Toggle>().isOn = false;
        }
        hqMenu.SetActive(false);
        infoPanel.SetActive(false);
    }
}
