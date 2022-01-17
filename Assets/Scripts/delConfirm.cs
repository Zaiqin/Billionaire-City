using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delConfirm : MonoBehaviour
{
    public GameObject selProp, saveloadobj, statsObj;
    public long refundValue;
    public void deleteProp()
    {
        print("called destroy");
        print("xy is " + selProp.GetComponent<Draggable>().XY[0] + "card is " + selProp.GetComponent<Property>().Card);
        if (selProp.GetComponent<Property>().Card.type == "Wonder")
        {
            if (selProp.GetComponent<Property>().Card.wonderBonus >= 100 && selProp.GetComponent<Property>().Card.wonderBonus < 1000)
            {
                statsObj.GetComponent<Statistics>().doubleChance -= (int)(((float)selProp.GetComponent<Property>().Card.wonderBonus) / 100);
            }
            else if (selProp.GetComponent<Property>().Card.wonderBonus > 10 && selProp.GetComponent<Property>().Card.wonderBonus < 100)
            {
                statsObj.GetComponent<Statistics>().wonderCommerceBonus -= (int)(((float)selProp.GetComponent<Property>().Card.wonderBonus) / 10);
                statsObj.GetComponent<Statistics>().commerceWonders.Remove(selProp.GetComponent<Property>().Card.displayName);
            }
            else
            {
                statsObj.GetComponent<Statistics>().wonderBonus -= selProp.GetComponent<Property>().Card.wonderBonus;
            }
            statsObj.GetComponent<Statistics>().builtWonders.Remove(selProp.GetComponent<Property>().Card.displayName);
        }
        selProp.GetComponent<Draggable>().rebuildPlots(selProp.GetComponent<Property>().Card, selProp.GetComponent<Draggable>().XY);
        selProp.transform.parent = null;
        Destroy(selProp);
        statsObj.GetComponent<Statistics>().updateStats(diffmoney: refundValue);
        saveloadobj.GetComponent<saveloadsystem>().saveGame();
    }

    public void cancelDel()
    {
        if (selProp.name != "Main Camera")
        {
            selProp.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
