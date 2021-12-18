using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class Property
{
    public UnityEngine.GameObject obj;

    public void init(PropertyCard pcard, Sprite contractStarSprite, string type)
    {
        obj = new GameObject();
        SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
        renderer.sprite = pcard.propImage;
        renderer.sprite = Sprite.Create(pcard.propImage.texture, new Rect(0, 0, pcard.propImage.texture.width, pcard.propImage.texture.height), new Vector2(0f, 0f), 32);
        renderer.sortingOrder = 1;

        obj.name = pcard.displayName;

        PropertyStats pstats = obj.AddComponent<PropertyStats>();
        pstats.number = 10;
        obj.AddComponent<BoxCollider2D>();

        if (type == "House")
        {
            Debug.Log("here here is hosue");
            GameObject contract = new GameObject();
            contract.name = "Contract";
            SpriteRenderer contractStarrenderer = contract.AddComponent<SpriteRenderer>();
            contractStarrenderer.sprite = Sprite.Create(contractStarSprite.texture, new Rect(0, 0, contractStarSprite.texture.width, contractStarSprite.texture.height), new Vector2(0.5f, 0.5f), 850);
            contract.AddComponent<scaleLerper>();
            contractStarrenderer.sortingOrder = 0; //hides it
            contract.transform.parent = obj.transform;
            contract.transform.localPosition = new Vector3(float.Parse(pcard.space.Substring(0, 1)) / 2, float.Parse(pcard.space.Substring(pcard.space.Length - 1)) / 2, 0f);

            contract.AddComponent<contractScript>();
            contract.GetComponent<contractScript>().propCard = pcard;
        } else
        {
            Debug.Log("this not a house");
        }
    }
}

public class contractScript : MonoBehaviour, IPointerClickHandler
{
    public int testNumber;
    public PropertyCard propCard;
    
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        print("clicked on contract");
        GameObject canvas = GameObject.Find("Canvas");
        GameObject contractMenu = canvas.transform.GetChild(canvas.transform.childCount - 1).gameObject;
        contractMenu.SetActive(true);
        GameObject contractController = GameObject.Find("Contract Scroll Controller");
        contractController.GetComponent<RecyclableScrollerContract>().pCard = propCard;
        contractController.GetComponent<RecyclableScrollerContract>().userReloadData();
        
    }
}

internal class PropertyStats : MonoBehaviour, IPointerClickHandler
{
    public int zPos;
    public int number;
    public PropertyCard pCard;
    public string propType;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        print("From property.cs clicked on" + this.name);
        GameObject propParent = GameObject.Find("Properties");
        GameObject infoPanel = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        if (this.transform.parent == propParent.transform)
        {
            infoPanel.SetActive(true);
            infoPanel.transform.GetChild(0).GetComponent<Text>().text = pCard.displayName;
            GameObject hqmenu = GameObject.Find("HQStats");
            if (hqmenu != null)
            {
                hqmenu.SetActive(false);
            }
        }
    }
}

/*
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

internal class Property
{
    public GameObject obj;

    public void init(PropertyCard pcard)
    {
        obj = new GameObject();
        SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
        renderer.sprite = pcard.propImage;
        renderer.sprite = Sprite.Create(pcard.propImage.texture, new Rect(0, 0, pcard.propImage.texture.width, pcard.propImage.texture.height), new Vector2(0f, 0f), 32);
        renderer.sortingOrder = 2;

        obj.name = pcard.displayName;

        PropertyStats pstats = obj.AddComponent<PropertyStats>();
        pstats.number = 10;
        obj.AddComponent<BoxCollider2D>();
    }
}

internal class PropertyStats : MonoBehaviour, IPointerClickHandler
{
    public int zPos;
    public int number;
    public PropertyCard pCard;
    public string propType;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        print("clicke dme mememememe " + this.name);
    }
}


 * */