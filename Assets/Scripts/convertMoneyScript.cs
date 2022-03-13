using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class convertMoneyScript : MonoBehaviour
{
    public GameObject stats, moneyBar, goldBar, convMoneyBar, convGoldBar;

    public void convert()
    {
        if (stats.GetComponent<Statistics>().gold >= 1)
        {
            stats.GetComponent<Statistics>().updateStats(diffmoney: 60000, diffgold: - 1);
            convMoneyBar.transform.GetChild(0).GetComponent<Text>().text = stats.GetComponent<Statistics>().moneyText.text;
            convGoldBar.transform.GetChild(0).GetComponent<Text>().text = stats.GetComponent<Statistics>().goldText.text;
        }
    }

    public void closePanel()
    {
        print("close");
        this.transform.LeanScale(Vector2.zero, 0.2f).setEaseInBack();
        Invoke("setInactive", 0.2f);
    }

    void setInactive()
    {
        this.gameObject.SetActive(false);
        this.gameObject.transform.localScale = Vector2.one;
    }
}
