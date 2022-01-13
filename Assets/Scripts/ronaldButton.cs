using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ronaldButton : MonoBehaviour
{

    [SerializeField]
    private Statistics stats;

    public void OnButtonClick()
    {
        stats.updateStats(diffmoney: 1000000, diffgold: 100, diffxp: 1000);
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
