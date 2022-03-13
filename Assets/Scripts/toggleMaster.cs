using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleMaster : MonoBehaviour
{
    public Toggle shopToggle, plotToggle, roadToggle, deleteToggle, storageToggle;
    public GameObject cover;

    public bool checkAllOff()
    {
        if (shopToggle.isOn == false && plotToggle.isOn == false && roadToggle.isOn == false && deleteToggle.isOn == false && storageToggle.isOn == false)
        {
            return true;
        }
        return false;
    }
}
