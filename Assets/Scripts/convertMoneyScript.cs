using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class convertMoneyScript : MonoBehaviour
{
    public GameObject stats, moneyBar, goldBar, convMoneyBar, convGoldBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void convert()
    {
        if (stats.GetComponent<Statistics>().gold >= 1)
        {
            stats.GetComponent<Statistics>().updateStats(diffmoney: 60000, diffgold: - 1);
            convMoneyBar.transform.GetChild(0).GetComponent<Text>().text = stats.GetComponent<Statistics>().moneyText.text;
            convGoldBar.transform.GetChild(0).GetComponent<Text>().text = stats.GetComponent<Statistics>().goldText.text;
        }
    }
}
