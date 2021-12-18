using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Draggable : MonoBehaviour
{

    Vector2 diff = Vector2.zero;
    GameObject ppDrag;
    Tilemap map;
    GameObject pendingParent;
    PropertyCard pCard;
    public float[] XY = new[] { 0f, 0f };
    public bool dragEnabled;

    private void OnMouseDown()
    {
        ppDrag = GameObject.Find("ppDrag");
        map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        pendingParent = GameObject.Find("PendingProperty");
        pCard = this.GetComponent<Property>().Card;
    }


    public void OnMouseDrag()
    {
        
        //transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - diff;

        Vector3 amousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int agridPosition = map.WorldToCell(amousePosition);

        if (this.transform.parent == pendingParent.transform)
        {
            transform.position = new Vector3((float)agridPosition.x, (float)agridPosition.y, transform.position.z);
            transform.position += new Vector3(-1f, -0.32f, 0f); //offset vector
            ppDrag.transform.position = new Vector3(transform.position.x + (float.Parse(pCard.space.Substring(0, 1))) / 2, transform.position.y - 1f, ppDrag.transform.position.z);
            string loc = pCard.displayName + "(" + (agridPosition.x-1) + "," + (agridPosition.y-1) + ")";
            this.name = loc;
            XY[0] = (float)agridPosition.x-1; XY[1] = (float)agridPosition.y-1;
        }
    }

}
