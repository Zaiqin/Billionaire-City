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
    public Text costText;

    [SerializeField]
    private AudioClip touchSound;

    public bool contractor;

    //Model
    private int _cellIndex;

    private void Start()
    {
        signButton.GetComponent<Button>().onClick.AddListener(signListener);
    }

    private void signListener()
    {
        print("trying to sign contract");
        if (contractor == true && GameObject.Find("Stats").GetComponent<Statistics>().gold >= 1)
        {
            GameObject props = GameObject.Find("Properties");
            GameObject.Find("Stats").GetComponent<Statistics>().updateStats(diffgold: -1);
            foreach (Transform child in props.transform)
            {
                if (child.GetComponent<Property>() != null && child.GetComponent<Property>().Card.type == "House" && child.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder == 2)
                {
                    print("Need to sign " + child.name);
                    GameObject.Find("SignController").GetComponent<signController>().selProperty = child.gameObject;
                    GameObject.Find("SignController").GetComponent<signController>().signer(_cellIndex);
                }
            }
        } else if (contractor == false)
        {
            GameObject.Find("SignController").GetComponent<signController>().signer(_cellIndex);
        }
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(Sprite bgSprite, PropertyCard pCard, int cellIndex, GameObject selProp, bool contractor)
    {
        _cellIndex = cellIndex;
        bgImage.sprite = bgSprite;

        this.contractor = contractor;

        if (contractor == false)
        {
            int tempIncome = 0;
            switch (cellIndex)
            {
                case 0:
                    tempIncome = pCard.threemins;
                    tenantsText.text = pCard.tenants.ToString();
                    xpText.text = pCard.xpthreemins + " XP";
                    costText.text = "$100";
                    break;
                case 1:
                    tempIncome = pCard.thirtymins;
                    tenantsText.text = (pCard.tenants * 2).ToString();
                    xpText.text = pCard.xpthirtymins + " XP";
                    costText.text = "$580";
                    break;
                case 2:
                    tempIncome = pCard.onehour;
                    tenantsText.text = (pCard.tenants * 3).ToString();
                    xpText.text = pCard.xponehour + " XP";
                    costText.text = "$870";
                    break;
                case 3:
                    tempIncome = pCard.fourhours;
                    tenantsText.text = (pCard.tenants * 4).ToString();
                    xpText.text = pCard.xpfourhours + " XP";
                    costText.text = "$1,250";
                    break;
                case 4:
                    tempIncome = pCard.eighthours;
                    tenantsText.text = (pCard.tenants * 5).ToString();
                    xpText.text = pCard.xpeighthours + " XP";
                    costText.text = "$1,450";
                    break;
                case 5:
                    tempIncome = pCard.twelvehours;
                    tenantsText.text = (pCard.tenants * 6).ToString();
                    xpText.text = pCard.xptwelvehours + " XP";
                    costText.text = "$4,350";
                    break;
                case 6:
                    tempIncome = pCard.oneday;
                    tenantsText.text = (pCard.tenants * 7).ToString();
                    xpText.text = pCard.xponeday + " XP";
                    costText.text = "$5,800";
                    break;
                case 7:
                    tempIncome = pCard.twodays;
                    tenantsText.text = (pCard.tenants * 8).ToString();
                    xpText.text = pCard.xptwodays + " XP";
                    costText.text = "$7,250";
                    break;
                case 8:
                    tempIncome = pCard.threedays;
                    tenantsText.text = (pCard.tenants * 9).ToString();
                    xpText.text = pCard.xpthreedays + " XP";
                    costText.text = "$10,000";
                    break;
                default:
                    tempIncome = pCard.threemins;
                    tenantsText.text = pCard.tenants.ToString();
                    xpText.text = pCard.XP + " XP";
                    costText.text = "$100";
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
        } else
        {
            int count = 0;
            GameObject props = GameObject.Find("Properties");
            foreach (Transform child in props.transform)
            {
                if (child.GetComponent<Property>() != null && child.GetComponent<Property>().Card.type == "House" && child.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder == 2)
                {
                    print("Need to sign " + child.name);
                    count += 1;
                }
            }
            double costPerProp = 0;
            switch (cellIndex)
            {
                case 0: costPerProp = 100; break;
                case 1: costPerProp = 580; break;
                case 2: costPerProp = 870; break;
                case 3: costPerProp = 1250; break;
                case 4: costPerProp = 1450; break;
                case 5: costPerProp = 4350; break;
                case 6: costPerProp = 5800; break;
                case 7: costPerProp = 7250; break;
                case 8: costPerProp = 10000; break;
                default: costPerProp = 0; break;
            }
            long tempCost = (long)(costPerProp * (double)count * 1.2);
            if (tempCost >= 100000000)
            {
                string temp = tempCost.ToString("#,##0");
                costText.text = "$" + temp.Substring(0, temp.Length - 8) + "M";
            }
            else
            {
                costText.text = "$" + tempCost.ToString("#,##0");
            }
            incomeText.text = "-";
            tenantsText.text = "-";
            xpText.text = "- XP";
        }
    }

}
