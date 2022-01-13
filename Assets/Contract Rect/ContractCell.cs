using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using System;

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
    public void ConfigureCell(Sprite bgSprite, PropertyCard pCard, int cellIndex)
    {
        _cellIndex = cellIndex;
        bgImage.sprite = bgSprite;
        xpText.text = pCard.XP + " XP";

        switch (cellIndex)
        {
            case 0:
                incomeText.text = "$" + pCard.threemins.ToString("#,##0");
                tenantsText.text = pCard.tenants.ToString();
                break;
            case 1:
                incomeText.text = "$" + pCard.thirtymins.ToString("#,##0");
                tenantsText.text = (pCard.tenants*2).ToString();
                break;
            case 2:
                incomeText.text = "$" + pCard.onehour.ToString("#,##0");
                tenantsText.text = (pCard.tenants * 3).ToString();
                break;
            case 3:
                incomeText.text = "$" + pCard.fourhours.ToString("#,##0");
                tenantsText.text = (pCard.tenants * 4).ToString();
                break;
            case 4:
                incomeText.text = "$" + pCard.eighthours.ToString("#,##0");
                tenantsText.text = (pCard.tenants * 5).ToString();
                break;
            case 5:
                incomeText.text = "$" + pCard.twelvehours.ToString("#,##0");
                tenantsText.text = (pCard.tenants * 6).ToString();
                break;
            case 6:
                incomeText.text = "$" + pCard.oneday.ToString("#,##0");
                tenantsText.text = (pCard.tenants * 7).ToString();
                break;
            case 7:
                incomeText.text = "$" + pCard.twodays.ToString("#,##0");
                tenantsText.text = (pCard.tenants * 8).ToString();
                break;
            case 8:
                incomeText.text = "$" + pCard.threedays.ToString("#,##0");
                tenantsText.text = (pCard.tenants * 9).ToString();
                break;
            default:
                incomeText.text = "$" + pCard.threemins.ToString("#,##0");
                tenantsText.text = pCard.tenants.ToString();
                break;
        }
        

    }

}
