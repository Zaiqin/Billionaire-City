using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission
{
    public string msnName;
    public string msnDesc;
    public float msnType;
    public string msnReward;
    public bool msnPending;
    public string msnMetadata;

    public Mission(string n, string d, float t, string r, bool b, string m)
    {
        msnName = n;
        msnDesc = d;
        msnType = t;
        msnReward = r;
        msnPending = b;
        msnMetadata = m;
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
    public GameObject missionController, stats, saveObj, claimButton, exclaimButton, storageController, extAudio, missionCompletePanel;
    public Image rewardImage;
    public Sprite money, gold;
    public AudioClip sound;

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
        // Get each type lists ----
        foreach (var item in missionList)
        {
            if (Math.Truncate(item.msnType) == 0)
            {
                stats.GetComponent<Statistics>().typeZero.Add(item);
                print("added " + item.msnName);
            }
        }
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
        print("pending count is " + pendingMissionList.Count);
        missionController.GetComponent<RecyclableScrollerMission>().missionlist = missionList;
        saveObj.GetComponent<saveloadsystem>().saveMissions();
        exclaimButton.SetActive(true);
        extAudio.GetComponent<AudioSource>().PlayOneShot(sound);
        missionCompletePanel.SetActive(true);
        print("pending count2 is " + pendingMissionList.Count);
        missionCompletePanel.GetComponent<missionPopup>().func(m.msnName);
        print("pending count4 is " + pendingMissionList.Count);

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
                descPanel.transform.LeanMoveLocalX(descPanel.transform.localPosition.x + 290, 0.2f).setEaseOutBack();
                missionPanel.transform.LeanMoveLocalX(missionPanel.transform.localPosition.x - 132, 0.2f).setEaseOutBack();
                extended = true;
            }
            else
            {
                descPanel.transform.LeanMoveLocalX(descPanel.transform.localPosition.x - 290, 0.2f).setEaseInBack();
                missionPanel.transform.LeanMoveLocalX(missionPanel.transform.localPosition.x + 132, 0.2f).setEaseInBack();
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
            rewardText.text = missionList[index].msnReward.Substring(0, missionList[index].msnReward.Length - 5) + " Gold";
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
        print("pending count7 is " + pendingMissionList.Count);
        pendingMissionList.Remove(missionList[chosenIndex].msnName);
        print("pending count8 is " + pendingMissionList.Count);
        doneMissionList.Add(missionList[chosenIndex].msnName);
        missionList.Remove(missionList[chosenIndex]);
        print("pending count9 is " + pendingMissionList.Count);
        saveObj.GetComponent<saveloadsystem>().saveMissions();
        print("pending count10 is " + pendingMissionList.Count);
        missionController.GetComponent<RecyclableScrollerMission>().missionRect.ReloadData();
        print("pending count11 is " + pendingMissionList.Count);
        descPanel.transform.LeanMoveLocalX(descPanel.transform.localPosition.x - 290, 0.2f).setEaseInBack();
        missionPanel.transform.LeanMoveLocalX(missionPanel.transform.localPosition.x + 132, 0.2f).setEaseInBack();
        extended = false;
        if (pendingMissionList.Count != 0)
        {
            exclaimButton.SetActive(true);
        } else
        {
            exclaimButton.SetActive(false);
        }
        print("pending count12 is " + pendingMissionList.Count);
    }
}
