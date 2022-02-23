using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpriteDetector : MonoBehaviour
{

    [SerializeField]
    private GameObject infoPanel, hqMenu;

    public GameObject selectedCommerce, dailyPanel, cover;

    public Toggle deleteToggle, storageToggle;

    bool dailyPressed;
    public int buttonDownLayer;

    void Start()
    {
        addPhysics2DRaycaster();
        //print("added raycaster");
    }

    void addPhysics2DRaycaster()
    {
        Physics2DRaycaster physicsRaycaster = GameObject.FindObjectOfType<Physics2DRaycaster>();
        if (physicsRaycaster == null)
        {
            //print("adding raycaster");
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }

    void castDownRay()
    {
        print("Cast down ray");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            buttonDownLayer = hit.collider.gameObject.layer;
            print("layer " + buttonDownLayer + "for " + hit.collider.gameObject.name);

            RaycastHit2D[] hits;
            hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hita = hits[i];
                print("raycastedasff all hit object: " + hita.collider.gameObject.name);
                if (hita.collider.gameObject.layer == 6)
                {
                    buttonDownLayer = 6;
                }
                if (hita.collider.gameObject.layer == 5)
                {
                    buttonDownLayer = 5;
                }
            }
        }
    }

    void CastRay()
    {
        //print("called cast ray");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        RaycastHit2D[] hits;
        hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);

        bool hitProp = false;
        if (buttonDownLayer == 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hita = hits[i];
                print("raycast all hit object: " + hita.collider.gameObject.name);
                if (hita.collider != null)
                {
                    if (hita.collider.GetComponent<Property>() != null)
                    {
                        hitProp = true;
                        hit = hita;
                        if (infoPanel.GetComponent<infoScript>().highlightedProp != null)
                        {
                            if ((hit.collider.GetComponent<Property>().Card.type == "Commerce" && selectedCommerce != null && hit.collider.gameObject.name != selectedCommerce.name) || infoPanel.GetComponent<infoScript>().highlightedProp.name != hit.collider.gameObject.name)
                            {
                                print("selected a diff property");
                                if (selectedCommerce != null)
                                {
                                    selectedCommerce.GetComponent<SpriteRenderer>().material.color = Color.white;
                                    selectedCommerce.transform.GetChild(0).gameObject.SetActive(false);
                                    selectedCommerce.transform.GetChild(0).GetComponent<influence>().removeHighlights();
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
                        break;
                    }
                }
            }
        }

        if (hitProp == false)
        {
            print("did not hit any property");
            if (selectedCommerce != null)
            {
                selectedCommerce.GetComponent<SpriteRenderer>().material.color = Color.white;
                selectedCommerce.transform.GetChild(0).gameObject.SetActive(false);
                selectedCommerce.transform.GetChild(0).GetComponent<influence>().removeHighlights();
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
            infoPanel.SetActive(false);
        }

        if (hitProp == true && buttonDownLayer == 0)
        {
            print("hit a property in spritedetector");

            if (hit.collider.GetComponent<Property>().Card.type == "Commerce")
            {
                var diff = DateTime.Parse(hit.collider.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime) - System.DateTime.Now;

                if (diff.TotalSeconds < 179)
                {
                    hit.collider.gameObject.GetComponent<Property>().clickedPropertyFunc();
                }
            } else {
                hit.collider.gameObject.GetComponent<Property>().clickedPropertyFunc();
            }

            if ((hit.collider.gameObject.name == "Money" || hit.collider.gameObject.name == "Contract") && deleteToggle.isOn == true)
            {
                if (buttonDownLayer == 0)
                {
                    print("showing del popup pressed on contract");
                    GameObject delPopup = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
                    delPopup.SetActive(true);
                    delPopup.transform.localScale = Vector2.zero;
                    delPopup.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
                    PropertyCard Card = hit.collider.gameObject.transform.parent.GetComponent<Property>().Card;
                    // Deducting money ---------------
                    long refund;
                    if (Card.cost.Contains("Gold"))
                    {
                        refund = (long)(21000 * double.Parse(Card.cost.Remove(Card.cost.Length - 5)));
                        if (refund >= 100000000)
                        {
                            string temp = refund.ToString("#,##0");
                            delPopup.transform.GetChild(1).GetComponent<Text>().text = "$" + temp.Substring(0, temp.Length - 8) + "M";
                        }
                        else
                        {
                            delPopup.transform.GetChild(1).GetComponent<Text>().text = "$" + refund.ToString("#,##0");
                        }
                        print("refund convert from gold is " + refund);
                    }
                    else
                    {
                        refund = (long)(0.35 * double.Parse(Card.cost));
                        if (refund >= 100000000)
                        {
                            string temp = refund.ToString("#,##0");
                            delPopup.transform.GetChild(1).GetComponent<Text>().text = "$" + temp.Substring(0, temp.Length - 8) + "M";
                        }
                        else
                        {
                            delPopup.transform.GetChild(1).GetComponent<Text>().text = "$" + refund.ToString("#,##0");
                        }
                        print("refund is " + refund);
                    }
                    // -------------------------------
                    delPopup.transform.GetChild(2).GetComponent<delConfirm>().refundValue = refund;
                    delPopup.transform.GetChild(2).GetComponent<delConfirm>().selProp = hit.collider.gameObject.transform.parent.gameObject;
                    hit.collider.gameObject.transform.parent.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }
    }
            

    public bool isMouseOverUI() //return true if mouse is over ui
    {
        //print("ismosueoverui result is " + EventSystem.current.IsPointerOverGameObject());

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<Draggable>() != null)
            {
                //print("mouseoverui hit building named " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.GetComponent<Draggable>().dragEnabled == true)
                {
                    //print("hit building that is draggable");
                    return true;
                }
                else
                {
                    //print("hit building that is draggable");
                }
            }
            if (hit.collider.gameObject.layer == 6 || hit.collider.gameObject.layer == 5)
            {
                print("return true for layer 6 or 5" + hit.collider.gameObject.layer);
                return true;
            }
        }
        else
        {
            //no collider detected
        }
        //print("returning false");
        return false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (dailyPanel.activeSelf == true)
            {
                dailyPressed = true;
            } else
            {
                dailyPressed = false;
            }
            castDownRay();
        }

        if (Input.GetMouseButtonUp(0) && cover.activeSelf == false && storageToggle.isOn == false)
        {
            print("dragging is " + this.GetComponent<CameraMovement>().dragging + " and daily pressed is " + dailyPressed);
            if (this.GetComponent<CameraMovement>().dragging == false && dailyPressed == false)
            {
                //print("mouse down");
                CastRay();
            }
        }
    }

        //Implement Other Events from Method 1
}
