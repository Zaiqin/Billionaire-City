using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantBuild : MonoBehaviour
{
    public GameObject save, infoPanel, stats, extAudio, missionsPanel;
    public AudioClip constSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void instantButton()
    {
        if (stats.GetComponent<Statistics>().returnStats()[0] < infoPanel.GetComponent<infoScript>().instantCost)
        {
            print("insufficient money");
        } else
        {
            instant();
            stats.GetComponent<Statistics>().updateStats(diffmoney: -infoPanel.GetComponent<infoScript>().instantCost);
            extAudio.GetComponent<AudioSource>().PlayOneShot(constSound);
        }
    }

    public void instant()
    {
        SpriteRenderer renderer = this.transform.parent.GetComponent<infoScript>().selProp.GetComponent<SpriteRenderer>();
        renderer.sprite = this.transform.parent.GetComponent<infoScript>().selProp.GetComponent<Property>().Card.propImage;
        renderer.sprite = Sprite.Create(this.transform.parent.GetComponent<infoScript>().selProp.GetComponent<Property>().Card.propImage.texture, new Rect(0, 0, this.transform.parent.GetComponent<infoScript>().selProp.GetComponent<Property>().Card.propImage.texture.width, this.transform.parent.GetComponent<infoScript>().selProp.GetComponent<Property>().Card.propImage.texture.height), new Vector2(0f, 0f), 32);
        this.transform.parent.GetComponent<infoScript>().selProp.GetComponent<Property>().constructEnd = "na";
        this.transform.parent.GetComponent<infoScript>().selProp.GetComponent<Property>().constructStart = "na";

        if (this.transform.parent.GetComponent<infoScript>().selProp.GetComponent<Property>().Card.type == "House")
        {
            this.transform.parent.GetComponent<infoScript>().selProp.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
            this.transform.parent.GetComponent<infoScript>().selProp.transform.GetChild(1).gameObject.AddComponent<BoxCollider2D>();

            this.transform.parent.GetComponent<infoScript>().selProp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }

        infoPanel.GetComponent<infoScript>().highlightedProp.GetComponent<SpriteRenderer>().material.color = Color.white;
        switch (this.transform.parent.GetComponent<infoScript>().selProp.GetComponent<Property>().Card.type)
        {
            case "House":
                Destroy(infoPanel.GetComponent<infoScript>().highlightedProp.transform.GetChild(4).gameObject);
                break;
            case "Commerce":
                this.transform.parent.GetComponent<infoScript>().selProp.transform.GetChild(0).gameObject.SetActive(false);
                this.transform.parent.GetComponent<infoScript>().selProp.transform.GetChild(0).GetComponent<influence>().removeHighlights();

                DateTime theTime;
                theTime = DateTime.Now.AddMinutes(3);
                print("signing property commerce after building");
                string datetime = theTime.ToString("yyyy/MM/dd HH:mm:ss");
                this.transform.parent.GetComponent<infoScript>().selProp.GetComponent<Property>().transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime = datetime;
                this.transform.parent.GetComponent<infoScript>().selProp.GetComponent<Property>().transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signCreationTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                print("sign time is " + datetime);
                this.transform.parent.GetComponent<infoScript>().selProp.GetComponent<Property>().transform.GetChild(0).gameObject.SetActive(false);
                this.transform.parent.GetComponent<infoScript>().selProp.GetComponent<Property>().transform.GetChild(0).GetComponent<influence>().removeHighlights();
                break;
            default: break;
        }

        save.GetComponent<saveloadsystem>().saveGame();
        infoPanel.SetActive(false);
        // ----- Type 2 Missions -----------------
        if (missionsPanel.GetComponent<missionParent>().missionList != null)
        {
            foreach (var item in missionsPanel.GetComponent<missionParent>().missionList)
            {
                if (item.msnType == 2 && item.msnPending == false)
                {
                    if (item.msnName == "Instant Build")
                    {
                        missionsPanel.GetComponent<missionParent>().completeMission(item);
                    }
                }
            }
        }
    }
}
