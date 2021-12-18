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
    PropertyStats pStats;
    public float[] XY = new[] { 0f, 0f };
    public bool dragEnabled;

    private void OnMouseDown()
    {
        ppDrag = GameObject.Find("ppDrag");
        map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        pendingParent = GameObject.Find("PendingProperty");
        pStats = this.GetComponent<PropertyStats>();
    }


    public void OnMouseDrag()
    {
        
        //transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - diff;

        Vector3 amousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int agridPosition = map.WorldToCell(amousePosition);

        if (this.transform.parent == pendingParent.transform)
        {
            transform.position = new Vector3((float)agridPosition.x, (float)agridPosition.y, transform.position.z);
            transform.position += new Vector3(-1f, -0.35f, 0f); //offset vector
            ppDrag.transform.position = new Vector3(transform.position.x + (float.Parse(pStats.pCard.space.Substring(0, 1))) / 2, transform.position.y - 1f, ppDrag.transform.position.z);
            string loc = pStats.pCard.displayName + "(" + agridPosition.x + "," + agridPosition.y + ")";
            this.name = loc;
            XY[0] = (float)agridPosition.x; XY[1] = (float)agridPosition.y;
        }
    }

}
