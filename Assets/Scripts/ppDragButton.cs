using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ppDragButton : MonoBehaviour
{
    [SerializeField]
    private GameObject pendingParent, propParent, ppDrag, externalAudioPlayer, saveloadsystem;

    [SerializeField]
    private Statistics stats;

    [SerializeField]
    private AudioClip buildSound, touchSound;

    public Tilemap map;
    public TileBase greenGrass, tileGrass;

    public float getZ(float[] coords) //range of y is 3 to -3, range of x is -44 to 45
    {
        float[] c = new[] { coords[0], coords[1] }; //create new float array that is not connected to Draggable.XY
        float result;
        //print("x is at " + coords[0] + ", y is at " + coords[1]);
        float xFactor = (c[0] -= 45)/1000;
        float yFactor = c[1] / 10;
        result = xFactor + yFactor;/*
        print("xFactor is " + xFactor + ", yFactor is " + yFactor);
        print("result is " + result);*/
        return result;
        //more negative is above
        //at y=-1, when x is -10 and -1, -0.1
    }

    private void removePlots(PropertyCard card, float[] XY)
    {
        print("removing plots");
        int spaceX = int.Parse(card.space.Substring(0, 1));
        int spaceY = int.Parse(card.space.Substring(card.space.Length - 1));

        int x = (int)XY[0]; int y = (int)XY[1];
        for (int i = 0; i < spaceX * spaceY; i++)
        {
            TileBase Tile = map.GetTile(new Vector3Int(x, y, 0));
            //call plot function here
            this.GetComponent<plotManager>().plotFunction(new Vector3Int(x, y,0), true, true);
            x += 1;
            if (x == ((int)XY[0] + spaceX))
            {
                x = (int)XY[0]; y += 1;
            }
        }
        print("removed");
    }

    public void dragConfirm()
    {
        Property pp;
        pp = pendingParent.transform.GetChild(0).gameObject.GetComponent<Property>();

        if (pp.GetComponent<Draggable>().buildable == true)
        {
            removePlots(pp.Card, pp.GetComponent<Draggable>().XY);
            // Setting position and parent to main properties -----------
            pp.transform.position = new Vector3(pp.transform.position.x, pp.transform.position.y, getZ(pp.GetComponent<Draggable>().XY));
            pp.transform.parent = propParent.transform;
            pp.GetComponent<SpriteRenderer>().sortingOrder = 1;

            // ------------- removing blinking and draggable -----------------------------
            pp.GetComponent<BlinkingProperty>().StopBlink();
            pp.GetComponent<Renderer>().material.color = Color.white;
            pp.GetComponent<Draggable>().dragEnabled = false;

            // Deducting money ---------------
            PropertyCard pCard = pp.GetComponent<Property>().Card;
            if (pCard.cost.Contains("Gold"))
            {
                stats.updateStats(diffgold: -(int.Parse(pCard.cost.Remove(pCard.cost.Length - 5))));
                stats.updateStats(diffxp: pCard.XP);
            }
            else
            {
                stats.updateStats(diffmoney: -(int.Parse(pCard.cost)));
                stats.updateStats(diffxp: pCard.XP);
            }
            // -------------------------------

            externalAudioPlayer.GetComponent<AudioSource>().PlayOneShot(buildSound);
            ppDrag.SetActive(false);

            // adding the contract collider and sorting its order -----------
            if (pCard.type == "House")
            {
                pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2; //shows contract
            }

            // --------------------- Swapping to green border grass -------------
            map.SwapTile(greenGrass, tileGrass);
            // ------------------------------------------------------------------

            // --------------------- Save Game ----------------------------------
            saveloadsystem.GetComponent<saveloadsystem>().saveGame();
            // ------------------------------------------------------------------
        }
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
