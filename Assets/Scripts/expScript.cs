using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class expScript : MonoBehaviour, IPointerClickHandler
{

    public GameObject expPopup, cam, extAudio, text;
    public AudioClip touchsound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (cam.GetComponent<CameraMovement>().dragging == false && GameObject.Find("Canvas").GetComponent<toggleMaster>().checkAllOff() == true)
        {
            print("name is " + this.name);
            expPopup.GetComponent<expansion>().i = int.Parse(this.name.Substring(this.name.Length - 2));
            extAudio.GetComponent<AudioSource>().PlayOneShot(touchsound);
            expPopup.SetActive(true);

            string cost = "";
            if (int.Parse(this.name.Substring(this.name.Length - 2)) < 20)
            {
                cost = "$1,000,000";
            } else if (int.Parse(this.name.Substring(this.name.Length - 2)) > 20 && int.Parse(this.name.Substring(this.name.Length - 2)) < 30)
            {
                cost = "$5,000,000";
            }
            else if (int.Parse(this.name.Substring(this.name.Length - 2)) > 30 && int.Parse(this.name.Substring(this.name.Length - 2)) < 40)
            {
                cost = "$10M";
            }
            else if (int.Parse(this.name.Substring(this.name.Length - 2)) > 40 && int.Parse(this.name.Substring(this.name.Length - 2)) < 50)
            {
                cost = "$50M";
            }
            else if (int.Parse(this.name.Substring(this.name.Length - 2)) > 50 && int.Parse(this.name.Substring(this.name.Length - 2)) < 60)
            {
                cost = "$100M";
            }
            text.GetComponent<Text>().text = cost;
        }
    }
}
