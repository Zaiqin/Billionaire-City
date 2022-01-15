using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.UI;
using System.IO;

[Serializable]
public class propertySaveForm {

    public string propName;
    public int locX;
    public int locY;
    public string signTime;
    public int signIndex;
    public string signCreationTime;
    public string comSignTime;
    public string comSignCreationTime;
    public propertySaveForm(string n, float[] v, string t = "notsigned", int i = -1, string ct = "notsigned", string comt = "notsigned", string comct = "notsigned")
    {
        propName = n; locX = (int)v[0]; locY = (int)v[1]; signTime = t; signIndex = i; signCreationTime = ct; comSignTime = comt; comSignCreationTime = comct;
    }
}

[Serializable]
public class statsSaveForm
{
    public long money;
    public long gold;
    public long level;
    public long xp;
    public string cityName;
    public int noOfPlots;
    public statsSaveForm(long m, long g, long l, long x, string n, int p)
    {
        money = m; gold = g; level = l; xp = x; cityName = n; noOfPlots = p;
    }
}

[Serializable]
public class tileSaveForm
{
    public float x;
    public float y;
    public string texName;

    public tileSaveForm(Vector3Int v, string texture)
    {
        x = v.x; y = v.y; 
        if (texture == "greenGrass")
        {
            texName = "tileGrass";
        } else
        {
            texName = texture;
        }
            
    }
}

public class saveloadsystem : MonoBehaviour
{
    public CSVReader csv;
    public Tilemap map;
    public GameObject PropertiesParent, Stats, expPopup, cityText, hq;
    public InputField nameField;

    public void Start()
    {
        //print("Attempting to Load Save Game");
        string testString = FileHandler.ReadRawFromJSON("propsSave.json");
        //print("raw string is " + testString);

        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        {
            Debug.Log("First Time Opening");
            print("Deleting any fake copy");
            foreach (var directory in Directory.GetDirectories(Application.persistentDataPath))
            {
                DirectoryInfo data_dir = new DirectoryInfo(directory);
                data_dir.Delete(true);
            }

            foreach (var file in Directory.GetFiles(Application.persistentDataPath))
            {
                FileInfo file_info = new FileInfo(file);
                file_info.Delete();
            }
            LoadNewGame();
            saveGame();

            //Set first time opening to false
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);

            //Do your stuff here

        }
        else
        {
            Debug.Log("NOT First Time Opening");

            //Do your stuff here
            loadSaveGame();
        }

