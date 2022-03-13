using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using UnityEngine.Tilemaps;
using static CSVReader;

//Cell class for demo. A cell in Recyclable Scroll Rect must have a cell class inheriting from ICell.
//The class is required to configure the cell(updating UI elements etc) according to the data during recycling of cells.
//The configuration of a cell is done through the DataSource SetCellData method.
//Check RecyclableScrollerDemo class
public class DemoCell : MonoBehaviour, ICell
{
    //UI
    public Image bgImage, infoBG, propertyImage;
    public Sprite buyButtonFadedSprite, buyButtonSprite, infoBg2, infoBg3, infoBg4, money, gold, cardBase;
    public Button buyButton, infoButton;
    public GameObject infoParent, limited, priceIcon;
    public Text cardName, cardXP, cardCost;

    //Info Panel
    public Image firstImg;
    public Text firstText;
    public Image secondImg;
    public Text secondText;
    public Image thirdImg;
    public Text thirdText;
    public Image fourthImg;
    public Text fourthText;
    //Info resources
    public Sprite radius;
    public Sprite buildTime;
    public Sprite size;
    public Sprite bonus;
    public Sprite income;
    public Sprite time;

    //lock
    public GameObject lockParent;
    public Text levelReq;

    //Model
    private int _cellIndex;

    private PropertyCard _propertyCard;
    public ShopInsuff shopInsuffController;
    public Statistics stats;

    public Tilemap map;
    public TileBase greenGrass;
    public TileBase tileGrass;

