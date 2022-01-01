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

    public AudioSource myFx;
    public AudioClip deleteSound;

    [SerializeField]
    public GameObject splashObject, saveObj;

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

                        if ((i == 1) && (delete == true))
                        {
                            createdTile = map.GetTile<Tile>(new Vector3Int(-1, 0, 0)); //grass below hq
                        }
                        if (isMouseOverUI() == false && startInUI == false)
                        {
                            if (mainCam.GetComponent<CameraMovement>().dragging == false)
                            {
                                map.SetTile(gridPosition, createdTile);
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
            }
        }
        if ((deleteToggle.isOn == true) && (Input.GetMouseButtonUp(0)) && startInUI == false)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            Tile clickedTile = map.GetTile<Tile>(gridPosition);
            if (clickedTile.name.Contains("road"))
            {
                myFx.PlayOneShot(deleteSound);
                roadFunction(gridPosition, true);
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
