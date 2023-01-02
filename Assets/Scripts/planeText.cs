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
        if (GameObject.Find("neighbourParent") != null)
        {
            if (GameObject.Find("neighbourParent").transform.GetChild(3).gameObject.activeSelf == false)
            {
                if (this.GetComponent<TextMesh>().text != stats.GetComponent<Statistics>().cityName)
                {
                    this.GetComponent<TextMesh>().text = stats.GetComponent<Statistics>().cityName;
                }
            }
            else
            {
                this.GetComponent<TextMesh>().text = "Chocolate Fields";
            }
        }
    }
}
