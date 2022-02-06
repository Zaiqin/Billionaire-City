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
        print("clicked ronald button");
        missionPanel.SetActive(true);
        missionPanel.GetComponent<missionParent>().resetDesc();
        print("done ronald button click");
        
        //stats.updateStats(diffmoney: 1000000, diffgold: 100, diffxp: 1000);
    }
    public void reload()
    {
        print("start reload msn rect");
        rect.ReloadData();
        print("end reload msn rect");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
