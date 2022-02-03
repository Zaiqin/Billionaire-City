using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class missionParent : MonoBehaviour
{
    public GameObject descPanel;
    public Text descText;
    public bool extended = false;
    public int chosenIndex;

    private string[] descArray = new string[] { "Pizzalicious desc", "Pizzalicious Clients I desc", "Pizzalicious Sales I desc", "Name It desc", "Instant Build desc", "Green Thumb I desc", "M-City's VIP Club desc" };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleDesc(int index)
    {
        if (chosenIndex == index || extended == false)
        {
            if (extended == false)
            {
                descPanel.transform.localPosition = new Vector3(descPanel.transform.localPosition.x + 290, descPanel.transform.localPosition.y, descPanel.transform.localPosition.z);
                extended = true;
            }
            else
            {
                descPanel.transform.localPosition = new Vector3(descPanel.transform.localPosition.x - 290, descPanel.transform.localPosition.y, descPanel.transform.localPosition.z);
                extended = false;
            }
        }
        descText.text = descArray[index];
        chosenIndex = index;
    }
}
