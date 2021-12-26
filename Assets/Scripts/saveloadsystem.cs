using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

[Serializable]
public class propertySaveForm {

    public string propName;
    public int locX;
    public int locY;
    public string signTime;
    public int signIndex;
    public propertySaveForm(string n, float[] v, string t = "notsigned", int i = -1)
    {
        propName = n; locX = (int)v[0]; locY = (int)v[1]; signTime = t; signIndex = i;
    }
}

[Serializable]
public class statsSaveForm
{
    public long money;
    public long gold;
    public long level;
    public long xp;

    public statsSaveForm(long m, long g, long l, long x)
    {
        money = m; gold = g; level = l; xp = x;
    }
}

public class saveloadsystem : MonoBehaviour
{
    public CSVReader csv;
    public Tilemap map;
    public GameObject PropertiesParent, Stats;

    public void Start()
    {
        print("Attempting to Load Save Game");
        loadSaveGame();
    }

    [ContextMenu("Save Game")]
    public void saveGame()
    {
        print("Saving Game");
        List<propertySaveForm> propSaveList = new List<propertySaveForm>();
        foreach (Transform child in PropertiesParent.transform)
        {
            if (child.GetComponent<Property>() != null) {
                print("saving: " + child.GetComponent<Property>().Card.displayName);
                if (child.GetComponent<Property>().Card.type == "House")
                {
                    propSaveList.Add(new propertySaveForm(child.GetComponent<Property>().Card.displayName, child.GetComponent<Draggable>().XY, child.transform.GetChild(0).GetComponent<contractScript>().signTime, child.transform.GetChild(0).GetComponent<contractScript>().signIndex));
                } else
                {
                    propSaveList.Add(new propertySaveForm(child.GetComponent<Property>().Card.displayName, child.GetComponent<Draggable>().XY));
                }
            }
        }
        FileHandler.SaveToJSON<propertySaveForm>(propSaveList, "propsSave.json");

        statsSaveForm statsSaveObj = new statsSaveForm(Stats.GetComponent<Statistics>().money, Stats.GetComponent<Statistics>().gold, Stats.GetComponent<Statistics>().level, Stats.GetComponent<Statistics>().xp);
        FileHandler.SaveToJSON<statsSaveForm>(statsSaveObj, "statsSave.json");
        print("Game saved");
    }

    [ContextMenu("Save Stats")]
    public void saveStats()
    {
        print("Saving Stats");
        statsSaveForm statsSaveObj = new statsSaveForm(Stats.GetComponent<Statistics>().money, Stats.GetComponent<Statistics>().gold, Stats.GetComponent<Statistics>().level, Stats.GetComponent<Statistics>().xp);
        FileHandler.SaveToJSON<statsSaveForm>(statsSaveObj, "statsSave.json");
        print("Stats saved");
    }

    [ContextMenu("Load From Save")]
    public void loadSaveGame()
    {
        List<propertySaveForm> list = new List<propertySaveForm>();
        list = FileHandler.ReadListFromJSON<propertySaveForm>("propsSave.json");
        statsSaveForm statsObj;
        statsObj = FileHandler.ReadFromJSON<statsSaveForm>("statsSave.json");
        
        if (list.Count == 0)
        {
            print("No Save Game found, loading new game");
            LoadNewGame();
            saveGame();
        }
        else
        {
            print("Save Game Found, loading from save");
            foreach (var p in list)
            {
                loadProperty(p.propName, new Vector2Int(p.locX, p.locY), p.signTime, p.signIndex);
                print("loaded: " + p.propName);
            }
            //neeed to load stats
            Stats.GetComponent<Statistics>().setStats(statsObj.money, statsObj.gold, statsObj.level, statsObj.xp);
        }
    }

    [ContextMenu("Load New Game")]
    public void LoadNewGame()
    {
        print("Loading Default Properties");
        loadProperty("Japanese Tree", new Vector2Int(-1, -2));
        loadProperty("Fountain", new Vector2Int(-1, -4));
        loadProperty("Pizzeria", new Vector2Int(-7, 0));
        loadProperty("Townhouse", new Vector2Int(-5, -4), "notsigned");
        loadProperty("Cypress Tree", new Vector2Int(2, -4));
        loadProperty("Bungalow", new Vector2Int(3, -4), "notsigned");
        loadProperty("Townhouse Luxury", new Vector2Int(6, 0), "notsigned");
        loadProperty("Bungalow Luxury", new Vector2Int(-11, -3), "notsigned");
        loadProperty("Cypress Tree", new Vector2Int(2, -7));
        print("Successfully Loaded Game");
    }

    public void loadProperty(string propName, Vector2Int pos, string signTime = "notsigned", int signIndex = -1) //propName must be the display form, not camelCase; eg Bungalow Luxury, not bungalowlux
    {
        //print("loading and spawning property into game from load save");
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
        pp.GetComponent<SpriteRenderer>().sortingOrder = 1;
        string loc = "(" + (pos.x) + "," + (pos.y) + ")";
        pp.name += loc;

        // adding components
        pp.gameObject.AddComponent<Draggable>();
        pp.gameObject.GetComponent<Draggable>().XY = new[] { (float)pos.x, (float)pos.y};
        pp.GetComponent<Draggable>().dragEnabled = false;
        pp.gameObject.AddComponent<BlinkingProperty>();
        pp.gameObject.GetComponent<BlinkingProperty>().StopBlink();
        if (pp.Card.type == "House") // check to only do these thats only for houses
        {
            pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime = signTime;
            pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = signIndex;
            // adding collider to contract and sorting its order ------
            pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
            // add collider to money
            pp.transform.GetChild(1).gameObject.AddComponent<BoxCollider2D>();
            var dateAndTimeVar = System.DateTime.Now;
            print("going check contract for " + pp.name + "which is signtime " + pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime);
            if (pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime == "notsigned") {
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                print("notsigned");
            } else if (dateAndTimeVar >= DateTime.Parse(pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime)) {
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                print("sign over timea alre");
            } else if (dateAndTimeVar < DateTime.Parse(pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime)) {
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            } else {
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = signIndex;
                print("sign still ongoiing");
            }
            
        }
        //print("Successfully loaded in " + propName + " at: x:" + pos.x + "y:" + pos.y);
    }
}
