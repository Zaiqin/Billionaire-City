using PolyAndCode.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ronaldButton : MonoBehaviour
{

    [SerializeField]
    private GameObject missionPanel;
    public RecyclableScrollRect rect;

    public void OnButtonClick()
    {
        missionPanel.SetActive(true);
        missionPanel.transform.localScale = Vector2.zero;
        missionPanel.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
        missionPanel.GetComponent<missionParent>().resetDesc();
        
        //stats.updateStats(diffmoney: 1000000, diffgold: 100, diffxp: 1000);
    }
    public void reload()
    {
        rect.ReloadData();
    }

}
