using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopToggleScript : MonoBehaviour
{

    [SerializeField]
    public Toggle shopToggle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void offShopButton()
    {
        //turns off shop Button when buy button is pressed
        shopToggle.isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
