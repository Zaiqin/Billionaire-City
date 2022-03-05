using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Property : MonoBehaviour, IPointerClickHandler
{
    public PropertyCard Card;
    public int bonus;
    public string constructStart;
    public string constructEnd;
    public bool justConnected = false;
    private double nextUpdate = 0.1;
    public void initialise(PropertyCard pcard)
    {
        // Adding propertySprite
        SpriteRenderer renderer = this.gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = pcard.propImage;
        renderer.sprite = Sprite.Create(pcard.propImage.texture, new Rect(0, 0, pcard.propImage.texture.width, pcard.propImage.texture.height), new Vector2(0f, 0f), 32);
        renderer.sortingOrder = 2;

        Card = pcard;
        this.name = pcard.displayName;
        this.gameObject.AddComponent<BoxCollider2D>();

        float x = float.Parse(pcard.space.Substring(0, 1));
        float y = float.Parse(pcard.space.Substring(pcard.space.Length - 1));
        this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(x-(float)0.1, y-(float)0.1);
        this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(x / 2, y / 2);

        print("type is " + pcard.type);
        if (pcard.type == "House")
        {
            // ---------------------- Contract Object ---------------------
            GameObject contract = new GameObject();
            contract.name = "Contract";
            SpriteRenderer contractStarrenderer = contract.AddComponent<SpriteRenderer>();
            Sprite contractStarSprite = Resources.Load<Sprite>("contract");
            contractStarrenderer.sprite = Sprite.Create(contractStarSprite.texture, new Rect(0, 0, contractStarSprite.texture.width, contractStarSprite.texture.height), new Vector2(0.5f, 0.5f), 130);
            contract.AddComponent<scaleLerper>();
            contractStarrenderer.sortingOrder = 0; // Hides it
            contract.transform.parent = this.transform;
            contract.transform.localPosition = new Vector3(float.Parse(pcard.space.Substring(0, 1)) / 2, float.Parse(pcard.space.Substring(pcard.space.Length - 1)) / 2, 0f);
            contract.AddComponent<contractScript>();
            contract.GetComponent<contractScript>().propCard = pcard;

            // ----------------------- Money Object -----------------------------
            GameObject money = new GameObject();
            money.name = "Money";
            SpriteRenderer moneyrenderer = money.AddComponent<SpriteRenderer>();
            Sprite moneySprite = Resources.Load<Sprite>("moneyPickup");
            moneyrenderer.sprite = Sprite.Create(moneySprite.texture, new Rect(0, 0, moneySprite.texture.width, moneySprite.texture.height), new Vector2(0.5f, 0.5f), 130);
            money.AddComponent<scaleLerper>();
            moneyrenderer.sortingOrder = 0; // Hides it
            money.transform.parent = this.transform;
            money.transform.localPosition = new Vector3(float.Parse(pcard.space.Substring(0, 1)) / 2, float.Parse(pcard.space.Substring(pcard.space.Length - 1)) / 2, 0f);
            money.AddComponent<moneyPickupScript>();
            money.AddComponent<moneyPickupScript>().propCard = pcard;
            // --- Deco detection Influence Overlay -----------
            float maxReach = GameObject.Find("CSV").gameObject.GetComponent<CSVReader>().maxDecoReach;
            GameObject inf = new GameObject();
            inf.name = "Deco Influence";
            SpriteRenderer infRenderer = inf.AddComponent<SpriteRenderer>();
            Sprite infSprite = Resources.Load<Sprite>("blueInfluence");
            infRenderer.drawMode = SpriteDrawMode.Sliced;
            infRenderer.sprite = infSprite;
            infRenderer.size = new Vector2(((maxReach * 2) + int.Parse(pcard.space.Substring(0, 1))) - 0.1f, ((maxReach * 2) + int.Parse(pcard.space.Substring(pcard.space.Length - 1))) - 0.1f);
            //infRenderer.sprite = Sprite.Create(infSprite.texture, new Rect(0, 0, ((maxReach*2)+int.Parse(pcard.space.Substring(0, 1))) - 0.1f, ((maxReach * 2) + int.Parse(pcard.space.Substring(pcard.space.Length - 1))) - 0.1f), new Vector2(0.5f, 0.5f), 1);
            infRenderer.color = new Color(35f / 255f, 206f / 255f, 241f / 255f, 125f / 255f);
            infRenderer.sortingOrder = 0;
            inf.transform.parent = this.transform;
            inf.transform.localPosition = new Vector3(float.Parse(pcard.space.Substring(0, 1)) / 2, (float.Parse(pcard.space.Substring(pcard.space.Length - 1)) / 2) - 0.05f, 0f);
            inf.AddComponent<detectDecoInf>();
            inf.SetActive(false);
            // ----------------------- Road Disconnected Object -----------------------------
            GameObject discon = new GameObject();
            discon.name = "Disconnected";
            SpriteRenderer disconrenderer = discon.AddComponent<SpriteRenderer>();
            Sprite disconSprite = Resources.Load<Sprite>("disconnected");
            disconrenderer.sprite = Sprite.Create(disconSprite.texture, new Rect(0, 0, disconSprite.texture.width, disconSprite.texture.height), new Vector2(0.5f, 0.5f), 340);
            disconrenderer.sortingOrder = 0;
            discon.transform.parent = this.transform;
            discon.transform.localPosition = new Vector3(float.Parse(pcard.space.Substring(0, 1)) / 2, float.Parse(pcard.space.Substring(pcard.space.Length - 1)) / 2, 0f);
            discon.AddComponent<scaleLerper>();

        } else if (pcard.type == "Commerce")
        {
            // --- Influence Overlay -----------
            GameObject inf = new GameObject();
            inf.name = "Influence";
            SpriteRenderer infRenderer = inf.AddComponent<SpriteRenderer>();
            Sprite infSprite = Resources.Load<Sprite>("blueInfluence");
            infRenderer.drawMode = SpriteDrawMode.Sliced;
            infRenderer.sprite = infSprite;
            infRenderer.size = new Vector2(float.Parse(pcard.influence.Substring(0, 2)) - 0.2f, float.Parse(pcard.influence.Substring(pcard.influence.Length - 2)) - 0.2f);
            infRenderer.color = new Color(35f / 255f, 206f / 255f, 241f / 255f, 125f / 255f);
            infRenderer.sortingOrder = 2;
            inf.transform.parent = this.transform;
            inf.transform.localPosition = new Vector3(float.Parse(pcard.space.Substring(0, 1)) / 2, (float.Parse(pcard.space.Substring(pcard.space.Length - 1)) / 2) -0.05f, 0f);
            inf.AddComponent<influence>();
            // -------- Commerce Pickup -----------------
            GameObject commerce = new GameObject();
            commerce.name = "Money";
            SpriteRenderer commerceRenderer = commerce.AddComponent<SpriteRenderer>();
            Sprite commerceSprite;
            foreach (Sprite s in GameObject.Find("commercePickups").GetComponent<comPickups>().l)
            {
                if (s.name == pcard.propName + "Icon")
                {
                    commerceSprite = s;
                    commerceRenderer.sprite = Sprite.Create(commerceSprite.texture, new Rect(0, 0, commerceSprite.texture.width, commerceSprite.texture.height), new Vector2(0.5f, 0.5f), 45);
                    break;
                }
            }
            commerce.AddComponent<scaleLerper>();
            commerceRenderer.sortingOrder = 0;
            commerce.transform.parent = this.transform;
            commerce.transform.localPosition = new Vector3(float.Parse(pcard.space.Substring(0, 1)) / 2, float.Parse(pcard.space.Substring(pcard.space.Length - 1)) / 2, 0f);
            commerce.AddComponent<commercePickupScript>();
            commerce.AddComponent<commercePickupScript>().propCard = pcard;
            // ----------------------- Road Disconnected Object -----------------------------
            GameObject discon = new GameObject();
            discon.name = "Disconnected";
            SpriteRenderer disconrenderer = discon.AddComponent<SpriteRenderer>();
            Sprite disconSprite = Resources.Load<Sprite>("disconnected");
            disconrenderer.sprite = Sprite.Create(disconSprite.texture, new Rect(0, 0, disconSprite.texture.width, disconSprite.texture.height), new Vector2(0.5f, 0.5f), 340);
            disconrenderer.sortingOrder = 0;
            discon.transform.parent = this.transform;
            discon.transform.localPosition = new Vector3(float.Parse(pcard.space.Substring(0, 1)) / 2, float.Parse(pcard.space.Substring(pcard.space.Length - 1)) / 2, 0f);
            discon.AddComponent<scaleLerper>();
        } else if (pcard.type == "Deco")
        {
            // --- Influence Overlay -----------
            GameObject inf = new GameObject();
            inf.name = "Influence";
            SpriteRenderer infRenderer = inf.AddComponent<SpriteRenderer>();
            Sprite infSprite = Resources.Load<Sprite>("blueInfluence");
            infRenderer.drawMode = SpriteDrawMode.Sliced;
            infRenderer.sprite = infSprite;
            infRenderer.size = new Vector2(float.Parse(pcard.influence.Substring(0, 2)) - 0.2f, float.Parse(pcard.influence.Substring(pcard.influence.Length - 2)) - 0.2f);
            infRenderer.color = new Color(35f / 255f, 206f / 255f, 241f / 255f, 125f / 255f);
            infRenderer.sortingOrder = 2;
            inf.transform.parent = this.transform;
            inf.transform.localPosition = new Vector3(float.Parse(pcard.space.Substring(0, 1)) / 2, (float.Parse(pcard.space.Substring(pcard.space.Length - 1)) / 2) - 0.05f, 0f);
            inf.AddComponent<influence>();
        }
        else
        {
            //--------------------------- Wonder -------------------------
            // ----------------------- Road Disconnected Object -----------------------------
            GameObject discon = new GameObject();
            discon.name = "Disconnected";
            SpriteRenderer disconrenderer = discon.AddComponent<SpriteRenderer>();
            Sprite disconSprite = Resources.Load<Sprite>("disconnected");
            disconrenderer.sprite = Sprite.Create(disconSprite.texture, new Rect(0, 0, disconSprite.texture.width, disconSprite.texture.height), new Vector2(0.5f, 0.5f), 340);
            disconrenderer.sortingOrder = 0;
            discon.transform.parent = this.transform;
            discon.transform.localPosition = new Vector3(float.Parse(pcard.space.Substring(0, 1)) / 2, float.Parse(pcard.space.Substring(pcard.space.Length - 1)) / 2, 0f);
            discon.AddComponent<scaleLerper>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameObject.Find("Main Camera").GetComponent<CameraMovement>().dragging == false && GameObject.Find("Main Camera").GetComponent<SpriteDetector>().buttonDownLayer == 0)
        {
            print("From property.cs clicked on" + this.name);
            clickedPropertyFunc();
        }
    }

    public void clickedPropertyFunc()
    {
        GameObject propParent = GameObject.Find("Properties");
        GameObject infoPanel = GameObject.Find("Canvas").transform.GetChild(0).gameObject;

        if (GameObject.Find("deleteToggle").GetComponent<Toggle>().isOn == true)
        {
            print("show del popup");
            GameObject delPopup = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
            delPopup.SetActive(true);
            delPopup.transform.localScale = Vector2.zero;
            delPopup.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
            // Deducting money ---------------
            long refund;
            if (this.Card.cost.Contains("Gold"))
            {
                refund = (long)(21000 * double.Parse(this.Card.cost.Remove(this.Card.cost.Length - 5)));
                if (refund >= 100000000)
                {
                    string temp = refund.ToString("#,##0");
                    delPopup.transform.GetChild(4).GetComponent<Text>().text = "$" + temp.Substring(0, temp.Length - 8) + "M";
                }
                else
                {
                    delPopup.transform.GetChild(4).GetComponent<Text>().text = "$" + refund.ToString("#,##0");
                }
                print("refund convert from gold is " + refund);
            }
            else
            {
                refund = (long)(0.35 * double.Parse(this.Card.cost));
                if (refund >= 100000000)
                {
                    string temp = refund.ToString("#,##0");
                    delPopup.transform.GetChild(4).GetComponent<Text>().text = "$" + temp.Substring(0, temp.Length - 8) + "M";
                }
                else
                {
                    delPopup.transform.GetChild(4).GetComponent<Text>().text = "$" + refund.ToString("#,##0");
                }
                print("refund is " + refund);
            }
            // -------------------------------
            delPopup.transform.GetChild(2).GetComponent<delConfirm>().refundValue = refund;
            delPopup.transform.GetChild(2).GetComponent<delConfirm>().selProp = this.gameObject;
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        }
        if (this.Card.type == "Commerce" && GameObject.Find("moveContractorToggle").GetComponent<Toggle>().isOn == false && this.transform.parent.name == "Properties" && GameObject.Find("Canvas").GetComponent<toggleMaster>().checkAllOff() == true && this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder != 2)
        {
            print("2");
            GameObject influence = this.transform.GetChild(0).gameObject;
            influence.SetActive(true);
            influence.GetComponent<influence>().detectInfluence();
            GameObject.Find("Main Camera").GetComponent<SpriteDetector>().selectedCommerce = this.gameObject;
        }
        if (this.transform.parent == propParent.transform && GameObject.Find("Canvas").GetComponent<toggleMaster>().checkAllOff() == true && GameObject.Find("moveContractorToggle").GetComponent<Toggle>().isOn == false)
        {
            if ((this.Card.type == "House" && this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder != 2 && this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder != 2 && this.gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().sortingOrder != 2)
                || (this.Card.type == "Commerce" && this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder != 2)
                || (this.Card.type == "Deco")
                || (this.Card.type == "Wonder")
                || (this.GetComponent<Property>().constructEnd != null && this.GetComponent<Property>().constructEnd != "na"))
            {
                infoPanel.SetActive(true);
                infoPanel.transform.localScale = Vector2.zero;
                infoPanel.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
                infoPanel.GetComponent<infoScript>().selProp = this.gameObject;
                if (infoPanel.GetComponent<infoScript>().highlightedProp != null)
                {
                    if (infoPanel.GetComponent<infoScript>().highlightedProp.transform.childCount > 4 && infoPanel.GetComponent<infoScript>().highlightedProp.name != this.gameObject.name)
                    {
                        print("Destroy this");
                        Destroy(infoPanel.GetComponent<infoScript>().highlightedProp.transform.GetChild(4).gameObject);
                    }
                }
                infoPanel.GetComponent<infoScript>().initInfo();

                print("show info Panel");
                infoPanel.GetComponent<infoScript>().rightSide = true;
                infoPanel.transform.position = new Vector3(0.2f + this.gameObject.transform.position.x + (infoPanel.GetComponent<BoxCollider2D>().bounds.size.x / 2) + float.Parse(this.GetComponent<Property>().Card.space.Substring(0, 1)), this.gameObject.transform.position.y + (infoPanel.GetComponent<BoxCollider2D>().bounds.size.y / 4) + 1f, 0f);
                if (infoPanel.transform.position.x + (infoPanel.GetComponent<BoxCollider2D>().bounds.size.x / 2) + 0.6f > (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0))).x)
                {
                    print("show infoPanel on left side of house");
                    infoPanel.GetComponent<infoScript>().rightSide = false;
                    infoPanel.transform.position = new Vector3(-0.2f + this.gameObject.transform.position.x - (infoPanel.GetComponent<BoxCollider2D>().bounds.size.x / 2), this.gameObject.transform.position.y + (infoPanel.GetComponent<BoxCollider2D>().bounds.size.y / 4) + 1f, 0f);
                }
                print("info localpos is " + infoPanel.transform.localPosition);
                /*infoPanel.transform.localScale = Vector2.zero;
                infoPanel.transform.LeanScale(Vector2.one, 0.1f);*/
            }
            GameObject hqmenu = GameObject.Find("HQStats");
            if (hqmenu != null)
            {
                hqmenu.SetActive(false);
            }
        }
        if (this.Card.type == "House" && GameObject.Find("moveContractorToggle").GetComponent<Toggle>().isOn == false)
        {
            if (this.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder == 0 && this.gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().sortingOrder != 2 && this.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder == 0 && this.transform.childCount <= 4 && this.transform.parent.name == "Properties" && GameObject.Find("Canvas").GetComponent<toggleMaster>().checkAllOff() == true)
            {
                GameObject decoObj = Instantiate(Resources.Load<GameObject>("decoObj"), new Vector3(this.transform.position.x + (float.Parse(this.Card.space.Substring(0, 1))) / 2, this.transform.position.y, -5f), Quaternion.identity) as GameObject;
                decoObj.transform.parent = this.gameObject.transform;
                decoObj.transform.GetChild(1).GetComponent<TextMesh>().text = "+" + bonus.ToString() + "%";
            }
            if (this.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder == 2)
            {
                this.transform.GetChild(1).GetComponent<moneyPickupScript>().collectMoney();
            }
        }
        if (this.Card.type == "Deco" && this.transform.parent.name == "Properties" && GameObject.Find("Canvas").GetComponent<toggleMaster>().checkAllOff() == true && GameObject.Find("moveContractorToggle").GetComponent<Toggle>().isOn == false)
        {
            print("clicked in deco");
            GameObject influence = this.transform.GetChild(0).gameObject;
            influence.SetActive(true);
            influence.GetComponent<influence>().detectInfluence();
        }
        
    }

    private void Update()
    {
        if (Time.time >= nextUpdate)
        {
            //Debug.Log(Time.time + ">=" + nextUpdate);
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 0.1;
            // Call your fonction
            checkConstruction();

            if (this.justConnected == true)
            {
                if (this.Card.type == "House")
                {
                    if (this.gameObject.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sortingOrder == 0)
                    {
                        updateConnected();
                    }
                }
                else if (this.Card.type == "Commerce")
                {
                    if (this.gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingOrder == 0)
                    {
                        updateConnected();
                    }
                }
                else if (this.Card.type == "Wonder")
                {
                    if (this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder == 0)
                    {
                        updateConnected();
                    }
                }
            }
        }
    }

    public void checkConstruction()
    {
        // ------------- Check construction --------------
        if (constructEnd != null && constructEnd != "na" && this.gameObject.GetComponent<Property>().Card.type != "Deco")
        {
            if ((DateTime.Parse(constructEnd) - DateTime.Now) <= TimeSpan.Zero)
            {
                SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
                renderer.sprite = Card.propImage;
                renderer.sprite = Sprite.Create(Card.propImage.texture, new Rect(0, 0, Card.propImage.texture.width, Card.propImage.texture.height), new Vector2(0f, 0f), 32);
                this.gameObject.GetComponent<Property>().constructEnd = "na";
                this.gameObject.GetComponent<Property>().constructStart = "na";

                this.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
                this.transform.GetChild(1).gameObject.AddComponent<BoxCollider2D>();

                if (Card.type == "House")
                {
                    this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                }
                else if (Card.type == "Commerce" && this.gameObject.transform.parent.GetChild(1).GetComponent<commercePickupScript>().signTime == "notsigned")
                {
                    DateTime theTime;
                    theTime = DateTime.Now.AddMinutes(3);
                    print("signing property commerce again");
                    string datetime = theTime.ToString("yyyy/MM/dd HH:mm:ss");
                    this.gameObject.transform.parent.GetChild(1).GetComponent<commercePickupScript>().signTime = datetime;
                    this.gameObject.transform.parent.GetChild(1).GetComponent<commercePickupScript>().signCreationTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    print("sign time is " + datetime);
                }
            }
            else
            {
                SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
                float x = float.Parse(Card.space.Substring(0, 1));
                float y = float.Parse(Card.space.Substring(Card.space.Length - 1));
                string spritename = "constructSprites/construct" + (x / y).ToString();
                Sprite constructSprite = Resources.Load<Sprite>(spritename);
                renderer.sprite = constructSprite;
                //print("x is " + x + " y is " + y);
                float ppi = (constructSprite.texture.width) / (x); //print("ppi is " + ppi);
                renderer.sprite = Sprite.Create(constructSprite.texture, new Rect(0, 0, constructSprite.texture.width, constructSprite.texture.height), new Vector2(0f, 0f), ppi);
            }
        }
        // -----------------------------------------------------
    }

    public void updateConnected()
    {
        this.justConnected = false;

        Property pp = this.gameObject.GetComponent<Property>();
        if (pp.Card.type == "House") // check to only do these thats only for houses
        {
            var dateAndTimeVar = System.DateTime.Now;
            //print("going check contract for " + pp.name + "which is signtime " + pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime);
            if (pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime == "notsigned")
            {
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signCreationTime = "notsigned";
                print("notsigned");
            }
            else if (dateAndTimeVar >= DateTime.Parse(pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime))
            {
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                //print("sign over timea alre");
            }
            else if (dateAndTimeVar < DateTime.Parse(pp.transform.GetChild(0).gameObject.GetComponent<contractScript>().signTime))
            {
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            else
            {
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                //print("sign still ongoiing");
            }

            if (pp.constructEnd != "na")
            {
                pp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                print("not equals na");
            }

        }
        else if (pp.Card.type == "Commerce")
        {
            var dateAndTimeVar = System.DateTime.Now;
            //print("going check contract for " + pp.name + "which is signtime " + pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime);
            if (pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime == "notsigned")
            {
                //print("notsigned, thus signing now");
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

                DateTime theTime;
                theTime = DateTime.Now.AddMinutes(3);
                //print("signing property commerce again");
                string datetime = theTime.ToString("yyyy/MM/dd HH:mm:ss");
                pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime = datetime;
                pp.transform.GetChild(1).GetComponent<commercePickupScript>().signCreationTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                //print("sign time is " + datetime);

            }
            else if (dateAndTimeVar >= DateTime.Parse(pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime))
            {
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                //print("sign over time already");
            }
            else if (dateAndTimeVar < DateTime.Parse(pp.transform.GetChild(1).gameObject.GetComponent<commercePickupScript>().signTime))
            {
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            else
            {
                pp.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                //print("sign still ongoiing");
            }
        }
    }
}

public class contractScript : MonoBehaviour, IPointerClickHandler
{
    public string signTime = "notsigned";
    public int signIndex = -1;
    public string signCreationTime = "notsigned";
    public PropertyCard propCard;
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (GameObject.Find("Main Camera").GetComponent<CameraMovement>().dragging == false && GameObject.Find("Main Camera").GetComponent<SpriteDetector>().isMouseOverUI() == false && GameObject.Find("Canvas").GetComponent<toggleMaster>().checkAllOff() == true && this.transform.parent.GetComponent<Property>().constructEnd == "na"&& GameObject.Find("moveContractorToggle").GetComponent<Toggle>().isOn == false)
        {
            print("clicked on contract");
            GameObject canvas = GameObject.Find("Canvas");
            canvas.GetComponent<toggleMaster>().cover.gameObject.SetActive(true);
            GameObject contractController = GameObject.Find("Contract Scroll Controller");
            GameObject contractMenu = contractController.GetComponent<RecyclableScrollerContract>().contractParent;
            contractMenu.SetActive(true);
            contractMenu.transform.localScale = Vector2.zero;
            contractMenu.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
            contractMenu.transform.GetChild(3).gameObject.SetActive(false);
            GameObject infoPanel = canvas.transform.GetChild(0).gameObject;
            infoPanel.SetActive(false);
            contractController.GetComponent<RecyclableScrollerContract>().pCard = propCard;
            contractController.GetComponent<RecyclableScrollerContract>().selProp = this.transform.parent.gameObject;
            contractController.GetComponent<RecyclableScrollerContract>().contractor = false;
            contractController.GetComponent<RecyclableScrollerContract>().goCalc = false;
            contractController.GetComponent<RecyclableScrollerContract>().userReloadData();

            GameObject.Find("SignController").GetComponent<signController>().selProperty = this.gameObject.transform.parent.gameObject;

            if (GameObject.Find("Canvas").transform.GetChild(0).GetComponent<infoScript>().highlightedProp != null)
            {
                GameObject hProp = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<infoScript>().highlightedProp;
                hProp.GetComponent<SpriteRenderer>().material.color = Color.white;

                if (hProp.GetComponent<Property>().Card.type == "House" && hProp.transform.childCount > 4)
                {
                    Destroy(hProp.transform.GetChild(4).gameObject);
                }
            }
        }
        
    }

    private void Update()
    {
        var dateAndTimeVar = System.DateTime.Now;
        if (signTime != "notsigned")
        {
            if (dateAndTimeVar >= DateTime.Parse(signTime) && this.gameObject.transform.parent.GetChild(3).GetComponent<SpriteRenderer>().sortingOrder == 0)
            {
                this.gameObject.transform.parent.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
    }
}

public class moneyPickupScript : MonoBehaviour, IPointerClickHandler
{
    public PropertyCard propCard;

    public void collectMoney()
    {
        propCard = this.gameObject.transform.parent.GetComponent<Property>().Card;
        if (GameObject.Find("Main Camera").GetComponent<CameraMovement>().dragging == false && GameObject.Find("Main Camera").GetComponent<SpriteDetector>().isMouseOverUI() == false && GameObject.Find("Canvas").GetComponent<toggleMaster>().checkAllOff() == true && this.gameObject.transform.parent.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder == 2)
        {
            print("clicked on money");
            this.gameObject.transform.parent.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
            this.gameObject.transform.parent.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 0;

            // ---- Checking what houses it affects ---------
            this.gameObject.transform.parent.GetChild(2).gameObject.SetActive(true);
            this.gameObject.transform.parent.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
            List<Collider2D> infList = this.gameObject.transform.parent.GetChild(2).GetComponent<detectDecoInf>().returnHighlights();
            this.gameObject.transform.parent.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(35f / 255f, 206f / 255f, 241f / 255f, 125f / 255f);
            this.gameObject.transform.parent.GetChild(2).gameObject.SetActive(false);
            //------------------------------------------------

            //print("collecting index " + this.gameObject.transform.parent.GetChild(0).GetComponent<contractScript>().signIndex);
            int profit;
            long xp = 0;
            switch (this.gameObject.transform.parent.GetChild(0).GetComponent<contractScript>().signIndex)
            {
                case 0: profit = this.gameObject.transform.parent.GetComponent<Property>().Card.threemins; xp = this.gameObject.transform.parent.GetComponent<Property>().Card.xpthreemins; break;
                case 1: profit = this.gameObject.transform.parent.GetComponent<Property>().Card.thirtymins; xp = this.gameObject.transform.parent.GetComponent<Property>().Card.xpthirtymins; break;
                case 2: profit = this.gameObject.transform.parent.GetComponent<Property>().Card.onehour; xp = this.gameObject.transform.parent.GetComponent<Property>().Card.xponehour; break;
                case 3: profit = this.gameObject.transform.parent.GetComponent<Property>().Card.fourhours; xp = this.gameObject.transform.parent.GetComponent<Property>().Card.xpfourhours; break;
                case 4: profit = this.gameObject.transform.parent.GetComponent<Property>().Card.eighthours; xp = this.gameObject.transform.parent.GetComponent<Property>().Card.xpeighthours; break;
                case 5: profit = this.gameObject.transform.parent.GetComponent<Property>().Card.twelvehours; xp = this.gameObject.transform.parent.GetComponent<Property>().Card.xptwelvehours; break;
                case 6: profit = this.gameObject.transform.parent.GetComponent<Property>().Card.oneday; xp = this.gameObject.transform.parent.GetComponent<Property>().Card.xponeday; break;
                case 7: profit = this.gameObject.transform.parent.GetComponent<Property>().Card.twodays; xp = this.gameObject.transform.parent.GetComponent<Property>().Card.xptwodays; break;
                case 8: profit = this.gameObject.transform.parent.GetComponent<Property>().Card.threedays; xp = this.gameObject.transform.parent.GetComponent<Property>().Card.xpthreedays; break;
                default: profit = 0; break;
            }

            int totalDecoBonus = 0;
            foreach (Collider2D item in infList)
            {
                totalDecoBonus += GameObject.Find(item.name).GetComponent<Property>().Card.decoBonus;
            }
            print("decoBonus is " + totalDecoBonus);
            float percent = 1 + (((float)totalDecoBonus) / 100);
            long finalProfit = (long)((float)profit * percent);
            print("profit is " + profit + ", final profit is " + finalProfit);
            float wonderPercent = 1 + (((float)(GameObject.Find("Stats").GetComponent<Statistics>().wonderBonus)) / 100);
            finalProfit = (long)((float)finalProfit * wonderPercent);
            print("profit is " + profit + ", final profit is " + finalProfit);

            print("double rent chance is " + ((float)(GameObject.Find("Stats").GetComponent<Statistics>().doubleChance)) + "%");
            float m_dropChance = ((float)(GameObject.Find("Stats").GetComponent<Statistics>().doubleChance)) / 10f;
            if (UnityEngine.Random.Range(0f, 1f) <= m_dropChance)
            {
                print("Double rent!!");
                finalProfit = finalProfit * 2;
            }

            if (profit != 0)
            {
                GameObject.Find("Stats").GetComponent<Statistics>().updateStats(diffmoney: finalProfit, diffxp: xp);
                GameObject.Find("ExternalAudioPlayer").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/money"));

                GameObject value = Instantiate(Resources.Load<GameObject>("floatingParent"), new Vector3(this.gameObject.transform.parent.transform.position.x + (float.Parse(this.gameObject.transform.parent.GetComponent<Property>().Card.space.Substring(0, 1))) / 2, this.gameObject.transform.parent.transform.position.y + 3.4f, -5f), Quaternion.identity) as GameObject;
                value.transform.GetChild(0).GetComponent<TextMesh>().text = "+ $" + finalProfit;
                value.transform.GetChild(0).GetComponent<TextMesh>().color = new Color(168f / 255f, 255f / 255f, 4f / 255f);

                GameObject xpValue = Instantiate(Resources.Load<GameObject>("floatingParent"), new Vector3(this.gameObject.transform.parent.transform.position.x + (float.Parse(this.gameObject.transform.parent.GetComponent<Property>().Card.space.Substring(0, 1))) / 2, this.gameObject.transform.parent.transform.position.y + 2.8f, -5f), Quaternion.identity) as GameObject;
                xpValue.transform.GetChild(0).GetComponent<TextMesh>().text = "+ " + xp + "XP";
                xpValue.transform.GetChild(0).GetComponent<TextMesh>().color = Color.yellow;

                this.gameObject.transform.parent.GetChild(0).GetComponent<contractScript>().signTime = "notsigned";
                this.gameObject.transform.parent.GetChild(0).GetComponent<contractScript>().signCreationTime = "notsigned";
                this.gameObject.transform.parent.GetChild(0).GetComponent<contractScript>().signIndex = -1;
                GameObject.Find("SaveLoadSystem").GetComponent<saveloadsystem>().saveGame();
            }
            if (GameObject.Find("Canvas").transform.GetChild(0).GetComponent<infoScript>().highlightedProp != null)
            {
                GameObject hProp = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<infoScript>().highlightedProp;
                hProp.GetComponent<SpriteRenderer>().material.color = Color.white;

                if (hProp.GetComponent<Property>().Card.type == "House" && hProp.transform.childCount > 4)
                {
                    Destroy(hProp.transform.GetChild(4).gameObject);
                }
                if (GameObject.Find("Canvas").transform.GetChild(0).gameObject.activeSelf == true)
                {
                    GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(false);
                }
            }

            // ------------ Type 6 Missions -------------------------------------
            GameObject missionsPanel = GameObject.Find("Stats").GetComponent<Statistics>().missionsPanel;
            if (missionsPanel.GetComponent<missionParent>().missionList != null)
            {
                foreach (var item in missionsPanel.GetComponent<missionParent>().missionList)
                {
                    if (item.msnType == 6 && item.msnPending == false)
                    {
                        if (item.msnName == "Pimp the House I" && propCard.propName == "bungalow" && totalDecoBonus >= 12)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                        else if (item.msnName == "Pimp the House II" && propCard.propName == "bungalow" && totalDecoBonus >= 16)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                        else if (item.msnName == "Pimp the House III" && propCard.propName == "townhouse" && totalDecoBonus >= 30)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                        else if (item.msnName == "Pimp the House IV" && propCard.propName == "townhouse" && totalDecoBonus >= 60)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                        else if (item.msnName == "Pimp the Villa" && propCard.propName == "villa" && totalDecoBonus >= 90)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                        else if (item.msnName == "Pimp the Colonial" && propCard.propName == "colonialhouse" && totalDecoBonus >= 360)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                        else if (item.msnName == "Pimp the Chateau" && propCard.propName == "chateau" && totalDecoBonus >= 90)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                    }
                }
            }
        }
        
    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        collectMoney();
    }

    private void Update()
    {
        if (this.gameObject.transform.parent.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder == 2 && this.gameObject.transform.parent.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder == 0) //if contract is showing
        {
            this.gameObject.transform.parent.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 0; // hide money
        }
        else if (this.gameObject.transform.parent.GetChild(0).GetComponent<contractScript>().signCreationTime == "notsigned" && this.gameObject.transform.parent.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder == 0) //contract creation time is not set
        {
            this.gameObject.transform.parent.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 0; // hide money
        }
    }
}

public class commercePickupScript : MonoBehaviour, IPointerClickHandler
{
    public string signTime = "notsigned";
    public string signCreationTime = "notsigned";
    public PropertyCard propCard;
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (GameObject.Find("Main Camera").GetComponent<CameraMovement>().dragging == false && GameObject.Find("Main Camera").GetComponent<SpriteDetector>().isMouseOverUI() == false && propCard != null)
        {
            print("clicked on commerce");
            this.gameObject.transform.parent.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 0; //hide commerce collect

            // ---- Checking what houses it affects ---------
            this.gameObject.transform.parent.GetChild(0).gameObject.SetActive(true);
            this.gameObject.transform.parent.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
            List<Collider2D> infList = this.gameObject.transform.parent.GetChild(0).GetComponent<influence>().returnHighlights();
            this.gameObject.transform.parent.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(35f / 255f, 206f / 255f, 241f / 255f, 125f / 255f);
            this.gameObject.transform.parent.GetChild(0).gameObject.SetActive(false);
            //------------------------------------------------

            long finalIncome = 0;
            int finalTenants = 0;
            float percent = 1 + (((float)GameObject.Find("Stats").GetComponent<Statistics>().wonderCommerceBonus) / 10);
            foreach (Collider2D item in infList)
            {
                GameObject obj = GameObject.Find(item.name);
                if (obj.transform.GetChild(0).GetComponent<contractScript>().signTime != "notsigned")
                {
                    switch (obj.transform.GetChild(0).GetComponent<contractScript>().signIndex)
                    {
                        case 1: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 2) * propCard.rentPerTenant; break;
                        case 2: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 3) * propCard.rentPerTenant; break;
                        case 3: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 4) * propCard.rentPerTenant; break;
                        case 4: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 5) * propCard.rentPerTenant; break;
                        case 5: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 6) * propCard.rentPerTenant; break;
                        case 6: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 7) * propCard.rentPerTenant; break;
                        case 7: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 8) * propCard.rentPerTenant; break;
                        case 8: finalIncome += (long)(obj.GetComponent<Property>().Card.tenants * 9) * propCard.rentPerTenant; break;
                        default: finalIncome += (long)obj.GetComponent<Property>().Card.tenants * propCard.rentPerTenant; break;
                    }
                    
                    print("added " + (obj.GetComponent<Property>().Card.tenants* (obj.transform.GetChild(0).GetComponent<contractScript>().signIndex+1)) + "tenants from " + obj);
                    finalTenants += (obj.GetComponent<Property>().Card.tenants * (obj.transform.GetChild(0).GetComponent<contractScript>().signIndex + 1));
                    GameObject alue = Instantiate(Resources.Load<GameObject>("floatingParent"), new Vector3(obj.transform.GetChild(0).position.x, obj.transform.GetChild(0).position.y + 1.2f, obj.transform.GetChild(0).position.z), Quaternion.identity) as GameObject;
                    alue.transform.GetChild(0).GetComponent<TextMesh>().text = "+ $" + (long)((float)(obj.GetComponent<Property>().Card.tenants * (obj.transform.GetChild(0).GetComponent<contractScript>().signIndex + 1) * propCard.rentPerTenant) * percent);
                    alue.transform.GetChild(0).GetComponent<TextMesh>().color = new Color(168f / 255f, 255f / 255f, 4f / 255f);
                }
            }

            long finalProfit = (long)((float)finalIncome * percent);
            print("final profit is " + finalProfit + " with " + finalTenants + " tenants");

            GameObject.Find("Stats").GetComponent<Statistics>().updateStats(diffmoney: finalProfit);
            GameObject.Find("ExternalAudioPlayer").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/money"));

            DateTime theTime;
            theTime = DateTime.Now.AddMinutes(3);
            print("signing property commerce again");
            string datetime = theTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.gameObject.transform.parent.GetChild(1).GetComponent<commercePickupScript>().signTime = datetime;
            this.gameObject.transform.parent.GetChild(1).GetComponent<commercePickupScript>().signCreationTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            print("sign time is " + datetime);

            GameObject.Find("SaveLoadSystem").GetComponent<saveloadsystem>().saveGame();

            if (GameObject.Find("Canvas").transform.GetChild(0).GetComponent<infoScript>().highlightedProp != null)
            {
                GameObject hProp = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<infoScript>().highlightedProp;
                hProp.GetComponent<SpriteRenderer>().material.color = Color.white;

                if (hProp.GetComponent<Property>().Card.type == "House" && hProp.transform.childCount > 4)
                {
                    Destroy(hProp.transform.GetChild(4).gameObject);
                }
                if (GameObject.Find("Canvas").transform.GetChild(0).gameObject.activeSelf == true)
                {
                    GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(false);
                }
            }

            // ------------ Type 5 Missions -------------------------------------
            GameObject missionsPanel = GameObject.Find("Stats").GetComponent<Statistics>().missionsPanel;
            if (missionsPanel.GetComponent<missionParent>().missionList != null)
            {
                foreach (var item in missionsPanel.GetComponent<missionParent>().missionList)
                {
                    if (item.msnType == 5 && item.msnPending == false)
                    {
                        if (item.msnName == "Pizzalicious Clients I" && propCard.propName == "pizzeria" && finalTenants >= 3)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                        else if (item.msnName == "Pizzalicious Clients II" && propCard.propName == "pizzeria" && finalTenants >= 10)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                        else if (item.msnName == "Pizzalicious Clients III" && propCard.propName == "pizzeria" && finalTenants >= 16)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                        else if (item.msnName == "Coffeelicious Clients I" && propCard.propName == "coffeeshop" && finalTenants >= 12)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                        else if (item.msnName == "Coffeelicious Clients II" && propCard.propName == "coffeeshop" && finalTenants >= 24)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                        else if (item.msnName == "Florist's Clients" && propCard.propName == "flowershop" && finalTenants >= 30)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                        else if (item.msnName == "Bowling Alley Clients" && propCard.propName == "bowling" && finalTenants >= 243)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                        else if (item.msnName == "Pet Shop Clients" && propCard.propName == "petshop" && finalTenants >= 32)
                        {
                            missionsPanel.GetComponent<missionParent>().completeMission(item);
                        }
                    }
                }
            }
        }
    }

    private void Update()
    {
        var dateAndTimeVar = System.DateTime.Now;
        if (signTime != "notsigned")
        {
            //print("sign end time is " + signTime + ", sign created on " + signCreationTime);
            if (dateAndTimeVar >= DateTime.Parse(signTime) && this.gameObject.transform.parent.GetChild(2).GetComponent<SpriteRenderer>().sortingOrder == 0)
            {
                this.gameObject.transform.parent.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
        
    }
}