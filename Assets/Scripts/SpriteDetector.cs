using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteDetector : MonoBehaviour
{

    [SerializeField]
    private GameObject infoPanel, hqMenu;

    void Start()
    {
        addPhysics2DRaycaster();
        print("added raycaster");
    }

    void addPhysics2DRaycaster()
    {
        Physics2DRaycaster physicsRaycaster = GameObject.FindObjectOfType<Physics2DRaycaster>();
        if (physicsRaycaster == null)
        {
            print("adding raycaster");
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
        } else
        {
            infoPanel.SetActive(false);
            hqMenu.SetActive(false);
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
