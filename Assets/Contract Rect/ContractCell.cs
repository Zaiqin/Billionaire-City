using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using System;
using System.Collections.Generic;

//Cell class for demo. A cell in Recyclable Scroll Rect must have a cell class inheriting from ICell.
//The class is required to configure the cell(updating UI elements etc) according to the data during recycling of cells.
//The configuration of a cell is done through the DataSource SetCellData method.
//Check RecyclableScrollerDemo class
public class ContractCell : MonoBehaviour, ICell
{
    //UI
    public Image bgImage;
    public Button signButton;
    public Text incomeText;
    public Text xpText;
    public Text tenantsText;

    [SerializeField]
    private AudioClip touchSound;

    //Model
    private int _cellIndex;

    private void Start()
    {
        signButton.GetComponent<Button>().onClick.AddListener(signListener);
    }

    private void signListener()
    {
        print("trying to sign contract");
        GameObject.Find("SignController").GetComponent<signController>().signer(_cellIndex);
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(Sprite bgSprite, PropertyCard pCard, int cellIndex, GameObject selProp)
    {
        _cellIndex = cellIndex;
        bgImage.sprite = bgSprite;

        int tempIncome = 0;
        switch (cellIndex)
        {
            case 0:
                tempIncome = pCard.threemins;
                tenantsText.text = pCard.tenants.ToString();
                xpText.text = pCard.xpthreemins + " XP";
                break;
            case 1:
                tempIncome = pCard.thirtymins;
                tenantsText.text = (pCard.tenants*2).ToString();
                xpText.text = pCard.xpthirtymins + " XP";
                break;
            case 2:
                tempIncome = pCard.onehour;
                tenantsText.text = (pCard.tenants * 3).ToString();
                xpText.text = pCard.xponehour + " XP";
                break;
            case 3:
                tempIncome = pCard.fourhours;
                tenantsText.text = (pCard.tenants * 4).ToString();
                xpText.text = pCard.xpfourhours + " XP";
                break;
            case 4:
                tempIncome = pCard.eighthours;
                tenantsText.text = (pCard.tenants * 5).ToString();
                xpText.text = pCard.xpeighthours + " XP";
                break;
            case 5:
                tempIncome = pCard.twelvehours;
                tenantsText.text = (pCard.tenants * 6).ToString();
                xpText.text = pCard.xptwelvehours + " XP";
                break;
            case 6:
                tempIncome = pCard.oneday;
                tenantsText.text = (pCard.tenants * 7).ToString();
                xpText.text = pCard.xponeday + " XP";
                break;
            case 7:
                tempIncome = pCard.twodays;
                tenantsText.text = (pCard.tenants * 8).ToString();
                xpText.text = pCard.xptwodays + " XP";
                break;
            case 8:
                tempIncome = pCard.threedays;
                tenantsText.text = (pCard.tenants * 9).ToString();
                xpText.text = pCard.xpthreedays + " XP";
                break;
            default:
                tempIncome = pCard.threemins;
                tenantsText.text = pCard.tenants.ToString();
                xpText.text = pCard.XP + " XP";
                break;
        }

        // ---- Checking what houses it affects ---------
        selProp.transform.GetChild(2).gameObject.SetActive(true);
        selProp.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
        List<Collider2D> infList = selProp.transform.GetChild(2).GetComponent<detectDecoInf>().returnHighlights();
        selProp.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(35f / 255f, 206f / 255f, 241f / 255f, 125f / 255f);
        selProp.transform.GetChild(2).gameObject.SetActive(false);
        //------------------------------------------------

        int totalDecoBonus = 0;
        foreach (Collider2D item in infList)
        {
            totalDecoBonus += GameObject.Find(item.name).GetComponent<Property>().Card.decoBonus;
        }
        float percent = 1 + (((float)totalDecoBonus) / 100);
        long finalProfit = (long)((float)tempIncome * percent);
        float wonderPercent = 1 + (((float)(GameObject.Find("Stats").GetComponent<Statistics>().wonderBonus)) / 100);
        finalProfit = (long)((float)finalProfit * wonderPercent);
        print("profit is " + tempIncome + ", final profit is " + finalProfit);

        incomeText.text = "$" + finalProfit.ToString("#,##0");

    }

}
