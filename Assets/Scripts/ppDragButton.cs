using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ppDragButton : MonoBehaviour
{

    [SerializeField]
    private GameObject pendingParent, propParent, ppDrag, externalAudioPlayer;

    [SerializeField]
    private Statistics stats;

    [SerializeField]
    private AudioClip buildSound, touchSound;

    public Tilemap map;
    public TileBase greenGrass, tileGrass;

    private float getZ(float[] coords) //range of y is 3 to -3, range of x is -44 to 45
    {
        float result;
        //print("x is at " + coords[0] + ", y is at " + coords[1]);
        float xFactor = (coords[0] -= 45)/1000;
        float yFactor = coords[1] / 10;
        result = xFactor + yFactor;/*
        print("xFactor is " + xFactor + ", yFactor is " + yFactor);
        print("result is " + result);*/
        return result;
        //more negative is above
        //at y=-1, when x is -10 and -1, -0.1
    }

    public void dragConfirm()
    {
        Property pp;
        pp = pendingParent.transform.GetChild(0).gameObject.GetComponent<Property>();
        //print("obj name is in drag confirm is " + pp.obj.name);
        pp.transform.position = new Vector3(pp.transform.position.x, pp.transform.position.y, getZ(pp.GetComponent<Draggable>().XY));
        pp.transform.parent = propParent.transform;

        pp.GetComponent<BlinkingProperty>().StopBlink();
        pp.GetComponent<Renderer>().material.color = Color.white;
        pp.GetComponent<Draggable>().dragEnabled = false;
        PropertyCard pCard = pp.GetComponent<Property>().Card;
        //print("built a " + pp.obj.GetComponent<PropertyStats>().propType);

        if (pCard.cost.Contains("Gold"))
        {
            stats.updateStats(diffgold: -(int.Parse(pCard.cost.Remove(pCard.cost.Length - 5))));
            stats.updateStats(diffxp: pCard.XP);
        } else
        {
            stats.updateStats(diffmoney: -(int.Parse(pCard.cost)));
            stats.updateStats(diffxp: pCard.XP);
        }
        externalAudioPlayer.GetComponent<AudioSource>().PlayOneShot(buildSound);
        ppDrag.SetActive(false);

        if (pCard.type == "House")
        {
            pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
            pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2; //shows contract
        }

        // --------------------- Swapping to green border grass -------------
        map.SwapTile(greenGrass, tileGrass);
        // ------------------------------------------------------------------
    }

    public void dragCancel()
    {
        Destroy(pendingParent.transform.GetChild(0).gameObject);
        externalAudioPlayer.GetComponent<AudioSource>().PlayOneShot(touchSound);
        ppDrag.SetActive(false);
        // --------------------- Swapping to green border grass -------------
        map.SwapTile(greenGrass, tileGrass);
        // ------------------------------------------------------------------
    }
}
