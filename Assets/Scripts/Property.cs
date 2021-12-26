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

        float x = float.Parse(pcard.space.Substring(0, 1));
        float y = float.Parse(pcard.space.Substring(pcard.space.Length -1));
        this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(x, y);
        this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(x/2, y/2);

/*        if (pcard.type == "House")
        {
            GameObject contract = new GameObject();
            contract.name = "Contract";
            SpriteRenderer contractStarrenderer = contract.AddComponent<SpriteRenderer>();
            Sprite contractStarSprite = Resources.Load<Sprite>("contractStar");
            contractStarrenderer.sprite = Sprite.Create(contractStarSprite.texture, new Rect(0, 0, contractStarSprite.texture.width, contractStarSprite.texture.height), new Vector2(0.5f, 0.5f), 980);
            contract.AddComponent<scaleLerper>();
            contractStarrenderer.sortingOrder = 0; // Hides it
            contract.transform.parent = this.transform;
            contract.transform.localPosition = new Vector3(float.Parse(pcard.space.Substring(0, 1)) / 2, float.Parse(pcard.space.Substring(pcard.space.Length - 1)) / 2, 0f);

            contract.AddComponent<contractScript>();
            contract.GetComponent<contractScript>().propCard = pcard;
        } else
        {
            Debug.Log("this not a house");
        }*/
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("From property.cs clicked on" + this.name);
        GameObject propParent = GameObject.Find("Properties");
        GameObject infoPanel = GameObject.Find("Canvas").transform.GetChild(0).gameObject;

        if (GameObject.Find("deleteToggle").GetComponent<Toggle>().isOn == true)
        {
            print("show del popup");
            GameObject delPopup = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
            delPopup.SetActive(true);
            // Deducting money ---------------
            long refund;
            if (this.Card.cost.Contains("Gold"))
            {
                refund = (long)(21000 * double.Parse(this.Card.cost.Remove(this.Card.cost.Length - 5)));
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
                refund = (long)(0.35 * double.Parse(this.Card.cost));
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
            delPopup.transform.GetChild(2).GetComponent<delConfirm>().selProp = this.gameObject;
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        } else if (this.transform.parent == propParent.transform)
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