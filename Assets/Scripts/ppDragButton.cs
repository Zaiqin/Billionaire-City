using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ppDragButton : MonoBehaviour
{
    [SerializeField]
    private GameObject pendingParent, propParent, ppDrag, externalAudioPlayer, saveloadsystem, shopToggle, shopMenu;

    [SerializeField]
    private Statistics stats;

    [SerializeField]
    private AudioClip buildSound, touchSound;

    public Tilemap map;
    public TileBase greenGrass, tileGrass;
    public GameObject floatingValue, hq;

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

            if (card.type == "Deco" && Tile.name.Contains("plot"))
            {
                stats.GetComponent<Statistics>().updateStats(diffmoney: 1000);
                stats.GetComponent<Statistics>().noOfPlots -= 1;
                print("adding 1k in return");
                GameObject value = Instantiate(floatingValue, new Vector3(x + (float)0.5, (float)y + 2, 0f), Quaternion.identity) as GameObject;
                value.transform.GetChild(0).GetComponent<TextMesh>().text = "+$1000";
                value.transform.GetChild(0).GetComponent<TextMesh>().color = new Color(168f / 255f, 255f / 255f, 4f / 255f);
            }
            //call plot function here
            this.GetComponent<plotManager>().belowFunction(new Vector3Int(x, y,0), true, true);
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

        bool canBuild = true;

        if (pp.Card.type == "Deco")
        {
            int deduction = 0;
            if (pp.Card.cost.Contains("Gold"))
            {
                deduction = int.Parse(pp.Card.cost.Remove(pp.Card.cost.Length - 5));
                print("deducting " + deduction + " gold");
                if (stats.returnStats()[1] < deduction)
                {
                    print("insufficient gold");
                    canBuild = false;
                }
            }
            else
            {
                deduction = int.Parse(pp.Card.cost);
                print("deducting $" + deduction);
                if (stats.returnStats()[0] < deduction)
                {
                    print("insufficient money");
                    canBuild = false;
                }
            }
        }

        if (pp.GetComponent<Draggable>().buildable == true && canBuild == true)
        {
            shopToggle.GetComponent<Toggle>().isOn = false;
            removePlots(pp.Card, pp.GetComponent<Draggable>().XY);
            // Setting position and parent to main properties -----------
            pp.transform.position = new Vector3(pp.transform.position.x, pp.transform.position.y, getZ(pp.GetComponent<Draggable>().XY));
            pp.transform.parent = propParent.transform;
            pp.GetComponent<SpriteRenderer>().sortingOrder = 1;

            // ------------- removing blinking and draggable -----------------------------
            Destroy(pp.GetComponent<BlinkingProperty>());
            pp.GetComponent<Renderer>().material.color = Color.white;
            pp.GetComponent<Draggable>().dragEnabled = false;
            pp.GetComponent<Draggable>().buildable = false;

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

            // adding the contract collider and sorting its order -----------
            if (pCard.type == "House")
            {
                pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
                pp.transform.GetChild(2).gameObject.AddComponent<BoxCollider2D>();
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2; //shows contract
            }
            if (pCard.type == "Commerce")
            {
                pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
                pp.transform.GetChild(1).gameObject.AddComponent<BoxCollider2D>();
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0; //hide commerce collect
                DateTime theTime;
                theTime = DateTime.Now.AddMinutes(3);
                print("signing property commerce after building");
                string datetime = theTime.ToString("yyyy/MM/dd HH:mm:ss");
                pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime = datetime;
                pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signCreationTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                print("sign time is " + datetime);
                pp.transform.GetChild(0).gameObject.SetActive(false);
                pp.transform.GetChild(0).GetComponent<influence>().removeHighlights();
            }
            if (pp.Card.type == "Deco")
            {
                pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
                shopToggle.GetComponent<Toggle>().isOn = true;
                GameObject a = Instantiate(pp.gameObject);
                a.transform.parent = pendingParent.transform;
                pendingParent.transform.GetChild(0).gameObject.transform.position = new Vector3(a.transform.position.x, a.transform.position.y, -8);
                pendingParent.transform.GetChild(0).GetComponent<Draggable>().dragEnabled = true;
                a.GetComponent<Property>().Card = pp.Card;
                a.GetComponent<BlinkingProperty>().invokeStart();
                Destroy(a.transform.GetChild(0).GetComponent<BoxCollider2D>());
                pp.transform.GetChild(0).gameObject.SetActive(false);

            }
            if (pp.Card.type == "Wonder")
            {
                
                stats.GetComponent<Statistics>().builtWonders.Add(pp.Card.displayName);
                if (pp.Card.wonderBonus >= 100) 
                { 
                    stats.GetComponent<Statistics>().doubleChance += (int)(((float)pp.Card.wonderBonus) / 100);
                } else if (pp.Card.wonderBonus > 10 && pp.Card.wonderBonus < 100)
                {
                    print("added wonder commerce");
                    stats.GetComponent<Statistics>().wonderCommerceBonus += (int)(((float)pp.Card.wonderBonus)/10);
                    stats.GetComponent<Statistics>().commerceWonders.Add(pp.Card.displayName);
                }
                else
                {
                    stats.GetComponent<Statistics>().wonderBonus += pp.Card.wonderBonus;
                }
            }
            // --------------------- Swapping to green border grass -------------
            if (pp.Card.type != "Deco")
            {
                map.SwapTile(greenGrass, tileGrass);
                ppDrag.SetActive(false);
                string temp;
                if (pp.Card.buildTime.Contains("mins"))
                {
                    temp = pp.Card.buildTime.Remove(pp.Card.buildTime.Length-4);
                    pp.constructEnd = DateTime.Now.AddMinutes(int.Parse(temp)).ToString("yyyy/MM/dd HH:mm:ss");
                    pp.constructStart = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    print("temp is " + temp);
                }
                else if (pp.Card.buildTime.Contains("hr"))
                {
                    temp = pp.Card.buildTime.Remove(pp.Card.buildTime.Length - 2);
                    pp.constructEnd = DateTime.Now.AddHours(int.Parse(temp)).ToString("yyyy/MM/dd HH:mm:ss");
                    pp.constructStart = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    print("temp is " + temp);
                }
                else if (pp.Card.buildTime.Contains("day"))
                {
                    temp = pp.Card.buildTime.Remove(pp.Card.buildTime.Length - 3);
                    pp.constructEnd = DateTime.Now.AddDays(int.Parse(temp)).ToString("yyyy/MM/dd HH:mm:ss");
                    pp.constructStart = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    print("temp is " + temp);
                }
            }
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
        shopMenu.SetActive(true);
        // --------------------- Swapping to green border grass -------------
        map.SwapTile(greenGrass, tileGrass);
        // ------------------------------------------------------------------
    }
}
