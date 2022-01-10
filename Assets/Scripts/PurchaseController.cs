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

    [SerializeField]
    Sprite contractStarSprite;

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
        pp.gameObject.AddComponent<BlinkingProperty>();
        pp.GetComponent<BlinkingProperty>().StartBlink();
        /*bool check = pp.GetComponent<Draggable>().buildCheck(prop, new [] { (float)(cellPos.x - 1), (float)(cellPos.y + 1 )});
        if (check == true)
        {
            pp.gameObject.GetComponent<BlinkingProperty>().StopBlink();
            pp.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }*/

        float[] xy = new[] { (float)cellPos.x - 1, (float)cellPos.y + 1 };
        //pp.gameObject.GetComponent<Draggable>().buildCheck(pp.Card,xy); check if can build at where it first spawned

        // unhide propertyDrag buttons and placing it at grid center of property
        ppDrag.SetActive(true);
        ppDrag.transform.position = new Vector3(pp.transform.position.x + (float.Parse(prop.space.Substring(0, 1))) / 2, pp.transform.position.y - 1f, ppDrag.transform.position.z);
        
    }

    
}
