using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class luckToggle : MonoBehaviour
{
    public GameObject luckPanel;
    public void OnButtonClick()
    {
        luckPanel.SetActive(true);
        luckPanel.transform.localScale = Vector2.zero;
        luckPanel.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
        
    }

    public void closeLuckPanel()
    {
        print("clsoe luck");
        luckPanel.transform.LeanScale(Vector2.zero, 0.2f).setEaseInBack();
        Invoke("setInactive", 0.2f);
    }

    void setInactive()
    {
        luckPanel.SetActive(false);
        luckPanel.transform.localScale = Vector2.one;
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
