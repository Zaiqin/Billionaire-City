using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class planeText : MonoBehaviour
{
    public GameObject textField, stats, dragCover;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dragCover.activeSelf == false)
        {
            if (this.GetComponent<TextMesh>().text != stats.GetComponent<Statistics>().cityName) 
            {
                this.GetComponent<TextMesh>().text = stats.GetComponent<Statistics>().cityName;
            }
        } else
        {
            this.GetComponent<TextMesh>().text = "Chocolate Fields";
        }
        
    }
}
