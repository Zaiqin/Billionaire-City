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
