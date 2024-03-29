using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PurchaseController : MonoBehaviour
{

    [SerializeField]
    public Tilemap map;

    [SerializeField]
    public GameObject PropertiesParent, PendingPropertyParent, ppDrag;
    public GameObject shopToggle, plotToggle, roadToggle, deleteToggle, ronaldToggle, storageToggle, luckToggle, signToggle, moveToggle, settingsToggle, moneyBar, goldBar, xpBar, coyName, coyValue, neighbourBar;

    [SerializeField]
    Sprite contractStarSprite;

    public void hideUI()
    {
        shopToggle.SetActive(false);
        plotToggle.SetActive(false);
        roadToggle.SetActive(false);
        deleteToggle.SetActive(false);
        ronaldToggle.SetActive(false);
        storageToggle.SetActive(false);
        luckToggle.SetActive(false);
        signToggle.SetActive(false);
        moveToggle.SetActive(false);
        settingsToggle.SetActive(false);
        moneyBar.SetActive(false);
        goldBar.SetActive(false);
        xpBar.SetActive(false);
        coyName.SetActive(false);
        coyValue.SetActive(false);
        neighbourBar.SetActive(false);
    }

    public void showUI()
    {
        shopToggle.SetActive(true);
        plotToggle.SetActive(true);
        roadToggle.SetActive(true);
        deleteToggle.SetActive(true);
        ronaldToggle.SetActive(true);
        storageToggle.SetActive(true);
        luckToggle.SetActive(true);
        signToggle.SetActive(true);
        moveToggle.SetActive(true);
        settingsToggle.SetActive(true);
        moneyBar.SetActive(true);
        goldBar.SetActive(true);
        xpBar.SetActive(true);
        coyName.SetActive(true);
        coyValue.SetActive(true);
        neighbourBar.SetActive(true);
    }

    public void visitNeighbour()
    {
        shopToggle.SetActive(false);
        plotToggle.SetActive(false);
        roadToggle.SetActive(false);
        deleteToggle.SetActive(false);
        ronaldToggle.SetActive(false);
        storageToggle.SetActive(false);
        luckToggle.SetActive(false);
        signToggle.SetActive(false);
        moveToggle.SetActive(false);
        settingsToggle.SetActive(false);
        coyName.SetActive(true);
        coyValue.SetActive(true);
        neighbourBar.SetActive(true);
    }

    public void quitNeighbour()
    {
        shopToggle.SetActive(true);
        plotToggle.SetActive(true);
        roadToggle.SetActive(true);
        deleteToggle.SetActive(true);
        ronaldToggle.SetActive(true);
        storageToggle.SetActive(true);
        luckToggle.SetActive(true);
        signToggle.SetActive(true);
        moveToggle.SetActive(true);
        settingsToggle.SetActive(true);
        coyName.SetActive(true);
        coyValue.SetActive(true);
        neighbourBar.SetActive(true);
    }

    public void purchaseProperty(PropertyCard prop) //called when buy button is pressed with sufficient balance
    {
        print("spawning property into game");
        GameObject obj = new GameObject(); // Creating game object for property
        
        // Adding property component and initialising it
        obj.AddComponent<Property>();
        Property pp = obj.GetComponent<Property>();
        pp.initialise(prop);

        // Finding and spawning at center of the screen, add name of object
        Vector2 centerScreenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3Int cellPos = map.WorldToCell(centerScreenPos);
        pp.transform.position = new Vector3((float)(cellPos.x), (float)(cellPos.y), -8f); //bottom left tile of hq is tile 0,0
        pp.transform.parent = PendingPropertyParent.transform;
        pp.transform.position += new Vector3(-1f, 1.64f, 0f); //Offset vector so that bottom left tile is the tilename that the property is on
        pp.transform.localScale = new Vector2(1f, 1f);
        string loc = "(" + (cellPos.x-1) + "," + (cellPos.y+1) + ")";
        pp.name += loc;

        // adding components
        pp.gameObject.AddComponent<Draggable>();
        pp.GetComponent<Draggable>().dragEnabled = true;
        pp.GetComponent<Draggable>().XY =  new [] { (float)(cellPos.x - 1), (float)(cellPos.y + 1)};
        pp.gameObject.AddComponent<BlinkingProperty>();
        bool check = pp.GetComponent<Draggable>().buildCheck(prop, new [] { (float)(cellPos.x - 1), (float)(cellPos.y + 1 )}, map);
        print("check is " + check);
        if (check == true)
        {
            print("true checl stuff");
            pp.GetComponent<BlinkingProperty>().StopBlink();
            pp.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (check == false)
        {
            print("false check stuff");
            pp.GetComponent<BlinkingProperty>().invokeStart();
        }

        float[] xy = new[] { (float)cellPos.x - 1, (float)cellPos.y + 1 };
        //pp.gameObject.GetComponent<Draggable>().buildCheck(pp.Card,xy); check if can build at where it first spawned

        // unhide propertyDrag buttons and placing it at grid center of property
        ppDrag.SetActive(true);
        ppDrag.transform.position = new Vector3(pp.transform.position.x + (float.Parse(prop.space.Substring(0, 1))) / 2, pp.transform.position.y - 0.5f, ppDrag.transform.position.z);

        hideUI();
    }

    
}
