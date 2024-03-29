using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class delConfirm : MonoBehaviour
{
    public GameObject selProp, saveloadobj, statsObj, moveToggle;
    public long refundValue;
    public void deleteProp()
    {
        print("called destroy");
        print("xy is " + selProp.GetComponent<Draggable>().XY[0] + "card is " + selProp.GetComponent<Property>().Card.propName);
        if (selProp.GetComponent<Property>().Card.type == "Wonder")
        {
            if (selProp.GetComponent<Property>().Card.wonderBonus >= 100)
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
        if (moveToggle.GetComponent<Toggle>().isOn == false)
        {
            statsObj.GetComponent<Statistics>().updateStats(diffmoney: refundValue);
        }
        saveloadobj.GetComponent<saveloadsystem>().saveGame();
        closePanel();
    }

    public void cancelDel()
    {
        if (selProp.name != "Main Camera")
        {
            selProp.GetComponent<SpriteRenderer>().color = Color.white;
            closePanel();
        }
    }

    public void closePanel()
    {
        print("close");
        this.transform.parent.gameObject.transform.LeanScale(Vector2.zero, 0.2f).setEaseInBack();
        Invoke("setInactive", 0.2f);
    }

    void setInactive()
    {
        this.transform.parent.gameObject.SetActive(false);
        this.transform.parent.gameObject.transform.localScale = Vector2.one;
    }
}
