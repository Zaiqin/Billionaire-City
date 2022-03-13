using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using System;

//Cell class for demo. A cell in Recyclable Scroll Rect must have a cell class inheriting from ICell.
//The class is required to configure the cell(updating UI elements etc) according to the data during recycling of cells.
//The configuration of a cell is done through the DataSource SetCellData method.
//Check RecyclableScrollerDemo class
public class LevelCell : MonoBehaviour, ICell
{
    //UI
    public Image bgImage, propertyImage, priceIcon;
    public Text cardXP, cardCost, cardName;
    public Sprite cardBase, gold, money;

    //Model
    private int _cellIndex;

    private void Start()
    {
        
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(PropertyCard propertyCard, int cellIndex)
    {
        _cellIndex = cellIndex;
        bgImage.sprite = cardBase;

        cardName.text = propertyCard.displayName;
        if (propertyCard.displayName.Length > 22)
        {
            cardName.fontSize = 11;
        }
        else if (propertyCard.displayName.Length > 15)
        {
            cardName.fontSize = 15;
        }
        else
        {
            cardName.fontSize = 17;
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
    }

}
