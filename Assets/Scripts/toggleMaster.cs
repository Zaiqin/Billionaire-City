using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleMaster : MonoBehaviour
{
    public Toggle shopToggle, plotToggle, roadToggle, deleteToggle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool checkAllOff()
    {
        if (shopToggle.isOn == false && plotToggle.isOn == false && roadToggle.isOn == false && deleteToggle.isOn == false)
        {
            return true;
        }
        return false;
    }
}
