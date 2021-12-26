using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChecker : MonoBehaviour
{

    public static string calcTile(Tilemap map, TileBase centerTile, int column, int row)
    {

        //print("-------started function tileCheck--------");
        int number = 0;
        TileBase tTile = map.GetTile(new Vector3Int(column, row + 1, 0));
        TileBase rTile = map.GetTile(new Vector3Int(column + 1, row, 0));
        TileBase bTile = map.GetTile(new Vector3Int(column, row - 1, 0));
        TileBase lTile = map.GetTile(new Vector3Int(column - 1, row, 0));
        TileBase tlTile = map.GetTile(new Vector3Int(column - 1, row + 1, 0));
        TileBase trTile = map.GetTile(new Vector3Int(column + 1, row + 1, 0));
        TileBase brTile = map.GetTile(new Vector3Int(column + 1, row - 1, 0));
        TileBase blTile = map.GetTile(new Vector3Int(column - 1, row - 1, 0));

        if (tlTile != null) {
            if (tlTile.name.Contains("road") && tTile.name.Contains("road") && lTile.name.Contains("road"))
            {
                //print("had road at the top left that matters");
                number += 1;
            }
        }

        if (tTile != null)
        {
            if (tTile.name.Contains("road"))
            {
                //print("had road at the top");
                number += 2;
            }
        }

        if (trTile != null)
        {
            if (trTile.name.Contains("road") && tTile.name.Contains("road") && rTile.name.Contains("road"))
            {
                //print("had road at the topright that matters");
                number += 4;
            }
        }
        else
        {
            //no tr tile
        }

        if (lTile != null)
        {
            if (lTile.name.Contains("road")) {
                //print("had road at the left");
                number += 8;
            }
        }

        if (rTile != null)
        {
            if (rTile.name.Contains("road")) {
                //print("had road at the right");
                number += 16;
            }
        }
        else
        {
            //no r tile
        }

        if (blTile != null)
        {
            if (blTile.name.Contains("road") && bTile.name.Contains("road") && lTile.name.Contains("road")) {
                //print("had road at the bottomleft that matters");
                number += 32;
            }
        }

        if (bTile != null)
            {
            if (bTile.name.Contains("road")) {
                //print("had road at the bottom");
                number += 64;
            }
        }

        if (brTile != null)
        {
            if (brTile.name.Contains("road") && bTile.name.Contains("road") && rTile.name.Contains("road")) {
                //print("had road at the bottomright that matters");
                number += 128;
            }
        }
        else
        {
            //no br tile
        }

        // -------- Adding striped roads ----------
        if (number == 24)
        {
            //print("horizontal road tile");
            string[] Rintersections = {"26","30","74","88","90","94","216","218","222"};
            foreach (string name in Rintersections)
            {
                if (rTile.name.Contains("road"+name))  
                {
                    number = 888;
                }
                if (rTile.name.Contains("road888"))
                {
                    number = 24;
                }

            }
            string[] Lintersections = {"26","27","82","88","90","91","120","122","123"};
            if (number == 24) // check is it is still normal hori. Prevents checking when its already striped
            {
                foreach (string name in Lintersections)
                {
                    if (lTile.name.Contains("road" + name))
                    {
                        number = 888;
                    }
                    if (lTile.name.Contains("road888"))
                    {
                        number = 24;
                    }
                }
            }
        }
        if (number == 66)
        {
            //print("horizontal road tile");
            string[] Tintersections = {"74","75","82","86","88","90","91","94","95"};
            foreach (string name in Tintersections)
            {
                if (tTile.name.Contains("road"+name))  
                {
                    number = 999;
                }
                if (tTile.name.Contains("road999"))
                {
                    number = 66;
                }
            }
            string[] Bintersections = {"26","74","82","90","106","122","210","218","250"};
            if (number == 66) // check is it is still normal vert. Prevents checking when its already striped
            {
                foreach (string name in Bintersections)
                {
                    if (bTile.name.Contains("road" + name))
                    {
                        number = 999;
                    }
                    if (bTile.name.Contains("road999"))
                    {
                        number = 66;
                    }
                }
            }
        }

        // -------------------------------------------
        string id = "road" + number.ToString();
        //print("final id is " + id);
        //print("-------stopped function tileCheck--------");
        return id;
}

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


    



