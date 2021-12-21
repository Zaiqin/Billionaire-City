using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class plotTileChecker : MonoBehaviour
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

        if (tlTile != null)
        {
            if (tlTile.name.Contains("plot") && tTile.name.Contains("plot") && lTile.name.Contains("plot"))
            {
                //print("had plot at the top left that matters");
                number += 1;
            }
        }

        if (tTile != null)
        {
            if (tTile.name.Contains("plot"))
            {
                //print("had plot at the top");
                number += 2;
            }
        }

        if (trTile != null)
        {
            if (trTile.name.Contains("plot") && tTile.name.Contains("plot") && rTile.name.Contains("plot"))
            {
                //print("had plot at the topright that matters");
                number += 4;
            }
        }
        else
        {
            //no tr tile
        }

        if (lTile != null)
        {
            if (lTile.name.Contains("plot"))
            {
                //print("had plot at the left");
                number += 8;
            }
        }

        if (rTile != null)
        {
            if (rTile.name.Contains("plot"))
            {
                //print("had plot at the right");
                number += 16;
            }
        }
        else
        {
            //no r tile
        }

        if (blTile != null)
        {
            if (blTile.name.Contains("plot") && bTile.name.Contains("plot") && lTile.name.Contains("plot"))
            {
                //print("had plot at the bottomleft that matters");
                number += 32;
            }
        }

        if (bTile != null)
        {
            if (bTile.name.Contains("plot"))
            {
                //print("had plot at the bottom");
                number += 64;
            }
        }

        if (brTile != null)
        {
            if (brTile.name.Contains("plot") && bTile.name.Contains("plot") && rTile.name.Contains("plot"))
            {
                //print("had plot at the bottomright that matters");
                number += 128;
            }
        }
        else
        {
            //no br tile
        }

        // -------------------------------------------
        string id = "plot" + number.ToString();
        print("final id is " + id);
        //print("-------stopped function tileCheck--------");
        return id;
    }

}






