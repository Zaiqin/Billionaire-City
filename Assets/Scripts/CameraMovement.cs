using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{

    [SerializeField]
    private Camera cam;

	[SerializeField]
	private float zoomStep, minCamSize, maxCamSize;

    [SerializeField]
    private GameObject ShopMenu, hqMenu, infoPanel;

	private Vector3 dragOrigin;
    private bool startOnGrid;

    public bool dragging = false;

    private void Awake()
    {
        // Disable multi touch
        Input.multiTouchEnabled = false;
        dragging = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam.orthographicSize = Mathf.Clamp(6.0f, 1.0f, 10.0f);
        print("Zoomed out");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("set drag to false");
            dragging = false;
        }
        if (Input.GetMouseButton(0))
        {
            PanCamera(); //called when mouse button is pressed down and remains down
        }
        if (Input.GetMouseButtonUp(0))
        {
            startOnGrid = false;
            Invoke("setDrag", 0.01f);
        }
        //print("dragging is " + dragging);
    }

    void setDrag()
    {
        dragging = false;
    }

    private bool isMouseOverUI(Vector3 pos) //return true if mouse is over ui
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
                return true;
            }
        } else
        {
            //no collider detected
        }
        //print("returning false");
        return false;
    }

    private void PanCamera() {
        // save pos of mouse in wolrld space when drag starts (first time clicked}
    	if(Input.GetMouseButtonDown(0) && !isMouseOverUI(Input.mousePosition)) {
            //print("mouse is not under UI");
    		dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            startOnGrid = true;
        }
        // calc dist between drag origin and new pos if it is still held down
        Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0) && startOnGrid == true && ShopMenu.activeSelf == false && difference != Vector3.zero) {
            //print("origin " + dragOrigin + "newPosition " + cam.ScreenToWorldPoint(Input.mousePosition) + " =difference " + difference);
            // move the camera by that dist
            cam.transform.position += difference;
            hqMenu.SetActive(false);
            infoPanel.SetActive(false);
            dragging = true;
        }
    	
    }

    public void ZoomIn() {
    	float newSize = cam.orthographicSize - zoomStep;
    	cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
    }

    public void ZoomOut() {
    	float newSize = cam.orthographicSize + zoomStep;
    	cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
        
    }
}
