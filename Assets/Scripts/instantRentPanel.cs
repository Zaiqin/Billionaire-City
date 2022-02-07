using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class instantRentPanel : MonoBehaviour
{
    public GameObject infoPanel;
    public Text beforeText, timeText;
    public GameObject stats, saveObj, missionsPanel;

    public void initPanel()
    {
        beforeText.text = timeText.text;
    }

    public void confirmInstant()
    {
        GameObject prop = infoPanel.GetComponent<infoScript>().selProp;
        if (stats.GetComponent<Statistics>().returnStats()[1] > 0)
        {
            stats.GetComponent<Statistics>().updateStats(diffgold: -1);
            prop.GetComponent<SpriteRenderer>().material.color = Color.white;
            Destroy(prop.transform.GetChild(4).gameObject);
            prop.transform.GetChild(0).GetComponent<contractScript>().signTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            this.gameObject.SetActive(false);
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
        Destroy(prop.transform.GetChild(4).gameObject);
        prop.transform.GetChild(0).GetComponent<contractScript>().signTime = "notsigned";
        prop.transform.GetChild(0).GetComponent<contractScript>().signCreationTime = "notsigned";
        prop.transform.GetChild(0).GetComponent<contractScript>().signIndex = -1;
        prop.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        this.gameObject.SetActive(false);
        infoPanel.SetActive(false);
        saveObj.GetComponent<saveloadsystem>().saveGame();
    }
}
