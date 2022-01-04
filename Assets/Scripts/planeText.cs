using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class planeText : MonoBehaviour
{
    public GameObject textField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (textField.GetComponent<Text>().text != "")
        {
            this.GetComponent<TextMesh>().text = textField.GetComponent<Text>().text;
        }
    }
}
