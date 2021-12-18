using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class saveloadsystem : MonoBehaviour
{
    public CSVReader csv;
    public Tilemap map;
    public GameObject PropertiesParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [ContextMenu("Load Game")]
    public void LoadMyGame()
    {
        print("Loading Default Properties");
        loadProperty("Japanese Tree", new Vector2Int(-1, -2));
        loadProperty("Fountain", new Vector2Int(-1, -4));
        loadProperty("Pizzeria", new Vector2Int(-7, 0));
        loadProperty("Townhouse", new Vector2Int(-5, -4));
        loadProperty("Cypress Tree", new Vector2Int(2, -4));
        loadProperty("Bungalow", new Vector2Int(3, -4));
        loadProperty("Townhouse Luxury", new Vector2Int(6, 0));
        loadProperty("Bungalow Luxury", new Vector2Int(-11, -3));
        loadProperty("Cypress Tree", new Vector2Int(2, -7));
        print("Successfully Loaded Game");
    }

    public void loadProperty(string propName, Vector2Int pos) //propName must be the display form, not camelCase; eg Bungalow Luxury, not bungalowlux
    {
        print("loading and spawning property into game from load save");
        PropertyCard prop = csv.CardDatabase[propName];
        GameObject obj = new GameObject(); // Creating game object for property

        // Adding property component and initialising it
        obj.AddComponent<Property>();
        Property pp = obj.GetComponent<Property>();
        pp.initialise(prop);

        // getting z pos
        float[] coords = { (float)pos.x, (float)pos.y };
        float xFactor = (coords[0] -= 45) / 1000;
        float yFactor = coords[1] / 10;
        float zPos = xFactor + yFactor;

        // spawning property, add name of object
        pp.transform.position = new Vector3((float)(pos.x+1), (float)(pos.y-1), zPos); //bottom left tile of hq is tile 0,0
        pp.transform.parent = PropertiesParent.transform;
        pp.transform.position += new Vector3(-1f, 1.64f, 0f); //Offset vector so that bottom left tile is the tilename that the property is on
        pp.transform.localScale = new Vector2(1f, 1f);
        string loc = "(" + (pos.x) + "," + (pos.y) + ")";
        pp.name += loc;

        // adding components
        pp.gameObject.AddComponent<Draggable>();
        pp.GetComponent<Draggable>().dragEnabled = false;
        pp.gameObject.AddComponent<BlinkingProperty>();
        pp.gameObject.GetComponent<BlinkingProperty>().StopBlink();

        if (pp.Card.type == "House") // check to only do these thats only for houses
        {
            // adding collider to contract and sorting its order ------
            pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
            pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2; //shows contract
        }
        print("Successfully loaded in " + propName + "at: x:" + pos.x + "y:" + pos.y);
    }
}
