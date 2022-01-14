using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectDecoInf : MonoBehaviour
{
    public float newVal;
    public List<Collider2D> decoInfluenced = new List<Collider2D>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    [ContextMenu("Show Influence")]
    public void detectInfluence()
    {
        print("doing test");
        Collider2D myCollider = gameObject.GetComponent<Collider2D>();
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Default"));
        int colliderCount = myCollider.OverlapCollider(contactFilter, colliders);
        decoInfluenced.Clear();

        //getting any deco that is within influence collider reach
        foreach (var item in colliders)
        {
            //print("item is " + item.name);
            if (item.GetComponent<Property>() != null)
            {
                if (item.GetComponent<Property>().Card.type == "Deco")
                {
                    print("found deco " + item.name);
                    item.transform.GetChild(0).GetComponent<influence>().detectInfluence(true);
                    foreach (var house in item.transform.GetChild(0).GetComponent<influence>().housesInfluenced)
                    {
                        //print("finalised item is " + item.name);
                        if (house.name == this.transform.parent.name)
                        {
                            decoInfluenced.Add(item);
                            print(item.name + " affects " + house.name + "giving it " + item.GetComponent<Property>().Card.decoBonus + "% bonus");
                            break;
                        }
                    }
                }
            }
        }
        //print("influencing " + housesInfluenced.Count);
        foreach (var item in decoInfluenced)
        {
            //print("finalised item is " + item.name);
            GameObject.Find(item.name).GetComponent<SpriteRenderer>().color = new Color(35f / 255f, 206f / 255f, 241f / 255f);
        }
    }

    public void removeHighlights()
    {
        foreach (var item in decoInfluenced)
        {
            //print("finalised item is " + item.name);
            GameObject.Find(item.name).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public List<Collider2D> returnHighlights()
    {
        detectInfluence();
        removeHighlights();
        return decoInfluenced;
    }
}
