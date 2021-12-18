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
        Property pp = new Property();

        pp.init(prop, contractStarSprite); //Creation of the Property

        // Finding and spawning at center of the screen, add name of object
        Vector2 centerScreenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3Int cellPos = map.WorldToCell(centerScreenPos);
        pp.obj.transform.position = new Vector3((float)(cellPos.x), (float)(cellPos.y), -8f); //bottom left tile of hq is tile 0,0
        pp.obj.transform.parent = PendingPropertyParent.transform;
        pp.obj.transform.position += new Vector3(-1f, 1.64f, 0f); //Offset vector so that bottom left tile is the tilename that the property is on
        pp.obj.transform.localScale = new Vector2(1f, 1f);
        string loc = "(" + (cellPos.x-1) + "," + (cellPos.y+1) + ")";
        pp.obj.name += loc;

        // adding components
        pp.obj.AddComponent<Draggable>();
        pp.obj.GetComponent<Draggable>().dragEnabled = true;
        pp.obj.AddComponent<BlinkingProperty>();
        pp.obj.GetComponent<BlinkingProperty>().StartBlink();
        pp.obj.GetComponent<PropertyStats>().pCard = prop;  

        // unhide propertyDrag buttons and placing it at grid center of property
        ppDrag.SetActive(true);
        ppDrag.transform.position = new Vector3(pp.obj.transform.position.x + (float.Parse(prop.space.Substring(0, 1))) / 2, pp.obj.transform.position.y - 1f, ppDrag.transform.position.z);
        
    }

    
}
