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
    public Text timeText;
    public Text titleText;

    public long signAllCost;

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
        GameObject props = GameObject.Find("Properties");
        GameObject sc = GameObject.Find("SignController");
        bool hasAtLeastOneToSign = false;
        foreach (Transform child in props.transform)
        {
            if (child.GetComponent<Property>() != null && child.GetComponent<Property>().Card.type == "House" && child.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder == 2)
            {
                hasAtLeastOneToSign = true;
                break;
            }
        }
        if (contractor == true && GameObject.Find("Stats").GetComponent<Statistics>().gold >= 1 && GameObject.Find("Stats").GetComponent<Statistics>().money >= signAllCost && hasAtLeastOneToSign == true)
        {
            GameObject.Find("Stats").GetComponent<Statistics>().updateStats(diffgold: -1);
            foreach (Transform child in props.transform)
            {
                if (child.GetComponent<Property>() != null && child.GetComponent<Property>().Card.type == "House" && child.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder == 2)
                {
                    print("Need to sign " + child.name);
                    sc.GetComponent<signController>().selProperty = child.gameObject;
                    sc.GetComponent<signController>().signer(_cellIndex, true);
                }
            }
            sc.GetComponent<signController>().extAudio.GetComponent<AudioSource>().PlayOneShot(sc.GetComponent<signController>().contractSound);
        } else if (contractor == true && hasAtLeastOneToSign == true) {
            sc.GetComponent<signController>().insuff.SetActive(true);
        } else if (contractor == false)
        {
            sc.GetComponent<signController>().signer(_cellIndex, false);
        }
    }

    public long[] calcContract(PropertyCard pCard, int cellIndex, GameObject selProp)
    {
        long[] result = new long[4]; // income, tenants, xp, cost

        result[1] = pCard.tenants * (cellIndex+1);

        int tempIncome = 0;
        switch (cellIndex)
        {
            case 0:
                tempIncome = pCard.threemins;
                result[2] = pCard.xpthreemins;
                result[3] = 100;
                break;
            case 1:
                tempIncome = pCard.thirtymins;
                result[2] = pCard.xpthirtymins;
                result[3] = 580;
                break;
            case 2:
                tempIncome = pCard.onehour;
                result[2] = pCard.xponehour;
                result[3] = 870;
                break;
            case 3:
                tempIncome = pCard.fourhours;
                result[2] = pCard.xpfourhours;
                result[3] = 1250;
                break;
            case 4:
                tempIncome = pCard.eighthours;
                result[2] = pCard.xpeighthours;
                result[3] = 1450;
                break;
            case 5:
                tempIncome = pCard.twelvehours;
                result[2] = pCard.xptwelvehours;
                result[3] = 4350;
                break;
            case 6:
                tempIncome = pCard.oneday;
                result[2] = pCard.xponeday;
                result[3] = 5800;
                break;
            case 7:
                tempIncome = pCard.twodays;
                result[2] = pCard.xptwodays;
                result[3] = 7250;
                break;
            case 8:
                tempIncome = pCard.threedays;
                result[2] = pCard.xpthreedays;
                result[3] = 10000;
                break;
            default: break;
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

        result[0] = finalProfit;
        return result;
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(Sprite bgSprite, PropertyCard pCard, int cellIndex, GameObject selProp, bool contractor, bool goCalc)
    {
        _cellIndex = cellIndex;
        //bgImage.sprite = bgSprite;

        this.contractor = contractor;

        if (contractor == false)
        {
            long[] res = calcContract(pCard, cellIndex, selProp);

            incomeText.text = "$" + res[0].ToString("#,##0");
            xpText.text = res[1].ToString("#,##0") + " XP";
            tenantsText.text = res[2].ToString("#,##0");
            costText.text = "$" + res[3].ToString("#,##0");

        } else
        {
            if (goCalc == false)
            {
                incomeText.text = "Pending...";
                xpText.text = "Pending...";
                tenantsText.text = "Pending...";
                costText.text = "---";
            }
            else
            {
                long totalIncome = 0;
                long totalXP = 0;
                long totalTenants = 0;
                long totalCost = 0;
                GameObject props = GameObject.Find("Properties");
                foreach (Transform child in props.transform)
                {
                    if (child.GetComponent<Property>() != null && child.GetComponent<Property>().Card.type == "House" && child.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder == 2)
                    {
                        print("Need to sign " + child.name);
                        long[] res = calcContract(child.GetComponent<Property>().Card, cellIndex, child.gameObject);
                        totalIncome += res[0];
                        totalXP += res[1];
                        totalTenants += res[2];
                        totalCost += res[3];
                    }
                }

                signAllCost = totalCost;
                if (totalIncome >= 10000000)
                {
                    string temp = totalIncome.ToString("#,##0");
                    incomeText.text = "$" + temp.Substring(0, temp.Length - 8) + "M";
                }
                else
                {
                    incomeText.text = "$" + totalIncome.ToString("#,##0");
                }

                if (totalXP >= 10000000)
                {
                    string temp = totalXP.ToString("#,##0");
                    xpText.text = temp.Substring(0, temp.Length - 8) + "M XP";
                }
                else
                {
                    xpText.text = totalXP.ToString("#,##0") + " XP";
                }

                if (totalTenants >= 10000000)
                {
                    string temp = totalTenants.ToString("#,##0");
                    tenantsText.text = temp.Substring(0, temp.Length - 8) + "M";
                }
                else
                {
                    tenantsText.text = totalTenants.ToString("#,##0");
                }

                if (totalCost >= 10000000)
                {
                    string temp = totalCost.ToString("#,##0");
                    costText.text = "$" + temp.Substring(0, temp.Length - 8) + "M";
                }
                else
                {
                    costText.text = "$" + totalCost.ToString("#,##0");
                }
            }
        }
        switch (cellIndex)
        {
            case 0:
                titleText.text = "Visitors";
                timeText.text = "3 mins"; 
                break;
            case 1:
                titleText.text = "Backpackers";
                timeText.text = "30 mins";
                break;
            case 2:
                titleText.text = "Tourists";
                timeText.text = "1 hr";
                break;
            case 3:
                titleText.text = "Globetrotters";
                timeText.text = "4 hrs";
                break;           
            case 4:
                titleText.text = "Officers";
                timeText.text = "8 hrs";
                break;
            case 5:
                titleText.text = "Executives";
                timeText.text = "12 hrs";
                break;
            case 6:
                titleText.text = "Managers";
                timeText.text = "1 day";
                break;
            case 7:
                titleText.text = "Directors";
                timeText.text = "2 days";
                break;
            case 8:
                titleText.text = "Officials";
                timeText.text = "3 days";
                break;
            default: break;
        }
    }

}
