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

            bool selectedLocked = false;
            switch (int.Parse(this.name.Substring(this.name.Length - 2)))
            {
                case 11: break;
                case 12: break;
                case 13: break;
                case 14: break;
                case 21: if (expPopup.GetComponent<expansion>().deletedExp.Count < 4) { selectedLocked = true; }; break;
                case 22: if (expPopup.GetComponent<expansion>().deletedExp.Count < 4) { selectedLocked = true; }; break;
                case 23: if (expPopup.GetComponent<expansion>().deletedExp.Count < 4) { selectedLocked = true; }; break;
                case 24: if (expPopup.GetComponent<expansion>().deletedExp.Count < 4) { selectedLocked = true; }; break;
                case 31: if (expPopup.GetComponent<expansion>().deletedExp.Count < 8) { selectedLocked = true; }; break;
                case 32: if (expPopup.GetComponent<expansion>().deletedExp.Count < 8) { selectedLocked = true; }; break;
                case 33: if (expPopup.GetComponent<expansion>().deletedExp.Count < 8) { selectedLocked = true; }; break;
                case 34: if (expPopup.GetComponent<expansion>().deletedExp.Count < 8) { selectedLocked = true; }; break;
                case 41: if (expPopup.GetComponent<expansion>().deletedExp.Count < 12) { selectedLocked = true; }; break;
                case 42: if (expPopup.GetComponent<expansion>().deletedExp.Count < 12) { selectedLocked = true; }; break;
                case 43: if (expPopup.GetComponent<expansion>().deletedExp.Count < 12) { selectedLocked = true; }; break;
                case 44: if (expPopup.GetComponent<expansion>().deletedExp.Count < 12) { selectedLocked = true; }; break;
                case 45: if (expPopup.GetComponent<expansion>().deletedExp.Count < 12) { selectedLocked = true; }; break;
                case 46: if (expPopup.GetComponent<expansion>().deletedExp.Count < 12) { selectedLocked = true; }; break;
                case 47: if (expPopup.GetComponent<expansion>().deletedExp.Count < 12) { selectedLocked = true; }; break;
                case 48: if (expPopup.GetComponent<expansion>().deletedExp.Count < 12) { selectedLocked = true; }; break;
                case 51: if (expPopup.GetComponent<expansion>().deletedExp.Count < 20) { selectedLocked = true; }; break;
                case 52: if (expPopup.GetComponent<expansion>().deletedExp.Count < 20) { selectedLocked = true; }; break;
                case 53: if (expPopup.GetComponent<expansion>().deletedExp.Count < 20) { selectedLocked = true; }; break;
                case 54: if (expPopup.GetComponent<expansion>().deletedExp.Count < 20) { selectedLocked = true; }; break;
                default:
                    break;
            }

            expPopup.GetComponent<expansion>().i = int.Parse(this.name.Substring(this.name.Length - 2));
            extAudio.GetComponent<AudioSource>().PlayOneShot(touchsound);

            if (selectedLocked == false)
            {
                expPopup.SetActive(true);

                int cost = 0;
                switch (expPopup.GetComponent<expansion>().deletedExp.Count)
                {
                    case 0: cost = 4000000; break;
                    case 1: cost = 6000000; break;
                    case 2: cost = 7000000; break;
                    case 3: cost = 9000000; break;
                    case 4: cost = 30000000; break;
                    case 5: cost = 36000000; break;
                    case 6: cost = 42000000; break;
                    case 7: cost = 48000000; break;
                    case 8: cost = 60000000; break;
                    case 9: cost = 72000000; break;
                    case 10: cost = 84000000; break;
                    case 11: cost = 96000000; break;
                    case 12: cost = 180000000; break;
                    case 13: cost = 195000000; break;
                    case 14: cost = 210000000; break;
                    case 15: cost = 228000000; break;
                    case 16: cost = 300000000; break;
                    case 17: cost = 360000000; break;
                    case 18: cost = 420000000; break;
                    case 19: cost = 480000000; break;
                    case 20: cost = 540000000; break;
                    case 21: cost = 600000000; break;
                    case 22: cost = 660000000; break;
                    case 23: cost = 800000000; break;
                    default:
                        break;
                }
                text.GetComponent<Text>().text = "$" + cost.ToString("#,##0");
            } else
            {
                expPopup.GetComponent<expansion>().failPopup.SetActive(true);
            }
        }
    }
}
