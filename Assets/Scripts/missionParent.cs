using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission
{
    public string msnName;
    public string msnDesc;
    public int msnType;
    public string msnReward;
    public bool msnPending;

    public Mission(string n, string d, int t, string r, bool b)
    {
        msnName = n;
        msnDesc = d;
        msnType = t;
        msnReward = r;
        msnPending = b;
    }
}

public class missionParent : MonoBehaviour
{
    public GameObject descPanel;
    public GameObject missionPanel;
    public Text descText;
    public Text descTitle;
    public Text rewardText;
    public bool extended = false;
    public int chosenIndex;
    public GameObject missionController, stats, saveObj, claimButton, exclaimButton, storageController;
    public Image rewardImage;
    public Sprite money, gold;

    public CSVReader CSVObject;
    private Dictionary<string, PropertyCard> database;

    public List<Mission> missionList;
    public List<string> doneMissionList;
    public List<string> pendingMissionList;

    // Start is called before the first frame update
    void Start()
    {
        database = CSVObject.CardDatabase;
    }

    public void initMissions()
    {
        List<Mission> temp = new List<Mission>();
        foreach (var item in GameObject.Find("CSV").GetComponent<CSVReader>().missionList)
        {
            temp.Add(item);
            foreach (var pend in pendingMissionList)
            {
                if (item.msnName == pend)
                {
                    item.msnPending = true;
                }
            }
        }
        foreach (var item in GameObject.Find("CSV").GetComponent<CSVReader>().missionList)
        {
            foreach (var done in doneMissionList)
            {
                if (item.msnName == done)
                {
                    temp.Remove(item);
                    break;
                }
            }
        }
        missionList = temp;
        missionController.GetComponent<RecyclableScrollerMission>().missionlist = missionList;
        if (pendingMissionList.Count != 0)
        {
            exclaimButton.SetActive(true);
        }
        else
        {
            exclaimButton.SetActive(false);
        }
    }

    public void completeMission(Mission m)
    {
        print("complete mission " + m.msnName);
        m.msnPending = true;
        pendingMissionList.Add(m.msnName);
        missionController.GetComponent<RecyclableScrollerMission>().missionlist = missionList;
        saveObj.GetComponent<saveloadsystem>().saveMissions();
        exclaimButton.SetActive(true);
    }

    public void resetDesc()
    {
        print("resetting desc");
        chosenIndex = 0;
        extended = false;
        descPanel.transform.localPosition = new Vector3(-54, descPanel.transform.localPosition.y, descPanel.transform.localPosition.z);
        missionPanel.transform.localPosition = new Vector3(50, missionPanel.transform.localPosition.y, missionPanel.transform.localPosition.z);
        print("done resetting desc");
    }

    public void toggleDesc(int index)
    {
        claimButton.SetActive(false);
        if (chosenIndex == index || extended == false)
        {
            if (extended == false)
            {
                descPanel.transform.localPosition = new Vector3(descPanel.transform.localPosition.x + 290, descPanel.transform.localPosition.y, descPanel.transform.localPosition.z);
                missionPanel.transform.localPosition = new Vector3(missionPanel.transform.localPosition.x - 132, missionPanel.transform.localPosition.y, missionPanel.transform.localPosition.z);
                extended = true;
            }
            else
            {
                descPanel.transform.localPosition = new Vector3(descPanel.transform.localPosition.x - 290, descPanel.transform.localPosition.y, descPanel.transform.localPosition.z);
                missionPanel.transform.localPosition = new Vector3(missionPanel.transform.localPosition.x + 132, missionPanel.transform.localPosition.y, missionPanel.transform.localPosition.z);
                extended = false;
            }
        }
        descTitle.text = missionList[index].msnName;
        descText.text = missionList[index].msnDesc;
        if (missionList[index].msnReward.Contains("Prop"))
        {
            print("string is " + missionList[index].msnReward.Substring(4, missionList[index].msnReward.Length-4));
            rewardText.text = database[missionList[index].msnReward.Substring(4, missionList[index].msnReward.Length - 4)].displayName + " x1";
            rewardImage.sprite = database[missionList[index].msnReward.Substring(4, missionList[index].msnReward.Length-4)].propImage;
        } else if (missionList[index].msnReward.Contains("Gold"))
        {
            rewardText.text = missionList[index].msnReward.Substring(0, missionList[index].msnReward.Length - 5);
            rewardImage.sprite = gold;
        } else
        {
            rewardText.text = "$" + int.Parse(missionList[index].msnReward).ToString("#,##0");
            rewardImage.sprite = money;
        }
        chosenIndex = index;
        if (missionList[index].msnPending == true)
        {
            claimButton.SetActive(true);
        }
    }

    public void claimReward()
    {
        if (missionList[chosenIndex].msnReward.Contains("Prop"))
        {
            print("string is " + missionList[chosenIndex].msnReward.Substring(4, missionList[chosenIndex].msnReward.Length - 4));
            storageController.GetComponent<RecyclableScrollerStorage>().addIntoStorage(missionList[chosenIndex].msnReward.Substring(4, missionList[chosenIndex].msnReward.Length - 4));
        }
        else if (missionList[chosenIndex].msnReward.Contains("Gold"))
        {
            stats.GetComponent<Statistics>().updateStats(diffgold: int.Parse(missionList[chosenIndex].msnReward.Substring(0, missionList[chosenIndex].msnReward.Length - 5)));
        }
        else
        {
            stats.GetComponent<Statistics>().updateStats(diffmoney: int.Parse(missionList[chosenIndex].msnReward));
        }
        pendingMissionList.Remove(missionList[chosenIndex].msnName);
        doneMissionList.Add(missionList[chosenIndex].msnName);
        missionList.Remove(missionList[chosenIndex]);
        saveObj.GetComponent<saveloadsystem>().saveMissions();
        missionController.GetComponent<RecyclableScrollerMission>().missionRect.ReloadData();
        descPanel.transform.localPosition = new Vector3(descPanel.transform.localPosition.x - 290, descPanel.transform.localPosition.y, descPanel.transform.localPosition.z);
        missionPanel.transform.localPosition = new Vector3(missionPanel.transform.localPosition.x + 132, missionPanel.transform.localPosition.y, missionPanel.transform.localPosition.z);
        extended = false;
        if (pendingMissionList.Count != 0)
        {
            exclaimButton.SetActive(true);
        } else
        {
            exclaimButton.SetActive(false);
        }
    }
}
