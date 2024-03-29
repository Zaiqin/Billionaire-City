using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class instantRentPanel : MonoBehaviour
{
    public GameObject infoPanel, instantPanel;
    public Text beforeText, timeText;
    public GameObject stats, saveObj, missionsPanel;

    public void initPanel()
    {
        beforeText.text = timeText.text;
        instantPanel.SetActive(true);
        instantPanel.transform.localScale = Vector2.zero;
        instantPanel.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
    }

    public void confirmInstant()
    {
        GameObject prop = infoPanel.GetComponent<infoScript>().selProp;
        if (stats.GetComponent<Statistics>().returnStats()[1] > 0)
        {
            stats.GetComponent<Statistics>().updateStats(diffgold: -1);
            prop.GetComponent<SpriteRenderer>().material.color = Color.white;
            prop.transform.GetChild(0).GetComponent<contractScript>().signTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            closePanel();
            infoPanel.SetActive(false);
            saveObj.GetComponent<saveloadsystem>().saveGame();
            // ---------- Type 4 Missions -------------------
            if (missionsPanel.GetComponent<missionParent>().missionList != null)
            {
                foreach (var item in missionsPanel.GetComponent<missionParent>().missionList)
                {
                    if (item.msnType == 4 && item.msnPending == false)
                    {
                        if (item.msnName == "Instant Rent")
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                    }
                }
            }
        }
    }

    public void cancelContract()
    {
        GameObject prop = infoPanel.GetComponent<infoScript>().selProp;
        prop.GetComponent<SpriteRenderer>().material.color = Color.white;
        prop.transform.GetChild(0).GetComponent<contractScript>().signTime = "notsigned";
        prop.transform.GetChild(0).GetComponent<contractScript>().signCreationTime = "notsigned";
        prop.transform.GetChild(0).GetComponent<contractScript>().signIndex = -1;
        prop.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        closePanel();
        infoPanel.SetActive(false);
        saveObj.GetComponent<saveloadsystem>().saveGame();
    }

    public void closePanel()
    {
        this.transform.LeanScale(Vector2.zero, 0.2f).setEaseInBack();
        Invoke("setInactive", 0.2f);
    }

    void setInactive()
    {
        this.gameObject.SetActive(false);
        this.gameObject.transform.localScale = Vector2.one;
    }
}
