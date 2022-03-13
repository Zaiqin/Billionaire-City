using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addGold : MonoBehaviour
{
    public GameObject stats;

    public void AddGold()
    {
        stats.GetComponent<Statistics>().updateStats(diffgold: 10);
    }
}
