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
    private Toggle plotToggle, deleteToggle;

    public AudioSource myFx;
    public AudioClip deleteSound;

    private bool isMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
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


    private void plotFunction(Vector3Int input, bool delete)
    {
        print("plotFunction called");
        int cycle = 1;
        int i = 1;
        Vector3Int gridPosition = input;
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

                if (((i == 1) && clickedTile.name.Contains("greenGrass")) || ((i == 1) && (delete == true)) || (clickedTile.name.Contains("plot") && (i > 1)))
                {
                    string calcTile = plotTileChecker.calcTile(map, clickedTile, gridPosition.x, gridPosition.y);

                    if (((clickedTile.name != calcTile) && (delete == false)) || (delete == true))
                    {

                        Tile createdTile = ScriptableObject.CreateInstance<Tile>();
                        string createPath = "plotTiles/" + calcTile;
                        createdTile.sprite = Resources.Load<Tile>(createPath).sprite;
                        createdTile.name = calcTile;


                        if ((i == 1) && (delete == true))
                        {
                            createdTile = map.GetTile<Tile>(new Vector3Int(-1, 0, 0)); //grass below hq
                        }
                        if (isMouseOverUI() == false)
                        {
                            map.SetTile(gridPosition, createdTile);
                        }
                    }
                }
            }
            else
            {
                print("clicked tile is nil");
            }

            i++;
            print("i is now " + i);
            if ((i == 14) && (cycle == 1))
            {
                i = 1;
                cycle = 2;
            }
        }
        //float walkingSpeed = dataFromTiles[clickedTile].walkingSpeed;

        //print("Walking speed on " + clickedTile + " at position " + gridPosition + " is " + walkingSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if ((plotToggle.isOn == true) && (Input.GetMouseButtonUp(0)))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            Tile clickedTile = map.GetTile<Tile>(gridPosition);
            if (clickedTile.name.Contains("greenGrass"))
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
            print("clicked on tile: " + aclickedTile.name + " at position: " + agridPosition);
        }
    }
}
