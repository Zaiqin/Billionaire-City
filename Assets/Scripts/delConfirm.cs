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
        selProp.GetComponent<Draggable>().rebuildPlots(selProp.GetComponent<Property>().Card, selProp.GetComponent<Draggable>().XY);
        selProp.transform.parent = null;
        Destroy(selProp);
        statsObj.GetComponent<Statistics>().updateStats(diffmoney: refundValue);
        saveloadobj.GetComponent<saveloadsystem>().saveGame();
    }

    public void cancelDel()
    {
        selProp.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
