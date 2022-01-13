using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpriteDetector : MonoBehaviour
{

    [SerializeField]
    private GameObject infoPanel, hqMenu;

    public GameObject selectedCommerce;

    public Toggle deleteToggle;

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

    void CastRay()
    {
        print("called cast ray");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            print("Spritedetector hit object: " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.name != "Influence" && selectedCommerce != null && hit.collider.gameObject.name != "infoPanel")
            {
                selectedCommerce.transform.GetChild(0).gameObject.SetActive(false);
                selectedCommerce.transform.GetChild(0).GetComponent<influence>().removeHighlights();
            }

            if ((hit.collider.gameObject.name == "Money" || hit.collider.gameObject.name == "Contract") && deleteToggle.isOn == true)
            {
                print("showing del popup pressed on contract");
                GameObject delPopup = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
                delPopup.SetActive(true);
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
        } else
        {
            infoPanel.SetActive(false);
            hqMenu.SetActive(false);
            if (selectedCommerce != null)
            {
                selectedCommerce.transform.GetChild(0).gameObject.SetActive(false);
                selectedCommerce.transform.GetChild(0).GetComponent<influence>().removeHighlights();
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
            if (hit.collider.gameObject.layer == 6)
            {
                //print("return true for layer 6");
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
        //print("updateing");
        if (Input.GetMouseButtonDown(0))
        {
            print("mouse down");
            CastRay();
        }
    }

        //Implement Other Events from Method 1
}
