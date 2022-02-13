using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wheel : MonoBehaviour
{
    bool set = true;
    int goal = 0;
    bool wheelEnabled = false;

    public GameObject spinButton, closeButton, stats, luckReward, storageController;
    public Image luckImg;
    public Text luckText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void startWheel()
    {
        if (stats.GetComponent<Statistics>().returnStats()[1] > 0)
        {
            stats.GetComponent<Statistics>().updateStats(diffgold: -1);
            Invoke("stopRotation", 3.0f);
            Invoke("showPopup", 5.5f);
            set = false;
            goal = Random.Range(0, 360);
            spinButton.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0.5f);
            closeButton.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0.5f);
            spinButton.GetComponent<Button>().interactable = false;
            closeButton.GetComponent<Button>().interactable = false;
            wheelEnabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (wheelEnabled == true)
        {
            if (set == false)
            {
                Vector3 destination = new Vector3(0, 0, 900);
                transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, destination, Time.deltaTime);
            }
            else
            {
                Vector3 destination = new Vector3(0, 0, goal);
                transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, destination, Time.deltaTime);
            }
        }
    }

    void stopRotation()
    {
        set = true;
    }

    void showPopup()
    {
        luckReward.SetActive(true);
        int won = 1 + (int)(this.transform.eulerAngles.z / (360 / 8));
        luckImg.sprite = GameObject.Find("rew" + won).GetComponent<Image>().sprite;
        luckText.text = "x 1";
        switch (won)
        {
            case 1: storageController.GetComponent<RecyclableScrollerStorage>().addIntoStorage("Three Storey"); break;
            case 2: stats.GetComponent<Statistics>().updateStats(diffxp: 1000); luckText.text = "+ 1,000XP"; break;
            case 3: storageController.GetComponent<RecyclableScrollerStorage>().addIntoStorage("Bungalow Luxury"); break;
            case 4: stats.GetComponent<Statistics>().updateStats(diffgold: 1); luckText.text = "+ 1 Gold"; break;
            case 5: storageController.GetComponent<RecyclableScrollerStorage>().addIntoStorage("Coffee Shop"); break;
            case 6: storageController.GetComponent<RecyclableScrollerStorage>().addIntoStorage("Orange Tree"); break;
            case 7: storageController.GetComponent<RecyclableScrollerStorage>().addIntoStorage("Apartment Block"); break;
            case 8: stats.GetComponent<Statistics>().updateStats(diffmoney: 10000); luckText.text = "+ $10,000"; break;
            default:
                break;
        }
        print("won number " + won + " for goal " + goal + "with euler " + this.transform.eulerAngles.z);
    }

    public void restartWheel()
    {
        wheelEnabled = false;
        spinButton.GetComponent<Image>().color = Color.white;
        closeButton.GetComponent<Image>().color = Color.white;
        spinButton.GetComponent<Button>().interactable = true;
        closeButton.GetComponent<Button>().interactable = true;
    }
}
