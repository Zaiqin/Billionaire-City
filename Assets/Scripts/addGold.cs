using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addGold : MonoBehaviour
{
    public GameObject stats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGold()
    {
        stats.GetComponent<Statistics>().updateStats(diffgold: 10);
    }
}
