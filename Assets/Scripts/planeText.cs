using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class planeText : MonoBehaviour
{
    public GameObject textField, stats, neighbourParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void refreshText()
    {
        if (neighbourParent.transform.GetChild(3).gameObject.activeSelf == false)
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
