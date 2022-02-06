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

    public Mission(string n, string d, int t, string r)
    {
        msnName = n;
        msnDesc = d;
        msnType = t;
        msnReward = r;
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
    public GameObject missionController, stats, saveObj;
    public Image rewardImage;
    public Sprite money, gold;

    public List<Mission> missionList;
    public List<string> doneMissionList;

    // Start is called before the first frame update
    void Start()
    {
        List<Mission> temp = new List<Mission>();
        foreach (var item in GameObject.Find("CSV").GetComponent<CSVReader>().missionList)
        {
            temp.Add(item);
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
    }

    public void resetDesc()
    {
        chosenIndex = 0;
        extended = false;
        descPanel.transform.localPosition = new Vector3(-54, descPanel.transform.localPosition.y, descPanel.transform.localPosition.z);
        missionPanel.transform.localPosition = new Vector3(50, missionPanel.transform.localPosition.y, missionPanel.transform.localPosition.z);
    }

    public void toggleDesc(int index)
    {
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
        if (missionList[index].msnReward.Contains("Gold"))
        {
            rewardText.text = missionList[index].msnReward.Substring(0, missionList[index].msnReward.Length - 5);
            rewardImage.sprite = gold;
        } else
        {
            rewardText.text = "$" + int.Parse(missionList[index].msnReward).ToString("#,##0");
            rewardImage.sprite = money;
        }
        chosenIndex = index;
    }

    public void claimReward()
    {
        if (missionList[chosenIndex].msnReward.Contains("Gold"))
        {
            stats.GetComponent<Statistics>().updateStats(diffgold: int.Parse(missionList[chosenIndex].msnReward.Substring(0, missionList[chosenIndex].msnReward.Length - 5)));
        }
        else
        {
            stats.GetComponent<Statistics>().updateStats(diffmoney: int.Parse(missionList[chosenIndex].msnReward));
        }
        doneMissionList.Add(missionList[chosenIndex].msnName);
        missionList.Remove(missionList[chosenIndex]);
        saveObj.GetComponent<saveloadsystem>().saveMissions();
        missionController.GetComponent<RecyclableScrollerMission>().missionRect.ReloadData();
        descPanel.transform.localPosition = new Vector3(descPanel.transform.localPosition.x - 290, descPanel.transform.localPosition.y, descPanel.transform.localPosition.z);
        missionPanel.transform.localPosition = new Vector3(missionPanel.transform.localPosition.x + 132, missionPanel.transform.localPosition.y, missionPanel.transform.localPosition.z);
        extended = false;
    }
}
