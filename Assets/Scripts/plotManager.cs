using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class plotManager : MonoBehaviour
{

    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;

    [SerializeField]
    private GameObject stats, shopToggle;

    [SerializeField]
    private Toggle plotToggle, deleteToggle, roadToggle;

    public AudioSource myFx;
    public AudioClip deleteSound;

    public GameObject floatingValue, hq, splashObject, saveObj, pendingParent;

    public Camera mainCam;

    private bool startInUI;

    private bool isMouseOverUI() //return true if mouse is over ui
    {
        //print("ismosueoverui result is " + EventSystem.current.IsPointerOverGameObject());

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<Draggable>() != null)
            {
                //print("mouseoverui hit building named " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.GetComponent<Draggable>().dragEnabled == true)
                {
                    //print("hit building that is draggable");
                    return true;
                }
                else
                {
                    //print("hit building that is not draggable");
                    return true;
                }
            }
            if (hit.collider.gameObject.layer == 6)
            {
                //print("return true for layer 6");
                return true;
            }
            //print("hit someting that has collider");
            return true;
        }
        else
        {
            //print("no collider detected");
        }
        //print("returning false");
        return false;
    }

    private void Start()
    {

    }

    private void Awake()
    {

        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }

    }


    public void plotFunction(Vector3Int input, bool delete, bool forced = false)
    {
        print("plotFunction called");
        int cycle = 1;
        int i = 1;
        Vector3Int gridPosition = input;

        // For the deduction of $1000 for building plot
        Tile initialCenterTile = map.GetTile<Tile>(gridPosition);

        while ((i < 15) && cycle < 3) // cycle 1 extra time to reupdate clicked tile
        {
            gridPosition = input;
            switch (i)
            {
                case 2: gridPosition = new Vector3Int(gridPosition.x - 1, gridPosition.y + 1, gridPosition.z); break;
                case 3: gridPosition = new Vector3Int(gridPosition.x, gridPosition.y + 1, gridPosition.z); break;
                case 4: gridPosition = new Vector3Int(gridPosition.x + 1, gridPosition.y + 1, gridPosition.z); break;
                case 5: gridPosition = new Vector3Int(gridPosition.x - 1, gridPosition.y, gridPosition.z); break;
                case 6: gridPosition = new Vector3Int(gridPosition.x + 1, gridPosition.y, gridPosition.z); break;
                case 7: gridPosition = new Vector3Int(gridPosition.x - 1, gridPosition.y - 1, gridPosition.z); break;
                case 8: gridPosition = new Vector3Int(gridPosition.x, gridPosition.y - 1, gridPosition.z); break;
                case 9: gridPosition = new Vector3Int(gridPosition.x + 1, gridPosition.y - 1, gridPosition.z); break;
                case 10: gridPosition = new Vector3Int(gridPosition.x - 2, gridPosition.y, gridPosition.z); break;
                case 11: gridPosition = new Vector3Int(gridPosition.x, gridPosition.y + 2, gridPosition.z); break;
                case 12: gridPosition = new Vector3Int(gridPosition.x + 2, gridPosition.y, gridPosition.z); break;
                case 13: gridPosition = new Vector3Int(gridPosition.x, gridPosition.y - 2, gridPosition.z); break;
                default: break;
            }
            Tile clickedTile = map.GetTile<Tile>(gridPosition);

            if (clickedTile != null)
            {
                print("clickedTile.name is " + clickedTile.name + ", tilebase is " + clickedTile);

                if (((i == 1) && clickedTile.name.Contains("greenGrass")) || ((i == 1) && (delete == true)) || ((i == 1) && forced == true) || (clickedTile.name.Contains("plot") && (i > 1)))
                {
                    string calcTile = plotTileChecker.calcTile(map, clickedTile, gridPosition.x, gridPosition.y);

                    if (((clickedTile.name != calcTile) && (delete == false)) || (delete == true) || (forced == true))
                    {

                        Tile createdTile = ScriptableObject.CreateInstance<Tile>();
                        string createPath = "plotTiles/" + calcTile;
                        createdTile.sprite = Resources.Load<Tile>(createPath).sprite;
                        createdTile.name = calcTile;

                        if (isMouseOverUI() == false && startInUI == false)
                        {
                            if (mainCam.GetComponent<CameraMovement>().dragging == false)
                            {
                                if ((i == 1) && (delete == true))
                                {
                                    map.SetTile(gridPosition, Resources.Load<TileBase>("roadTiles/greenGrass"));
                                }
                                else
                                {
                                    map.SetTile(gridPosition, createdTile);
                                }
                            }
                        } else if (forced == true)
                        {
                            print("forced set tile");
                            map.SetTile(gridPosition, createdTile);
                        }
                    }
                }
            }
            else
            {
                //print("clicked tile is nil");
            }

            i++;
            //print("i is now " + i);
            if ((i == 14) && (cycle == 1))
            {
                i = 1;
                cycle = 2;
            }
        }

        // For deduction and adding back $1k if removed plot but not used to build house;
        //print("initial center tile is " + initialCenterTile.name + ", final center tile is " + map.GetTile<Tile>(gridPosition).name);
        if (initialCenterTile.name.Contains("plot") && map.GetTile<Tile>(gridPosition).name.Contains("Grass") && forced == false)
        {
            stats.GetComponent<Statistics>().updateStats(diffmoney: 1000);
            stats.GetComponent<Statistics>().noOfPlots -= 1;
            //print("adding 1k");
            GameObject value = Instantiate(floatingValue, new Vector3(gridPosition.x + (float)0.5, (float)gridPosition.y + 2, (float)gridPosition.z), Quaternion.identity) as GameObject;
            value.transform.GetChild(0).GetComponent<TextMesh>().text = "+$1000";
            value.transform.GetChild(0).GetComponent<TextMesh>().color = new Color(168f/255f, 255f/255f, 4f/255f);
        }
        if (initialCenterTile.name.Contains("Grass") && map.GetTile<Tile>(gridPosition).name.Contains("plot"))
        {
            stats.GetComponent<Statistics>().updateStats(diffmoney: -1000);
            //print("deducting 1k");
            GameObject value = Instantiate(floatingValue, new Vector3(gridPosition.x+(float)0.5, (float)gridPosition.y+2, (float)gridPosition.z), Quaternion.identity) as GameObject;
            value.transform.GetChild(0).GetComponent<TextMesh>().text = "-$1000";
            value.transform.GetChild(0).GetComponent<TextMesh>().color = new Color(255f / 255f, 76f / 255f, 76f / 255f);
            stats.GetComponent<Statistics>().noOfPlots += 1;
            GameObject splash = Instantiate(splashObject, new Vector3(gridPosition.x + (float)0.5, gridPosition.y+(float)1.4, (float)gridPosition.z), Quaternion.identity) as GameObject;
        }
        //float walkingSpeed = dataFromTiles[clickedTile].walkingSpeed;

        //print("Walking speed on " + clickedTile + " at position " + gridPosition + " is " + walkingSpeed);
        saveObj.GetComponent<saveloadsystem>().saveTilemap();
        saveObj.GetComponent<saveloadsystem>().saveStats();
        
    }

    public void belowFunction(Vector3Int input, bool delete, bool forced = false)
    {
        print("belowFunction called");
        int cycle = 1;
        int i = 1;
        Vector3Int gridPosition = input;

        // For the deduction of $1000 for building plot
        Tile initialCenterTile = map.GetTile<Tile>(gridPosition);

        while ((i < 15) && cycle < 3) // cycle 1 extra time to reupdate clicked tile
        {
            gridPosition = input;
            switch (i)
            {
                case 2: gridPosition = new Vector3Int(gridPosition.x - 1, gridPosition.y + 1, gridPosition.z); break;
                case 3: gridPosition = new Vector3Int(gridPosition.x, gridPosition.y + 1, gridPosition.z); break;
                case 4: gridPosition = new Vector3Int(gridPosition.x + 1, gridPosition.y + 1, gridPosition.z); break;
                case 5: gridPosition = new Vector3Int(gridPosition.x - 1, gridPosition.y, gridPosition.z); break;
                case 6: gridPosition = new Vector3Int(gridPosition.x + 1, gridPosition.y, gridPosition.z); break;
                case 7: gridPosition = new Vector3Int(gridPosition.x - 1, gridPosition.y - 1, gridPosition.z); break;
                case 8: gridPosition = new Vector3Int(gridPosition.x, gridPosition.y - 1, gridPosition.z); break;
                case 9: gridPosition = new Vector3Int(gridPosition.x + 1, gridPosition.y - 1, gridPosition.z); break;
                case 10: gridPosition = new Vector3Int(gridPosition.x - 2, gridPosition.y, gridPosition.z); break;
                case 11: gridPosition = new Vector3Int(gridPosition.x, gridPosition.y + 2, gridPosition.z); break;
                case 12: gridPosition = new Vector3Int(gridPosition.x + 2, gridPosition.y, gridPosition.z); break;
                case 13: gridPosition = new Vector3Int(gridPosition.x, gridPosition.y - 2, gridPosition.z); break;
                default: break;
            }
            Tile clickedTile = map.GetTile<Tile>(gridPosition);

            if (clickedTile != null)
            {
                //print("clickedTile.name is " + clickedTile.name + ", tilebase is " + clickedTile);

                if (((i == 1) && clickedTile.name.Contains("greenGrass")) || ((i == 1) && (delete == true)) || (clickedTile.name.Contains("plot") && (i > 1)))
                {
                    string calcTile = plotTileChecker.calcTile(map, clickedTile, gridPosition.x, gridPosition.y);

                    Tile createdTile = ScriptableObject.CreateInstance<Tile>();
                    string createPath = "plotTiles/" + calcTile;
                    createdTile.sprite = Resources.Load<Tile>(createPath).sprite;
                    createdTile.name = calcTile;
                    if ((i == 1) && (delete == true))
                    {
                        print("setting to noBelow");
                        map.SetTile(gridPosition, Resources.Load<TileBase>("roadTiles/noBelow"));
                    }
                    else
                    {
                        print("setting to createdTile");
                        map.SetTile(gridPosition, createdTile);
                    }
                }
            }
            else
            {
                //print("clicked tile is nil");
            }

            i++;
            //print("i is now " + i);
            if ((i == 14) && (cycle == 1))
            {
                i = 1;
                cycle = 2;
            }
        }

        // For deduction and adding back $1k if removed plot but not used to build house;
        //print("initial center tile is " + initialCenterTile.name + ", final center tile is " + map.GetTile<Tile>(gridPosition).name);
        if (initialCenterTile.name.Contains("plot") && map.GetTile<Tile>(gridPosition).name.Contains("Grass") && forced == false)
        {
            stats.GetComponent<Statistics>().updateStats(diffmoney: 1000);
            stats.GetComponent<Statistics>().noOfPlots -= 1;
            //print("adding 1k");
            GameObject value = Instantiate(floatingValue, new Vector3(gridPosition.x + (float)0.5, (float)gridPosition.y + 2, (float)gridPosition.z), Quaternion.identity) as GameObject;
            value.transform.GetChild(0).GetComponent<TextMesh>().text = "+$1000";
            value.transform.GetChild(0).GetComponent<TextMesh>().color = new Color(168f / 255f, 255f / 255f, 4f / 255f);
        }
        if (initialCenterTile.name.Contains("Grass") && map.GetTile<Tile>(gridPosition).name.Contains("plot"))
        {
            stats.GetComponent<Statistics>().updateStats(diffmoney: -1000);
            //print("deducting 1k");
            GameObject value = Instantiate(floatingValue, new Vector3(gridPosition.x + (float)0.5, (float)gridPosition.y + 2, (float)gridPosition.z), Quaternion.identity) as GameObject;
            value.transform.GetChild(0).GetComponent<TextMesh>().text = "-$1000";
            value.transform.GetChild(0).GetComponent<TextMesh>().color = new Color(255f / 255f, 76f / 255f, 76f / 255f);
            stats.GetComponent<Statistics>().noOfPlots += 1;
            GameObject splash = Instantiate(splashObject, new Vector3(gridPosition.x + (float)0.5, gridPosition.y + (float)1.4, (float)gridPosition.z), Quaternion.identity) as GameObject;
        }
        //float walkingSpeed = dataFromTiles[clickedTile].walkingSpeed;

        //print("Walking speed on " + clickedTile + " at position " + gridPosition + " is " + walkingSpeed);
        if (forced == false)
        {
            saveObj.GetComponent<saveloadsystem>().saveTilemap();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isMouseOverUI() == true)
        {
            startInUI = true;
            //print("start in ui settteidaddi to true");
        }

        if ((plotToggle.isOn == true) && (Input.GetMouseButtonUp(0)))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            Tile clickedTile = map.GetTile<Tile>(gridPosition);
            if (clickedTile.name.Contains("greenGrass") && stats.GetComponent<Statistics>().money >= 1000)
            {
                plotFunction(gridPosition, false);
            }
        }
        if ((deleteToggle.isOn == true) && (Input.GetMouseButtonUp(0)))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            Tile clickedTile = map.GetTile<Tile>(gridPosition);
            if (clickedTile.name.Contains("plot"))
            {
                myFx.PlayOneShot(deleteSound);
                plotFunction(gridPosition, true);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 amousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int agridPosition = map.WorldToCell(amousePosition);

            Tile aclickedTile = map.GetTile<Tile>(agridPosition);
            //print("clicked on tile: " + aclickedTile.name + " at position: " + agridPosition);

            if (aclickedTile.name.Contains("plot") && plotToggle.isOn == false && roadToggle.isOn == false && deleteToggle.isOn == false && shopToggle.GetComponent<Toggle>().isOn == false && pendingParent.transform.childCount == 0 && startInUI == false && mainCam.GetComponent<CameraMovement>().dragging == false)
            {
                shopToggle.GetComponent<UIToggle>().toggleToggles(shopToggle);
                shopToggle.GetComponent<shopButton>().shopToggle.isOn = true;
                shopToggle.GetComponent<shopButton>().ShopMenu.SetActive(true);
                shopToggle.GetComponent<shopButton>().rect.ReloadData();
            }
            startInUI = false;
        }
    }
}
