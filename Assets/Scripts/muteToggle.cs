using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class muteToggle : MonoBehaviour
{
    public GameObject Camera, extAudio, statsObj;
    public void OnButtonClick()
    {
        if (this.GetComponent<Toggle>().isOn == true)
        {
            Camera.GetComponent<AudioSource>().volume = 0f;
            extAudio.GetComponent<AudioSource>().volume = 0f;
            statsObj.GetComponent<Statistics>().muted = true;
        }
        else if (this.GetComponent<Toggle>().isOn == false)
        {
            Camera.GetComponent<AudioSource>().volume = 1f;
            extAudio.GetComponent<AudioSource>().volume = 1f;
            statsObj.GetComponent<Statistics>().muted = false;
        }
    }
}
