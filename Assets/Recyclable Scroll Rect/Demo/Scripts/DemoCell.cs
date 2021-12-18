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
    public Image bgImage;
    public Button buyButton;
    public Button infoButton;
    public GameObject infoParent;
    public Image infoBG;
    public Sprite infoBg2;
    public Sprite infoBg3;
    public Sprite infoBg4;

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
        _cellIndex = cellIndex;
        _propertyCard = propertyCard;

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
                secondImg.sprite = time;
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
                firstText.text = propertyCard.decoBonus.ToString() + "%";
                secondText.text = propertyCard.space;
                thirdText.text = propertyCard.influence;
                fourthText.text = "";
                infoBG.sprite = infoBg3;
                break;
            case "Wonder":
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
            default: break;
        }
        bgImage.sprite = propertyCard.bgImage;

        stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<Statistics>();
        //print("in democell, level is " + stats.returnStats()[2]);
        if (propertyCard.level > stats.returnStats()[2])
        {
            lockParent.SetActive(true);
            levelReq.text = "Level " + propertyCard.level + " needed";
        } else
        {
            lockParent.SetActive(false);
        }
    }

    void buyListener()
    {
        print("Buying " + _propertyCard.propName + " which costs " + _propertyCard.cost);
        GameObject shopMenu = GameObject.FindGameObjectWithTag("ShopMenu");
        shopToggleScript script = GameObject.FindGameObjectWithTag("ShopToggle").GetComponent<shopToggleScript>();
        
        stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<Statistics>();
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
                script.offShopButton();
                shopMenu.SetActive(false);
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
                script.offShopButton();
                shopMenu.SetActive(false);
                shopMenu.GetComponent<PurchaseController>().purchaseProperty(_propertyCard);
                // --------------------- Swapping to green border grass -------------
                map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
                map.SwapTile(tileGrass, greenGrass);
                // ------------------------------------------------------------------
            }
        }

    }

    void infoListener()
    {
        if (infoParent.activeSelf == false)
        {
            infoParent.SetActive(true);
        }
        else if (infoParent.activeSelf == true)
        {   
            infoParent.SetActive(false);
        }
        print("Space: "+ _propertyCard.space+"\nNo of tenants: "+ _propertyCard.tenants + " Earnings in 3mins: $" + _propertyCard.threemins);
    }
}
