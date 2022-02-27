using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveToggle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pressedToggle()
    {
        if (Camera.main.GetComponent<SpriteDetector>().moveSelected != null)
        {
            Camera.main.GetComponent<SpriteDetector>().moveSelected.GetComponent<SpriteRenderer>().material.color = Color.white;
            Camera.main.GetComponent<SpriteDetector>().moveSelected = null;
        }
    }
}
