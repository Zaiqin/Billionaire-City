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
    private GameObject pendingParent, propParent, ppDrag, externalAudioPlayer, saveloadsystem, shopToggle, shopMenu, missionsPanel, storagePanel, storageToggle, storageController, moveToggle, delConfirm, globalInsuff;

    [SerializeField]
    private Statistics stats;

    [SerializeField]
    private AudioClip buildSound, touchSound;

    public Tilemap map;
    public TileBase greenGrass, tileGrass;
    public GameObject floatingValue, hq, astar;
    public Sprite insuffMoney, insuffGold;

    public List<Mission> typeOne = new List<Mission>();

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

    public List<Vector2Int> getSurroundRoads(PropertyCard card, float[] XY)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        int[] xy = new int[] { (int)(XY[0]-1), (int)(XY[1]) };
        int propX = int.Parse(card.space.Substring(0, 1)); int propY = int.Parse(card.space.Substring(card.space.Length - 1));
        int totalNo = (propX * 2) + (propY * 2);
        int deltaX = 0; int deltaY = 0; bool topHori = true;
        for (int i = 0; i < totalNo; i++)
        {
            if (i < propY)
            {
                list.Add(new Vector2Int(xy[0], xy[1]+deltaY));
                deltaY++;
            } else if ((deltaX < propX) && (topHori == true))
            {
                if (i != propY-1)
                {
                    list.Add(new Vector2Int((int)(XY[0]) + deltaX, xy[1] + deltaY));
                    deltaX++;
                } else
                {
                    deltaY++;
                    list.Add(new Vector2Int((int)(XY[0]) + deltaX, xy[1] + deltaY));
                    deltaX++;
                }
                
            } else if (deltaY != 0)
            {
                if (deltaX == propX)
                {
                    topHori = false;
                    deltaY--;
                    list.Add(new Vector2Int((int)(XY[0]) + propX, xy[1] + deltaY));
                }
                else
                {
                    list.Add(new Vector2Int((int)(XY[0]) + propX, xy[1] + deltaY));
                    deltaY--;
                }
            } else
            {
                if (deltaX == propX)
                {
                    list.Add(new Vector2Int(xy[0] + deltaX, xy[1] + deltaY-1));
                    deltaX--;
                }
                else
                {
                    list.Add(new Vector2Int(xy[0] + deltaX, xy[1] + deltaY-1));
                    deltaX--;
                }
            }
        }
        foreach (var item in list)
        {
            //print("item " + item);
        }
        print("totalNo is " + list.Count);
        return list;
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
        int deduction = 0;
        if (moveToggle.GetComponent<Toggle>().isOn == false)
        {
            if (pp.Card.cost.Contains("Gold"))
            {
                deduction = int.Parse(pp.Card.cost.Remove(pp.Card.cost.Length - 5));
                print("deducting " + deduction + " gold");
                if (stats.returnStats()[1] < deduction)
                {
                    print("insufficient gold");
                    canBuild = false;
                    globalInsuff.SetActive(true);
                    globalInsuff.transform.GetChild(0).GetComponent<Image>().sprite = insuffGold;
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
                    globalInsuff.SetActive(true);
                    globalInsuff.transform.GetChild(0).GetComponent<Image>().sprite = insuffMoney;
                }
            }
        }

        if (pp.GetComponent<Draggable>().buildable == true && canBuild == true)
        {
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
            
            PropertyCard pCard = pp.GetComponent<Property>().Card;

            // Deducting money ---------------
            if (shopToggle.GetComponent<Toggle>().isOn == true)
            {
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
            }
            // -------------------------------

            shopToggle.GetComponent<Toggle>().isOn = false;

            externalAudioPlayer.GetComponent<AudioSource>().PlayOneShot(buildSound);

            // adding the contract collider and sorting its order -----------
            if (pCard.type == "House")
            {
                pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
                pp.transform.GetChild(2).gameObject.AddComponent<BoxCollider2D>();
                pp.transform.GetChild(2).gameObject.GetComponent<BoxCollider2D>().size = new Vector2(((GameObject.Find("CSV").gameObject.GetComponent<CSVReader>().maxDecoReach * 2) + int.Parse(pp.Card.space.Substring(0, 1))) - 0.1f, ((GameObject.Find("CSV").gameObject.GetComponent<CSVReader>().maxDecoReach * 2) + int.Parse(pp.Card.space.Substring(pp.Card.space.Length - 1))) - 0.1f);
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0; //hides contract
            }
            if (pCard.type == "Commerce")
            {
                pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
                pp.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().size = new Vector2(float.Parse(pp.Card.influence.Substring(0, 2)) - 0.2f, float.Parse(pp.Card.influence.Substring(pp.Card.influence.Length - 2)) - 0.2f);
                pp.transform.GetChild(1).gameObject.AddComponent<BoxCollider2D>().size = new Vector2((float)((double)pp.transform.GetChild(1).gameObject.AddComponent<BoxCollider2D>().size.x * 1.3), (float)((double)pp.transform.GetChild(1).gameObject.AddComponent<BoxCollider2D>().size.y * 1.3));
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0; //hide commerce collect

                pp.transform.GetChild(0).gameObject.SetActive(false);

                if (storageToggle.GetComponent<Toggle>().isOn == true)
                {
                    DateTime theTime;
                    theTime = DateTime.Now.AddMinutes(3);
                    print("signing property commerce after using from storage");
                    string datetime = theTime.ToString("yyyy/MM/dd HH:mm:ss");
                    pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime = datetime;
                    pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signCreationTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    print("sign time is " + datetime);
                }
            }
            if (pp.Card.type == "Deco")
            {
                if (storageToggle.GetComponent<Toggle>().isOn == false && moveToggle.GetComponent<Toggle>().isOn == false)
                {
                    pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
                    pp.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().size = new Vector2(float.Parse(pp.Card.influence.Substring(0, 2)) - 0.2f, float.Parse(pp.Card.influence.Substring(pp.Card.influence.Length - 2)) - 0.2f);
                    shopToggle.GetComponent<Toggle>().isOn = true;
                    GameObject a = Instantiate(pp.gameObject);
                    a.transform.parent = pendingParent.transform;
                    pendingParent.transform.GetChild(0).gameObject.transform.position = new Vector3(a.transform.position.x, a.transform.position.y, -8);
                    pendingParent.transform.GetChild(0).GetComponent<Draggable>().dragEnabled = true;
                    a.GetComponent<Property>().Card = pp.Card;
                    a.GetComponent<BlinkingProperty>().invokeStart();
                    Destroy(a.transform.GetChild(0).GetComponent<BoxCollider2D>());
                    pp.transform.GetChild(0).gameObject.SetActive(false);
                    pp.constructEnd = "na"; pp.constructStart = "na";
                } else if (moveToggle.GetComponent<Toggle>().isOn == true)
                {
                    shopMenu.GetComponent<PurchaseController>().showUI();
                    pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
                    pp.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().size = new Vector2(float.Parse(pp.Card.influence.Substring(0, 2)) - 0.2f, float.Parse(pp.Card.influence.Substring(pp.Card.influence.Length - 2)) - 0.2f);
                    pp.transform.GetChild(0).gameObject.SetActive(false);
                    pp.constructEnd = "na"; pp.constructStart = "na";
                }

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
                shopToggle.GetComponent<shopButton>().requireReload = true;
            }
            
            if (pp.Card.type != "Deco")
            {
                shopMenu.GetComponent<PurchaseController>().showUI();
                map.SwapTile(greenGrass, tileGrass);
                ppDrag.SetActive(false);
                string temp;
                if (pp.Card.buildTime.Contains("min"))
                {
                    temp = pp.Card.buildTime.Remove(pp.Card.buildTime.Length-4);
                    pp.constructEnd = DateTime.Now.AddMinutes(int.Parse(temp)).ToString("yyyy/MM/dd HH:mm:ss");
                    pp.constructStart = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    //print("temp is " + temp);
                }
                else if (pp.Card.buildTime.Contains("hr"))
                {
                    temp = pp.Card.buildTime.Remove(pp.Card.buildTime.Length - 2);
                    pp.constructEnd = DateTime.Now.AddHours(int.Parse(temp)).ToString("yyyy/MM/dd HH:mm:ss");
                    pp.constructStart = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    //print("temp is " + temp);
                }
                else if (pp.Card.buildTime.Contains("day"))
                {
                    temp = pp.Card.buildTime.Remove(pp.Card.buildTime.Length - 3);
                    pp.constructEnd = DateTime.Now.AddDays(int.Parse(temp)).ToString("yyyy/MM/dd HH:mm:ss");
                    pp.constructStart = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    //print("temp is " + temp);
                }

                // A star detection when build ------
                List<Vector2Int> alist = getSurroundRoads(pCard, pp.GetComponent<Draggable>().XY);
                /*foreach (var item in alist) // for testing surround roads
                {
                    map.SetTile(new Vector3Int(item.x, item.y, 0), road255);
                }*/
                bool test = false;
                foreach (var item in alist)
                {
                    if (map.GetTile(new Vector3Int(item.x, item.y, 0)).name.Contains("road"))
                    {
                        test = astar.GetComponent<Astar>().AStarFunc(new Vector2Int(item.x, item.y), new Vector2Int(0, -1), map);
                        print("test astar is " + test);
                    } else
                    {
                        print("no road, no astar called");
                        test = false;
                    }
                    if (test == true)
                    {
                        switch (pp.Card.type)
                        {
                            case "House": pp.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0; break;
                            case "Commerce": pp.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0; break;
                            case "Wonder": pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0; break;
                            default: break;
                        }
                        break;
                    }
                    else
                    {
                        switch (pp.Card.type)
                        {
                            case "House": pp.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2; break;
                            case "Commerce": pp.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2; break;
                            case "Wonder": pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2; break;
                            default: break;
                        }
                    }
                }
                // End of astar detection -------
            }
            // ------------------------------------------------------------------

            // ---------- Move House delete old version -------------------------
            if (moveToggle.GetComponent<Toggle>().isOn == true)
            {
                delConfirm.GetComponent<delConfirm>().selProp = Camera.main.GetComponent<SpriteDetector>().moveSelected;
                delConfirm.GetComponent<delConfirm>().deleteProp();
                pp.constructEnd = "na";
                pp.constructStart = "na";
                if (pp.Card.type == "House") // check to only do these thats only for houses
                {
                    pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime = Camera.main.GetComponent<SpriteDetector>().moveSelected.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime;
                    pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex = Camera.main.GetComponent<SpriteDetector>().moveSelected.transform.GetChild(0).gameObject.GetComponent<contractScript>().signIndex;
                    pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signCreationTime = Camera.main.GetComponent<SpriteDetector>().moveSelected.transform.GetChild(0).gameObject.GetComponent<contractScript>().signCreationTime;

                    var dateAndTimeVar = System.DateTime.Now;
                    //print("going check contract for " + pp.name + "which is signtime " + pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime);
                    if (pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime == "notsigned")
                    {
                        pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                        pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                        print("notsigned");
                    }
                    else if (dateAndTimeVar >= DateTime.Parse(pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime))
                    {
                        pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                        pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                        //print("sign over timea alre");
                    }
                    else if (dateAndTimeVar < DateTime.Parse(pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime))
                    {
                        pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                        pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    }
                    else
                    {
                        pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                        pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                        //print("sign still ongoiing");
                    }
                }
                else if (pp.Card.type == "Commerce")
                {
                    pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime = Camera.main.GetComponent<SpriteDetector>().moveSelected.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime;
                    pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signCreationTime = Camera.main.GetComponent<SpriteDetector>().moveSelected.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signCreationTime;
                    var dateAndTimeVar = System.DateTime.Now;
                    //print("going check contract for " + pp.name + "which is signtime " + pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime);
                    if (dateAndTimeVar >= DateTime.Parse(pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime))
                    {
                        pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                        //print("sign over time already");
                    }
                    else if (dateAndTimeVar < DateTime.Parse(pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime))
                    {
                        pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    }
                    else
                    {
                        pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                        //print("sign still ongoiing");
                    }
                }
                else if (pp.Card.type == "Deco")
                {
                    pp.transform.GetChild(0).gameObject.SetActive(false);
                    ppDrag.SetActive(false);
                }
                Camera.main.GetComponent<SpriteDetector>().moveSelected = null;
            }

            if (storageToggle.GetComponent<Toggle>().isOn == true)
            {
                shopMenu.GetComponent<PurchaseController>().showUI();
                storageToggle.GetComponent<Toggle>().isOn = false;
                storageController.GetComponent<RecyclableScrollerStorage>().deleteFromStorage(pCard.displayName);
                pp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
                pp.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().size = new Vector2(float.Parse(pp.Card.influence.Substring(0, 2)) - 0.2f, float.Parse(pp.Card.influence.Substring(pp.Card.influence.Length - 2)) - 0.2f);
                pp.transform.GetChild(0).gameObject.SetActive(false);
                pp.constructEnd = "na"; pp.constructStart = "na";
                if (pp.Card.type == "Deco")
                {
                    map.SwapTile(greenGrass, tileGrass);
                    ppDrag.SetActive(false);
                }
            }

            

            // --------------------- Save Game ----------------------------------
            saveloadsystem.GetComponent<saveloadsystem>().saveGame();
            // ------------------------------------------------------------------

            // ------------ Type 1 Missions -------------------------------------
            if (missionsPanel.GetComponent<missionParent>().missionList != null && moveToggle.GetComponent<Toggle>().isOn == false)
            {
                if (missionsPanel.GetComponent<missionParent>().missionList != null)
                {
                    foreach (var mission in typeOne)
                    {
                        print("cheching " + mission.msnName + " with type " + mission.msnType);
                        string displayName = mission.msnMetadata.Substring(3, mission.msnMetadata.Length - 3);
                        int count = int.Parse(mission.msnMetadata.Substring(0, 3));
                        print("displayname is " + displayName + ", countreq is " + count);

                        if (mission.msnPending == false)
                        {
                            if (count == 0)
                            {
                                if (mission.msnName == "Build I" && pCard.type == "House")
                                {
                                    missionsPanel.GetComponent<missionParent>().completeMission(mission);
                                }
                                else if (pCard.displayName == displayName)
                                {
                                    missionsPanel.GetComponent<missionParent>().completeMission(mission);
                                }
                            }
                            else
                            {
                                int propCount = 0;
                                foreach (Transform child in propParent.transform)
                                {
                                    if (child.name != "HQ")
                                    {
                                        if (child.GetComponent<Property>().Card.displayName == displayName)
                                        {
                                            propCount++;
                                        }
                                    }
                                }
                                if (propCount >= count)
                                {
                                    missionsPanel.GetComponent<missionParent>().completeMission(mission);
                                }
                            }
                        }
                    }
                }
            }

        }
    }

    public void dragCancel()
    {
        Destroy(pendingParent.transform.GetChild(0).gameObject);
        externalAudioPlayer.GetComponent<AudioSource>().PlayOneShot(touchSound);
        ppDrag.SetActive(false);
        shopMenu.GetComponent<PurchaseController>().showUI();
        if (shopToggle.GetComponent<Toggle>().isOn == true)
        {
            shopMenu.SetActive(true);
            shopMenu.transform.localScale = Vector2.zero;
            shopMenu.transform.LeanScale(new Vector2(73.9463f, 73.9463f), 0.2f).setEaseOutBack();
        } else if (storageToggle.GetComponent<Toggle>().isOn == true)
        {
            storagePanel.SetActive(true);
            storagePanel.transform.localScale = Vector2.zero;
            storagePanel.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
        }
        if (moveToggle.GetComponent<Toggle>().isOn == true)
        {
            print("remove dark");
            Camera.main.GetComponent<SpriteDetector>().moveSelected.GetComponent<SpriteRenderer>().material.color = Color.white;
            Camera.main.GetComponent<SpriteDetector>().moveSelected = null;
        }
        // --------------------- Swapping to green border grass -------------
        map.SwapTile(greenGrass, tileGrass);
        // ------------------------------------------------------------------
    }
}
