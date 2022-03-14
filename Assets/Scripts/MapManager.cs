using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{

	[SerializeField]
	private Tilemap map;

	[SerializeField]
	private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;


    [SerializeField]
    private Toggle roadToggle, deleteToggle;

    public AudioClip deleteSound;

    [SerializeField]
    public GameObject splashObject, saveObj, ppDragButton, Astar, PropertiesParent;

    public Camera mainCam;

    public bool startInUI;

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


    private void roadFunction(Vector3Int input, bool delete)
    {
        print("roadFunction called");
        int cycle = 1;
        int i = 1;
        Vector3Int gridPosition = input;

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

                if (((i == 1) && clickedTile.name.Contains("greenGrass")) || ((i == 1) && (delete == true)) || (clickedTile.name.Contains("road") && (i > 1)))
                {
                    string calcTile = TileChecker.calcTile(map, clickedTile, gridPosition.x, gridPosition.y);

                    if (((clickedTile.name != calcTile) && (delete == false)) || (delete == true))
                    {
                        
                        Tile createdTile = ScriptableObject.CreateInstance<Tile>();
                        string createPath = "roadTiles/" + calcTile;
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
                        }
                    }
                }
            } else
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
        if (initialCenterTile.name.Contains("Grass") && map.GetTile<Tile>(gridPosition).name.Contains("road"))
        {
            GameObject splash = Instantiate(splashObject, new Vector3(gridPosition.x + (float)0.5, gridPosition.y + (float)1.4, (float)gridPosition.z), Quaternion.identity) as GameObject;
        }
        //float walkingSpeed = dataFromTiles[clickedTile].walkingSpeed;

        //print("Walking speed on " + clickedTile + " at position " + gridPosition + " is " + walkingSpeed);
        saveObj.GetComponent<saveloadsystem>().saveTilemap();
    }

    public void testSurroundHouse(Vector3Int gridPosition)
    {
        Vector3Int test = new Vector3Int(0, 0, 0);
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0: test = new Vector3Int(gridPosition.x, gridPosition.y + 1, gridPosition.z); break;
                case 1: test = new Vector3Int(gridPosition.x + 1, gridPosition.y, gridPosition.z); break;
                case 2: test = new Vector3Int(gridPosition.x, gridPosition.y - 1, gridPosition.z); break;
                case 3: test = new Vector3Int(gridPosition.x - 1, gridPosition.y, gridPosition.z); break;
                default:
                    break;
            }
            Vector3 temp = map.CellToWorld(test);
            temp = new Vector3(temp.x + 0.5f, temp.y + 0.5f);
            Debug.DrawRay(temp, Vector3.forward, Color.red, Mathf.Infinity);
            if (map.GetTile(test).name.Contains("noBelow"))
            {
                RaycastHit2D[] hits;
                hits = Physics2D.RaycastAll(temp, Vector3.forward, Mathf.Infinity);
                for (int x = 0; x < hits.Length; x++)
                {
                    RaycastHit2D hit = hits[x];
                    if (hit.collider != null && hit.collider.gameObject.GetComponent<Property>() != null)
                    {
                        print("Hit Property: " + hit.collider.gameObject.name + " at x = " + x);
                        // A star detection when build ------
                        List<Vector2Int> alist = ppDragButton.GetComponent<ppDragButton>().getSurroundRoads(hit.collider.gameObject.GetComponent<Property>().Card, hit.collider.gameObject.GetComponent<Property>().GetComponent<Draggable>().XY);
                        bool astartest = false;
                        foreach (var item in alist)
                        {
                            if (map.GetTile(new Vector3Int(item.x, item.y, 0)).name.Contains("road"))
                            {
                                astartest = Astar.GetComponent<Astar>().AStarFunc(new Vector2Int(item.x, item.y), new Vector2Int(0, -1), map);
                                print("test astar is " + astartest);
                                if (astartest == true)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                astartest = false;
                            }
                        }
                        if (astartest == true)
                        {
                            switch (hit.collider.gameObject.GetComponent<Property>().Card.type)
                            {

                                case "House": hit.collider.gameObject.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0; break;
                                case "Commerce": hit.collider.gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0; break;
                                case "Wonder": hit.collider.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0; break;
                                default: break;
                            }
                        }
                        else
                        {
                            switch (hit.collider.gameObject.GetComponent<Property>().Card.type)
                            {
                                case "House":
                                    hit.collider.gameObject.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                                    hit.collider.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                    hit.collider.gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                    break;
                                case "Commerce":
                                    hit.collider.gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                                    hit.collider.gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                    break;
                                case "Wonder": hit.collider.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2; break;
                                default: break;
                            }
                        }
                        // End of astar detection -------
                    }
                }
            } else
            {
                //print("no below here");
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isMouseOverUI() == true)
        {
            startInUI = true;
        }

        if ((roadToggle.isOn == true) && (Input.GetMouseButtonUp(0)))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            Tile clickedTile = map.GetTile<Tile>(gridPosition);
            if (clickedTile.name.Contains("greenGrass"))
            {
                roadFunction(gridPosition, false);
                print("built a new road");

                //testSurroundHouse(gridPosition);
                foreach (Transform child in PropertiesParent.transform)
                {
                    if (child.gameObject.name != "HQ")
                    {
                        if (child.gameObject.GetComponent<Property>().Card.type != "Deco")
                        {
                            print("checked child " + child.gameObject.name);
                            Property pp = child.gameObject.GetComponent<Property>();
                            // A star detection when build ------
                            List<Vector2Int> alist = ppDragButton.GetComponent<ppDragButton>().getSurroundRoads(pp.Card, pp.GetComponent<Draggable>().XY);
                            bool test = false;
                            foreach (var item in alist)
                            {
                                //print("testing " + new Vector3Int(item.x, item.y, 0) + " with name " + map.GetTile(new Vector3Int(item.x, item.y, 0)).name);
                                if (map.GetTile(new Vector3Int(item.x, item.y, 0)).name.Contains("road"))
                                {
                                    //print("contain road");
                                    test = Astar.GetComponent<Astar>().AStarFunc(new Vector2Int(item.x, item.y), new Vector2Int(0, -1), map);
                                    print("test astar is " + test);
                                    if (test == true)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    //print("no road");
                                    test = false;
                                }
                            }
                            if (test == true)
                            {
                                pp.justConnected = true;
                                switch (pp.Card.type)
                                {
                                    case "House":
                                        if (pp.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sortingOrder == 2)
                                        {
                                            GameObject contractPopup = Instantiate(Resources.Load<GameObject>("propertyConnected"), new Vector3(pp.transform.position.x + (float.Parse(pp.GetComponent<Property>().Card.space.Substring(0, 1))) / 2, pp.transform.position.y + ((float.Parse(pp.GetComponent<Property>().Card.space.Substring(pp.GetComponent<Property>().Card.space.Length - 1))) / 2), -5f), Quaternion.identity) as GameObject;
                                            pp.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                        }
                                        break;
                                    case "Commerce":
                                        if (pp.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingOrder == 2)
                                        {
                                            GameObject contractPopup = Instantiate(Resources.Load<GameObject>("propertyConnected"), new Vector3(pp.transform.position.x + (float.Parse(pp.GetComponent<Property>().Card.space.Substring(0, 1))) / 2, pp.transform.position.y + ((float.Parse(pp.GetComponent<Property>().Card.space.Substring(pp.GetComponent<Property>().Card.space.Length - 1))) / 2), -5f), Quaternion.identity) as GameObject;
                                            pp.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                        }
                                        break;
                                    case "Wonder":
                                        if (pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder == 2)
                                        {
                                            GameObject contractPopup = Instantiate(Resources.Load<GameObject>("propertyConnected"), new Vector3(pp.transform.position.x + (float.Parse(pp.GetComponent<Property>().Card.space.Substring(0, 1))) / 2, pp.transform.position.y + ((float.Parse(pp.GetComponent<Property>().Card.space.Substring(pp.GetComponent<Property>().Card.space.Length - 1))) / 2), -5f), Quaternion.identity) as GameObject;
                                            pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                        }
                                        break;
                                    default: break;
                                }
                            }
                            else
                            {
                                switch (pp.Card.type)
                                {
                                    case "House":
                                        pp.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                                        pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                        pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                        break;
                                    case "Commerce":
                                        pp.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                                        pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                        break;
                                    case "Wonder": pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2; break;
                                    default: break;
                                }
                            }
                            // End of astar detection -------
                        }
                    }
                }
            }
        }
        if ((deleteToggle.isOn == true) && (Input.GetMouseButtonUp(0)) && startInUI == false)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            Tile clickedTile = map.GetTile<Tile>(gridPosition);
            if (clickedTile.name.Contains("road"))
            {
                roadFunction(gridPosition, true);

                print("deleted road");
                //testSurroundHouse(gridPosition);

                foreach (Transform child in PropertiesParent.transform)
                {
                    if (child.gameObject.name != "HQ")
                    {
                        if (child.gameObject.GetComponent<Property>().Card.type != "Deco")
                        {
                            print("checked child " + child.gameObject.name);
                            Property pp = child.gameObject.GetComponent<Property>();
                            // A star detection when build ------
                            List<Vector2Int> alist = ppDragButton.GetComponent<ppDragButton>().getSurroundRoads(pp.Card, pp.GetComponent<Draggable>().XY);
                            bool test = false;
                            foreach (var item in alist)
                            {
                                //print("testing " + new Vector3Int(item.x, item.y, 0) + " with name " + map.GetTile(new Vector3Int(item.x, item.y, 0)).name);
                                if (map.GetTile(new Vector3Int(item.x, item.y, 0)).name.Contains("road"))
                                {
                                    //print("contain road");
                                    test = Astar.GetComponent<Astar>().AStarFunc(new Vector2Int(item.x, item.y), new Vector2Int(0, -1), map);
                                    print("test astar is " + test);
                                    if (test == true)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    //print("no road");
                                    test = false;
                                }
                            }
                            if (test == true)
                            {
                                pp.justConnected = true;
                                switch (pp.Card.type)
                                {

                                    case "House": pp.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0; break;
                                    case "Commerce": pp.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0; break;
                                    case "Wonder": pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0; break;
                                    default: break;
                                }
                            }
                            else
                            {
                                switch (pp.Card.type)
                                {
                                    case "House":
                                        pp.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                                        pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                        pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                        break;
                                    case "Commerce":
                                        pp.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                                        pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                                        break;
                                    case "Wonder": pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2; break;
                                    default: break;
                                }
                            }
                            // End of astar detection -------
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 amousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int agridPosition = map.WorldToCell(amousePosition);

            Tile aclickedTile = map.GetTile<Tile>(agridPosition);
            print("clicked on tile: " + aclickedTile.name + " at position: " + agridPosition);
            startInUI = false;
        }
    }
}
