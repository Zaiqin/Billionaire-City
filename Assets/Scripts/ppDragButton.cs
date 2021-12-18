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
        Property pp = new Property();
        pp.obj = pendingParent.transform.GetChild(0).gameObject;
        //print("obj name is in drag confirm is " + pp.obj.name);
        pp.obj.transform.position = new Vector3(pp.obj.transform.position.x, pp.obj.transform.position.y, getZ(pp.obj.GetComponent<Draggable>().XY));
        pp.obj.transform.parent = propParent.transform;

        pp.obj.GetComponent<BlinkingProperty>().StopBlink();
        pp.obj.GetComponent<Renderer>().material.color = Color.white;
        pp.obj.GetComponent<Draggable>().dragEnabled = false;
        PropertyCard pCard = pp.obj.GetComponent<PropertyStats>().pCard;
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

        if (pp.obj.GetComponent<PropertyStats>().propType == "House")
        {
            pp.obj.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
            pp.obj.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2; //shows contract
        }

        // --------------------- Swapping to green border grass -------------
        map.SwapTile(greenGrass, tileGrass);
        // ------------------------------------------------------------------
    }

    public void dragCancel()
    {
        Property pp = new Property();
        pp.obj = pendingParent.transform.GetChild(0).gameObject;
        Destroy(pp.obj);
        externalAudioPlayer.GetComponent<AudioSource>().PlayOneShot(touchSound);
        ppDrag.SetActive(false);
    }
}
