using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveToggle : MonoBehaviour
{
    public GameObject infoPanel;

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
        if (infoPanel.GetComponent<infoScript>().highlightedProp != null)
        {
            if (Camera.main.GetComponent<SpriteDetector>().selectedCommerce != null)
            {
                Camera.main.GetComponent<SpriteDetector>().selectedCommerce.GetComponent<SpriteRenderer>().material.color = Color.white;
                Camera.main.GetComponent<SpriteDetector>().selectedCommerce.transform.GetChild(0).gameObject.SetActive(false);
                Camera.main.GetComponent<SpriteDetector>().selectedCommerce.transform.GetChild(0).GetComponent<influence>().removeHighlights();
            }
            if (infoPanel.GetComponent<infoScript>().highlightedProp != null)
            {
                infoPanel.GetComponent<infoScript>().highlightedProp.GetComponent<SpriteRenderer>().material.color = Color.white;
                if (infoPanel.GetComponent<infoScript>().highlightedProp.transform.childCount > 4)
                {
                    print("Destroy this too");
                    Destroy(infoPanel.GetComponent<infoScript>().highlightedProp.transform.GetChild(4).gameObject);
                }
                if (infoPanel.GetComponent<infoScript>().highlightedProp.GetComponent<Property>().Card.type == "Deco")
                {
                    infoPanel.GetComponent<infoScript>().highlightedProp.transform.GetChild(0).gameObject.SetActive(false);
                }
                if (infoPanel.GetComponent<infoScript>().highlightedProp.transform.GetChild(0).name == "Influence")
                {
                    infoPanel.GetComponent<infoScript>().highlightedProp.transform.GetChild(0).gameObject.SetActive(false);
                    infoPanel.GetComponent<infoScript>().highlightedProp.transform.GetChild(0).GetComponent<influence>().removeHighlights();
                }
            }
        }
    }
}
