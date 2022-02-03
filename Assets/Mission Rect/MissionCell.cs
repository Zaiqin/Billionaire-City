using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using System;

//Cell class for demo. A cell in Recyclable Scroll Rect must have a cell class inheriting from ICell.
//The class is required to configure the cell(updating UI elements etc) according to the data during recycling of cells.
//The configuration of a cell is done through the DataSource SetCellData method.
//Check RecyclableScrollerDemo class
public class MissionCell : MonoBehaviour, ICell
{
    //UI
    public Button readMoreButton;
    public Text missionName;

    public AudioClip touchSound;
    public GameObject descPanel;

    private bool move = false;
    private Vector3 descEnd;

    //Model
    private int _cellIndex;

    private void Start()
    {
        readMoreButton.GetComponent<Button>().onClick.AddListener(readListener);
        descPanel = GameObject.Find("descPanel");
        descEnd = new Vector3(7.1f, descPanel.transform.position.y, descPanel.transform.position.z);
    }

    private void Update()
    {
        if (move == true)
        {
            descPanel.transform.position = Vector3.MoveTowards(descPanel.transform.position, descEnd, 50f * Time.deltaTime);
        }
        if (descPanel.transform.position.x >= descEnd.x)
        {
            move = false;
        }
    }

    private void readListener()
    {
        GameObject.Find("ExternalAudioPlayer").GetComponent<AudioSource>().PlayOneShot(touchSound);
        move = true;
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(int cellIndex, string text)
    {
        _cellIndex = cellIndex;
        missionName.text = text;
    }

}
