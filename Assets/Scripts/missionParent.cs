using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class missionParent : MonoBehaviour
{
    public GameObject descPanel;
    public GameObject missionPanel;
    public Text descText;
    public Text descTitle;
    public bool extended = false;
    public int chosenIndex;
    public GameObject missionController;

    public List<string> missionArray;
    public List<string> descArray;

    // Start is called before the first frame update
    void Start()
    {
        missionArray = GameObject.Find("CSV").GetComponent<CSVReader>().missionName;
        descArray = GameObject.Find("CSV").GetComponent<CSVReader>().missionDesc;
        missionController.GetComponent<RecyclableScrollerMission>().missionNameList = missionArray;
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
        descTitle.text = missionArray[index];
        descText.text = descArray[index];
        chosenIndex = index;
    }
}
