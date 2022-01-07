using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class planeText : MonoBehaviour
{
    public GameObject textField, stats;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<TextMesh>().text != stats.GetComponent<Statistics>().cityName)
        {
            this.GetComponent<TextMesh>().text = stats.GetComponent<Statistics>().cityName;
        }
    }
}