        /*string prevString = "2022/01/15 11:32:01";
        if (testString.Contains(prevString))
        {
            print("detected fake copy");
            foreach (var directory in Directory.GetDirectories(Application.persistentDataPath))
            {
                DirectoryInfo data_dir = new DirectoryInfo(directory);
                data_dir.Delete(true);
            }

            foreach (var file in Directory.GetFiles(Application.persistentDataPath))
            {
                FileInfo file_info = new FileInfo(file);
                file_info.Delete();
            }
            LoadNewGame();
            saveGame();
        }
        else
        {
            loadSaveGame();
        }*/
    }

    [ContextMenu("Save Game")]
    public void saveGame()
    {
        print("Saving Game");
        // ------------ Saving properties -------------------------------
        List<propertySaveForm> propSaveList = new List<propertySaveForm>();
        foreach (Transform child in PropertiesParent.transform)
        {
            if (child.GetComponent<Property>() != null) {
                //print("saving: " + child.GetComponent<Property>().Card.displayName);
                if (child.GetComponent<Property>().Card.type == "House")
                {
                    propSaveList.Add(new propertySaveForm(child.GetComponent<Property>().Card.displayName, child.GetComponent<Draggable>().XY, child.transform.GetChild(0).GetComponent<contractScript>().signTime, child.transform.GetChild(0).GetComponent<contractScript>().signIndex, child.transform.GetChild(0).GetComponent<contractScript>().signCreationTime));
                } else if (child.GetComponent<Property>().Card.type == "Commerce")
                {
                    propSaveList.Add(new propertySaveForm(child.GetComponent<Property>().Card.displayName, child.GetComponent<Draggable>().XY, comt: child.transform.GetChild(1).GetComponent<commercePickupScript>().signTime, comct: child.transform.GetChild(1).GetComponent<commercePickupScript>().signCreationTime));
                } else
                {
                    propSaveList.Add(new propertySaveForm(child.GetComponent<Property>().Card.displayName, child.GetComponent<Draggable>().XY));
                }
            }
        }
        FileHandler.SaveToJSON<propertySaveForm>(propSaveList, "propsSave.json");

        // ------------ Saving stats -------------------------------
        statsSaveForm statsSaveObj = new statsSaveForm(Stats.GetComponent<Statistics>().money, Stats.GetComponent<Statistics>().gold, Stats.GetComponent<Statistics>().level, Stats.GetComponent<Statistics>().xp, cityText.GetComponent<Text>().text, Stats.GetComponent<Statistics>().noOfPlots);
        FileHandler.SaveToJSON<statsSaveForm>(statsSaveObj, "statsSave.json");

        // ------------ Saving tilemap -------------------------------
        List<tileSaveForm> tileSaveList = new List<tileSaveForm>();
        BoundsInt bounds = map.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tile = map.GetTile(pos);
                if (tile != null)
                {
                    tileSaveForm tF = new tileSaveForm(pos, tile.name);
                    tileSaveList.Add(tF);
                }
            }
        }
        FileHandler.SaveToJSON<tileSaveForm>(tileSaveList, "tileSave.json");

        // ------------- Save Expansions ---------------------
        List<string> delExpList = expPopup.GetComponent<expansion>().deletedExp;
        FileHandler.SaveToJSON<string>(delExpList, "deletedExp.json");
        print("Game saved");
    }

    [ContextMenu("Save Stats")]
    public void saveStats()
    {
        print("Saving Stats");
        statsSaveForm statsSaveObj = new statsSaveForm(Stats.GetComponent<Statistics>().money, Stats.GetComponent<Statistics>().gold, Stats.GetComponent<Statistics>().level, Stats.GetComponent<Statistics>().xp, cityText.GetComponent<Text>().text, Stats.GetComponent<Statistics>().noOfPlots);
        FileHandler.SaveToJSON<statsSaveForm>(statsSaveObj, "statsSave.json");
        print("Stats saved");
    }

    [ContextMenu("Save Tilemap")]
    public void saveTilemap()
    {
        print("Saving Tilemap");
        List<tileSaveForm> tileSaveList = new List<tileSaveForm>();
        BoundsInt bounds = map.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tile = map.GetTile(pos);
                if (tile != null)
                {
                    tileSaveForm tF = new tileSaveForm(pos, tile.name);
                    tileSaveList.Add(tF);
                }
            }
        }
        FileHandler.SaveToJSON<tileSaveForm>(tileSaveList, "tileSave.json");
        print("Tilemap saved");
    }
    
    [ContextMenu("Save Expansions")]
    public void saveExp()
    {
        print("Saving expansions");
        List<string> delExpList = expPopup.GetComponent<expansion>().deletedExp;
        FileHandler.SaveToJSON<string>(delExpList, "deletedExp.json");
    }

    [ContextMenu("Load From Save")]
    public void loadSaveGame()
    {
        List<propertySaveForm> list = new List<propertySaveForm>();
        list = FileHandler.ReadListFromJSON<propertySaveForm>("propsSave.json");
        statsSaveForm statsObj;
        statsObj = FileHandler.ReadFromJSON<statsSaveForm>("statsSave.json");
        List<tileSaveForm> tilelist = new List<tileSaveForm>();
        tilelist = FileHandler.ReadListFromJSON<tileSaveForm>("tileSave.json");

        if (list.Count == 0)
        {
            print("No Save Game found, loading new game");
            LoadNewGame();
            saveGame();
        }
        else
        {
            print("Save Game Found, loading from save");
            // ----------- Loading Properties ---------------
            foreach (var p in list)
            {
                loadProperty(p.propName, new Vector2Int(p.locX, p.locY), p.signTime, p.signIndex, p.signCreationTime, p.comSignTime, p.comSignCreationTime);
                //print("loaded: " + p.propName);
            }
            // ----------- Loading Statistics ---------------
            Stats.GetComponent<Statistics>().setStats(statsObj.money, statsObj.gold, statsObj.level, statsObj.xp);
            Stats.GetComponent<Statistics>().cityName = statsObj.cityName;
            Stats.GetComponent<Statistics>().noOfPlots = statsObj.noOfPlots;
            print("loaded name is " + statsObj.cityName);
            nameField.text = statsObj.cityName;
            // ----------- Loading Tilemap ------------------
            foreach (var item in tilelist)
            {
                Vector3Int pos = new Vector3Int((int)item.x, (int)item.y, 0);
                if (item.texName.Contains("road") || item.texName.Contains("Grass") || item.texName.Contains("Below"))
                {
                    map.SetTile(pos, Resources.Load<TileBase>("roadTiles/" + item.texName));
                    //print("loaded road" + item.texName + "at pos " + pos);
                } else if (item.texName.Contains("plot"))
                {
                    map.SetTile(pos, Resources.Load<TileBase>("plotTiles/" + item.texName));
                    //print("loaded plot" + item.texName + "at pos " + pos);
                }
            }
            // ------------ Loading Expansions -------------
            expPopup.GetComponent<expansion>().deletedExp = FileHandler.ReadListFromJSON<string>("deletedExp.json");
            foreach (string s in FileHandler.ReadListFromJSON<string>("deletedExp.json"))
            {
                Destroy(GameObject.Find(s));
            }
        }
    }

    [ContextMenu("Load New Game")]
    public void LoadNewGame()
    {
        Stats.GetComponent<Statistics>().noOfPlots = 35;
        Stats.GetComponent<Statistics>().cityName = "Chocolate Fields";
        nameField.text = "Chocolate Fields";
        //print("Loading Default Properties");
        loadProperty("Japanese Tree", new Vector2Int(-1, -2));
        loadProperty("Fountain", new Vector2Int(-1, -4));
        loadProperty("Pizzeria", new Vector2Int(-7, 0));
        loadProperty("Townhouse", new Vector2Int(-5, -4), "notsigned");
        loadProperty("Cypress Tree", new Vector2Int(2, -4));
        loadProperty("Bungalow", new Vector2Int(3, -4), "notsigned");
        loadProperty("Townhouse Luxury", new Vector2Int(6, 0), "notsigned");
        loadProperty("Bungalow Luxury", new Vector2Int(-11, -3), "notsigned");
        loadProperty("Cypress Tree", new Vector2Int(2, -7));
        print("Successfully Loaded New Game");
    }

    public void loadProperty(string propName, Vector2Int pos, string signTime = "notsigned", int signIndex = -1, string signCreationTime = "notsigned", string comSignTime = "notsigned", string comSignCreationTime = "notsigned") //propName must be the display form, not camelCase; eg Bungalow Luxury, not bungalowlux
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
        pp.transform.position = new Vector3((float)(pos.x + 1), (float)(pos.y - 1), zPos); //bottom left tile of hq is tile 0,0
        pp.transform.parent = PropertiesParent.transform;
        pp.transform.position += new Vector3(-1f, 1.64f, 0f); //Offset vector so that bottom left tile is the tilename that the property is on
        pp.transform.localScale = new Vector2(1f, 1f);
        pp.GetComponent<SpriteRenderer>().sortingOrder = 1;
        string loc = "(" + (pos.x) + "," + (pos.y) + ")";
        pp.name += loc;

        // adding components
        pp.gameObject.AddComponent<Draggable>();
        pp.gameObject.GetComponent<Draggable>().XY = new[] { (float)pos.x, (float)pos.y };
        pp.GetComponent<Draggable>().dragEnabled = false;
        if (pp.Card.type == "House") // check to only do these thats only for houses
        {
            pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime = signTime;
            pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = signIndex;
            // adding collider to contract and sorting its order ------
            pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
            // add collider to money
            pp.transform.GetChild(1).gameObject.AddComponent<BoxCollider2D>();
            // add collider to deco detect influence --------------
            pp.transform.GetChild(2).gameObject.AddComponent<BoxCollider2D>();

            var dateAndTimeVar = System.DateTime.Now;
            //print("going check contract for " + pp.name + "which is signtime " + pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime);
            if (pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime == "notsigned")
            {
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signCreationTime = "notsigned";
                //print("notsigned");
            }
            else if (dateAndTimeVar >= DateTime.Parse(pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime))
            {
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signCreationTime = signCreationTime;
                //print("sign over timea alre");
            }
            else if (dateAndTimeVar < DateTime.Parse(pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime))
            {
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signCreationTime = signCreationTime;
            }
            else
            {
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = signIndex;
                pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signCreationTime = signCreationTime;
                //print("sign still ongoiing");
            }

        }
        else if (pp.Card.type == "Commerce")
        {
            //print("loadproperty commerce");
            if (pp.transform.GetChild(0).GetComponent<BoxCollider2D>() == null)
            {
                pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
            }
            pp.transform.GetChild(0).gameObject.SetActive(false);
            pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime = comSignTime;
            // add collider to commerce money ----------
            pp.transform.GetChild(1).gameObject.AddComponent<BoxCollider2D>();
            var dateAndTimeVar = System.DateTime.Now;
            //print("going check contract for " + pp.name + "which is signtime " + pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime);
            if (pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime == "notsigned")
            {
                //print("notsigned, thus signing now");
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

                DateTime theTime;
                theTime = DateTime.Now.AddMinutes(3);
                //print("signing property commerce again");
                string datetime = theTime.ToString("yyyy/MM/dd HH:mm:ss");
                pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime = datetime;
                pp.transform.GetChild(1).GetComponent<commercePickupScript>().signCreationTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                //print("sign time is " + datetime);

            }
            else if (dateAndTimeVar >= DateTime.Parse(pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime))
            {
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signCreationTime = comSignCreationTime;
                //print("sign over time already");
            }
            else if (dateAndTimeVar < DateTime.Parse(pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime))
            {
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signCreationTime = comSignCreationTime;
            }
            else
            {
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signCreationTime = comSignCreationTime;
                //print("sign still ongoiing");
            }
        }
        else if (pp.Card.type == "Deco") // check to only do these thats only for houses
        {
            pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
            pp.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
