using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Draggable : MonoBehaviour
{

    Vector2 diff = Vector2.zero;
    GameObject ppDrag;
    Tilemap map;
    GameObject pendingParent;
    PropertyCard pCard;
    public float[] XY = new[] { 0f, 0f };
    public bool dragEnabled;
    public bool buildable;

    private void OnMouseDown()
    {
        print("touching " + this.name);
        ppDrag = GameObject.Find("ppDrag");
        map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        pendingParent = GameObject.Find("PendingProperty");
        if (this.GetComponent<Property>() != null)
        {
            pCard = this.GetComponent<Property>().Card;
        }
    }

    [ContextMenu("print name")]
    public void printName()
    {
        print("name is " + this.name);
    }


    public void OnMouseDrag()
    {
        //print("dragging here");
        //transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - diff;

        Vector3 amousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int agridPosition = map.WorldToCell(amousePosition);

        if (this.transform.parent == pendingParent.transform)
        {
            //print("parent is pending parent");
            Vector3 newTransform = new Vector3((float)agridPosition.x, (float)agridPosition.y, transform.position.z);
            if (transform.position != newTransform)
            {
                //print("cur trans != newTrans");
                transform.position = newTransform;
                transform.position += new Vector3(-1f, -0.32f, 0f); //offset vector
                ppDrag.transform.position = new Vector3(transform.position.x + (float.Parse(pCard.space.Substring(0, 1))) / 2, transform.position.y - 0.5f, 0f);
                string loc = pCard.displayName + "(" + (agridPosition.x - 1) + "," + (agridPosition.y - 1) + ")";
                this.name = loc;
                XY[0] = (float)agridPosition.x - 1; XY[1] = (float)agridPosition.y - 1;
                buildCheck(this.pCard, this.GetComponent<Draggable>().XY, map);
                if (buildable == true)
                {
                    this.GetComponent<BlinkingProperty>().StopBlink();
                    this.GetComponent<Renderer>().material.color = Color.green;
                } else if (buildable == false && this.GetComponent<BlinkingProperty>().stop == true)
                {
                    this.GetComponent<BlinkingProperty>().invokeStart();
                }
            }
        }

    }

    public bool buildCheck(PropertyCard card, float[] XY, Tilemap map)
    {
        //print("called build check");
        map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        bool result = true;
        if (card.type != "Deco")
        {
            int spaceX = int.Parse(card.space.Substring(0, 1));
            int spaceY = int.Parse(card.space.Substring(card.space.Length - 1));
            int x = (int)XY[0]; int y = (int)XY[1];
            for (int i = 0; i < spaceX * spaceY; i++)
            {
                TileBase Tile = map.GetTile(new Vector3Int(x, y, 0));
                //print("checking " + x + "," + y);
                if (!Tile.name.Contains("plot"))
                {
                    //print("setting false");
                    result = false;
                    break;
                }
                x += 1;
                if (x == ((int)XY[0] + spaceX))
                {
                    x = (int)XY[0]; y += 1;
                }
            }
        } else
        {
            int spaceX = int.Parse(card.space.Substring(0, 1));
            int spaceY = int.Parse(card.space.Substring(card.space.Length - 1));
            int x = (int)XY[0]; int y = (int)XY[1];
            for (int i = 0; i < spaceX * spaceY; i++)
            {
                TileBase Tile = map.GetTile(new Vector3Int(x, y, 0));
                print("tilename is " + Tile.name);
                //print("checking " + x + "," + y);
                if (Tile.name.Contains("road") || Tile.name.Contains("noBelow") || Tile.name.Contains("world"))
                {
                    print("setting false deco");
                    result = false;
                    break;
                }
                x += 1;
                if (x == ((int)XY[0] + spaceX))
                {
                    x = (int)XY[0]; y += 1;
                }
            }
        }
        //print("result is " + result);
        buildable = result;
        return result;
    }

    public void rebuildPlots(PropertyCard card, float[] XY)
    {
        print("rebuilding plots");
        int spaceX = int.Parse(card.space.Substring(0, 1));
        int spaceY = int.Parse(card.space.Substring(card.space.Length - 1));
        GameObject plotMgr = GameObject.Find("MapManager");
        map = GameObject.Find("Tilemap").GetComponent<Tilemap>();

        int x = (int)XY[0]; int y = (int)XY[1];
        for (int i = 0; i < spaceX * spaceY; i++)
        {
            TileBase Tile = map.GetTile(new Vector3Int(x, y, 0));
            //call plot function here
            if (card.type != "Deco")
            {
                plotMgr.GetComponent<plotManager>().plotFunction(new Vector3Int(x, y, 0), false, true);
            } else
            {
                map.SetTile(new Vector3Int(x, y, 0), Resources.Load<TileBase>("roadTiles/greenGrass"));
            }
            if (GameObject.Find("moveContractorToggle").GetComponent<Toggle>().isOn == true && card.type == "Deco")
            {
                map.SetTile(new Vector3Int(x, y, 0), Resources.Load<TileBase>("roadTiles/tileGrass"));
            }
            x += 1;
            if (x == ((int)XY[0] + spaceX))
            {
                x = (int)XY[0]; y += 1;
            }
        }
        print("rebuilt");
    }

}
