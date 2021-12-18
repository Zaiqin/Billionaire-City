using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Property : MonoBehaviour, IPointerClickHandler
{
    public PropertyCard Card;
    public void initialise(PropertyCard pcard)
    {
        // Adding propertySprite
        SpriteRenderer renderer = this.gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = pcard.propImage;
        renderer.sprite = Sprite.Create(pcard.propImage.texture, new Rect(0, 0, pcard.propImage.texture.width, pcard.propImage.texture.height), new Vector2(0f, 0f), 32);
        renderer.sortingOrder = 1;

        Card = pcard;
        this.name = pcard.displayName;
        this.gameObject.AddComponent<BoxCollider2D>();

        if (pcard.type == "House")
        {
            print("here here is house");
            GameObject contract = new GameObject();
            contract.name = "Contract";
            SpriteRenderer contractStarrenderer = contract.AddComponent<SpriteRenderer>();
            Sprite contractStarSprite = Resources.Load<Sprite>("contractStar");
            contractStarrenderer.sprite = Sprite.Create(contractStarSprite.texture, new Rect(0, 0, contractStarSprite.texture.width, contractStarSprite.texture.height), new Vector2(0.5f, 0.5f), 850);
            contract.AddComponent<scaleLerper>();
            contractStarrenderer.sortingOrder = 0; // Hides it
            contract.transform.parent = this.transform;
            contract.transform.localPosition = new Vector3(float.Parse(pcard.space.Substring(0, 1)) / 2, float.Parse(pcard.space.Substring(pcard.space.Length - 1)) / 2, 0f);

            contract.AddComponent<contractScript>();
            contract.GetComponent<contractScript>().propCard = pcard;
        } else
        {
            Debug.Log("this not a house");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("From property.cs clicked on" + this.name);
        GameObject propParent = GameObject.Find("Properties");
        GameObject infoPanel = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        if (this.transform.parent == propParent.transform)
        {
            infoPanel.SetActive(true);
            infoPanel.transform.GetChild(0).GetComponent<Text>().text = this.Card.displayName;
            GameObject hqmenu = GameObject.Find("HQStats");
            if (hqmenu != null)
            {
                hqmenu.SetActive(false);
            }
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