    private void Start()
    {
        buyButton.GetComponent<Button>().onClick.AddListener(buyListener);
        infoButton.GetComponent<Button>().onClick.AddListener(infoListener);

        shopInsuffController = GameObject.FindGameObjectWithTag("ShopInsuff").GetComponent<ShopInsuff>();
        shopInsuffController.turnOffInsuff();
        
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(PropertyCard propertyCard, int cellIndex, string propertyType)
    {
        stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<Statistics>();

        _cellIndex = cellIndex;
        _propertyCard = propertyCard;
        bgImage.sprite = cardBase;
        cardName.text = propertyCard.displayName;
        if (propertyCard.displayName.Length > 22)
        {
            cardName.fontSize = 20;
        } else if (propertyCard.displayName.Length > 15)
        {
            cardName.fontSize = 24;
        } else
        {
            cardName.fontSize = 26;
        }
        cardXP.text = propertyCard.XP.ToString("#,##0") + " XP";
        propertyImage.sprite = propertyCard.propImage;
        if (propertyCard.cost.Contains("Gold"))
        {
            int amt = int.Parse(propertyCard.cost.Remove(propertyCard.cost.Length - 5));
            cardCost.text = amt.ToString("#,##0") + " Gold";
            priceIcon.GetComponent<Image>().sprite = gold;
        }
        else
        {
            priceIcon.GetComponent<Image>().sprite = money;
            if (long.Parse(propertyCard.cost) >= 10000000)
            {
                string temp = long.Parse(propertyCard.cost).ToString("#,##0");
                cardCost.text = "$" + temp.Substring(0, temp.Length - 8) + "M";
            }
            else
            {
                cardCost.text = "$" + long.Parse(propertyCard.cost).ToString("#,##0");
            }
        }

        switch (propertyType)
        {
            case "House":
                firstImg.sprite = buildTime;
                firstText.text = propertyCard.buildTime;
                secondImg.sprite = size;
                secondText.text = propertyCard.space;
                thirdImg.enabled = false;
                thirdText.text = "";
                fourthImg.enabled = false;
                fourthText.text = "";
                infoBG.sprite = infoBg2;
                break;
            case "Commerce":
                firstImg.sprite = income;
                secondImg.sprite = buildTime;
                thirdImg.sprite = radius;
                fourthImg.sprite = size;
                firstText.text= "$" + propertyCard.rentPerTenant.ToString();
                secondText.text = propertyCard.buildTime;
                thirdText.text= propertyCard.influence;
                fourthText.text = propertyCard.space;
                infoBG.sprite = infoBg4;
                break;
            case "Deco":
                firstImg.sprite = bonus;
                secondImg.sprite = size;
                thirdImg.sprite = radius;
                fourthImg.enabled = false;
                firstText.text = propertyCard.decoBonus.ToString() + "% Bonus";
                secondText.text = propertyCard.space;
                thirdText.text = propertyCard.influence;
                fourthText.text = "";
                infoBG.sprite = infoBg3;
                break;
            case "Wonder":
                buyButton.interactable = true;
                buyButton.image.sprite = buyButtonSprite;
                foreach (var item in stats.GetComponent<Statistics>().builtWonders)
                {
                    if (item == propertyCard.displayName)
                    {
                        //print("setting " + propertyCard.displayName + "as faded");
                        buyButton.interactable = false;
                        buyButton.image.sprite = buyButtonFadedSprite;
                        break;
                    }
                }
                firstImg.sprite = buildTime;
                firstText.text = propertyCard.buildTime;
                secondImg.sprite = size;
                secondText.text = propertyCard.space;
                thirdImg.sprite = bonus;
                if (propertyCard.wonderBonus >= 100)
                {
                    thirdText.text = (((float)propertyCard.wonderBonus) / 100).ToString() + "% Rent x2";
                }
                else if (propertyCard.wonderBonus > 10)
                {
                    thirdText.text = (((float)propertyCard.wonderBonus) / 10).ToString() + "% Commerce";
                }
                else
                {
                    thirdText.text = propertyCard.wonderBonus.ToString() + "% Houses";
                }
                fourthImg.enabled = false;
                fourthText.text = "";
                infoBG.sprite = infoBg3;
                break;
            default: break;
        }

        //print("in democell, level is " + stats.returnStats()[2]);
        if (propertyCard.level > stats.returnStats()[2])
        {
            lockParent.SetActive(true);
            levelReq.text = "Level " + propertyCard.level + " needed";
        } else
        {
            lockParent.SetActive(false);
        }
        if (propertyCard.limited == "YES")
        {
            limited.SetActive(true);
        } else
        {
            limited.SetActive(false);
        }
    }

    public void closePanel()
    {
        GameObject ShopMenu = GameObject.FindGameObjectWithTag("ShopMenu");
        ShopMenu.transform.LeanScale(Vector2.zero, 0.2f).setEaseInBack();
        Invoke("setInactive", 0.2f);
    }

    void setInactive()
    {
        GameObject ShopMenu = GameObject.FindGameObjectWithTag("ShopMenu");
        ShopMenu.SetActive(false);
    }

    void buyListener()
    {
        print("Buying " + _propertyCard.propName + " which costs " + _propertyCard.cost);
        GameObject shopMenu = GameObject.FindGameObjectWithTag("ShopMenu");

        closePanel();
        shopMenu.GetComponent<PurchaseController>().purchaseProperty(_propertyCard);
        // --------------------- Swapping to green border grass -------------
        map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        map.SwapTile(tileGrass, greenGrass);
        // ------------------------------------------------------------------
        /*stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<Statistics>();
        print("money now is " + stats.returnStats()[0]);

        int deduction = 0;
        if (_propertyCard.cost.Contains("Gold"))
        {
            deduction = int.Parse(_propertyCard.cost.Remove(_propertyCard.cost.Length - 5));
            print("deducting " + deduction + " gold");
            if (stats.returnStats()[1] < deduction)
            {
                print("insufficient gold");
                shopInsuffController.turnOnInsuff(type: "gold");
            }
            else
            {
                print("enough gold");
                closePanel();
                shopMenu.GetComponent<PurchaseController>().purchaseProperty(_propertyCard);
                // --------------------- Swapping to green border grass -------------
                map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
                map.SwapTile(tileGrass, greenGrass);
                // ------------------------------------------------------------------

            }
        } else
        {
            deduction = int.Parse(_propertyCard.cost);
            print("deducting $" + deduction);
            if (stats.returnStats()[0] < deduction)
            {
                print("insufficient money");
                shopInsuffController.turnOnInsuff(type: "money");
            }
            else
            {
                print("enough money");
                closePanel();
                shopMenu.GetComponent<PurchaseController>().purchaseProperty(_propertyCard);
                // --------------------- Swapping to green border grass -------------
                map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
                map.SwapTile(tileGrass, greenGrass);
                // ------------------------------------------------------------------
            }
        }*/

    }

    void infoListener()
    {
        if (infoParent.activeSelf == false)
        {
            infoParent.SetActive(true);
            infoParent.transform.localScale = Vector2.zero;
            infoParent.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
        }
        else if (infoParent.activeSelf == true)
        {
            closeinfoPanel();
        }
        print("Space: "+ _propertyCard.space+"\nNo of tenants: "+ _propertyCard.tenants + " Earnings in 3mins: $" + _propertyCard.threemins);
    }

    public void closeinfoPanel()
    {
        infoParent.transform.LeanScale(Vector2.zero, 0.2f).setEaseInBack();
        Invoke("setinfoInactive", 0.2f);
    }

    void setinfoInactive()
    {
        infoParent.gameObject.SetActive(false);
        infoParent.gameObject.transform.localScale = Vector2.one;
    }
}
