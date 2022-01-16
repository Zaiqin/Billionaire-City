using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class influence : MonoBehaviour, IPointerClickHandler
{

    public float newVal;
    public List<Collider2D> housesInfluenced = new List<Collider2D>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    [ContextMenu("Show Influence")]
    public void detectInfluence(bool invisible = false)
    {
        //print("doing test");
        Collider2D myCollider = gameObject.GetComponent<Collider2D>();
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Default"));
        int colliderCount = myCollider.OverlapCollider(contactFilter, colliders);
        housesInfluenced.Clear();

        foreach (var item in colliders)
        {
            //print("item is " + item.name);
            if (item.GetComponent<Property>() != null)
            {
                if (item.GetComponent<Property>().Card.type == "House")
                {
                    housesInfluenced.Add(item);
                }
            }
        }
        //print("influencing " + housesInfluenced.Count);
        if (invisible == false)
        {
            foreach (var item in housesInfluenced)
            {
                //print("finalised item is " + item.name);
                GameObject.Find(item.name).GetComponent<SpriteRenderer>().color = new Color(35f / 255f, 206f / 255f, 241f / 255f);
            }
        }
    }


    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (GameObject.Find("Main Camera").GetComponent<CameraMovement>().dragging == false && GameObject.Find("Main Camera").GetComponent<SpriteDetector>().isMouseOverUI() == false && this.transform.parent.transform.parent.name != "PendingProperty")
        {
            print("clicked on influence");
            foreach (var item in housesInfluenced)
            {
                //print("finalised item is " + item.name);
                GameObject.Find(item.name).GetComponent<SpriteRenderer>().color = Color.white;
            }
            this.gameObject.SetActive(false);
            this.transform.parent.GetComponent<SpriteRenderer>().material.color = Color.white;
            GameObject.Find("infoPanel").SetActive(false);
        }
    }

    public void removeHighlights()
    {
        foreach (var item in housesInfluenced)
        {
            //print("finalised item is " + item.name);
            GameObject.Find(item.name).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public List<Collider2D> returnHighlights(bool inv = false)
    {
        detectInfluence(inv);
        removeHighlights();
        return housesInfluenced;
    }
}
