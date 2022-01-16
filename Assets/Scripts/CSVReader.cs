using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CSVReader : MonoBehaviour
{
    [SerializeField]
    private TextAsset textAssetData;

    public Dictionary<int, long> levelValues = new Dictionary<int, long>();

    // Database of all Cards in the game --------------------------------------------------------
    public Dictionary<String, PropertyCard> CardDatabase = new Dictionary<string, PropertyCard>();

    /* ------------------------------------------------------------------------------------------
     2D Array of PropertyCards, only purpose is to get 4 arrays of each type for the shop
     without having to extract from CardDatabase Dictionary for each tab, having to select portions of it
     which requires needing to get no for each type, index range for each property type
    --------------------------------------------------------------------------------------------*/
    public PropertyCard[][] propertyCardArrays = new PropertyCard[4][];
    // ------------------------------------------------------------------------------------------
    public float maxDecoReach = 0f;

    public void ReadCSV()
    {
        //print("Reading CSV File");

        // Creation of Dictionaries to hold Sprites that are loaded from file -----------------------------
        Dictionary<String, Sprite> bgSprites = new Dictionary<string, Sprite>();
        Dictionary<String, Sprite> propSprites = new Dictionary<string, Sprite>();
        // Loading all Property Card Sprites from Resources without order ---------------------------------
        Sprite[] unsortedPropCardSprites = Resources.LoadAll<Sprite>("propertyCards");
        for (int i = 0; i < unsortedPropCardSprites.Length; i++)
        {
            bgSprites.Add(unsortedPropCardSprites[i].name, unsortedPropCardSprites[i]);
        }
        //print("No of propertyCard sprites in resource folder: " + bgSprites.Count);
        //Loading all Property Sprites from Resources without order ---------------------------------------
        Sprite[] unsortedPropSprites = Resources.LoadAll<Sprite>("properties");
        for (int i = 0; i < unsortedPropSprites.Length; i++)
        {
            propSprites.Add(unsortedPropSprites[i].name, unsortedPropSprites[i]);
        }
        //print("No of property sprites in resource folder: " + propSprites.Count);

        //Setup for reading CSV files ----------------------------------------------------------------------
        string TitleLine = textAssetData.text.Split(new string[] { "\r\n" }, StringSplitOptions.None)[0];
        string[] data = textAssetData.text.Split(new string[] { ",", "\r\n" }, StringSplitOptions.None);
        int cols = 1;
        
        //To find out how many kinds of Attributes ---------------------------------------------------------
        for (int dig = 0; dig < TitleLine.Length; dig++)
        {
            if (TitleLine[dig] == ',')
            {
                cols++;
            }
        }
        //print("There are " + cols + " columns in the stats file");
        // -------------------------------------------------------------------------------------------------

        //COUNTING NO OF PROPERTIES AND HOW MANY OF EACH ---------------------------------------------------
        int startIndex = 0; int endIndex = 0;
        int noOfHouses = 0; int noOfCommerces = 0; int noOfDecos = 0; int noOfWonders = 0;
        int a = 0; int propertyCycle = 1; string startName = "FIRST HOUSE"; string endName = "LAST HOUSE";
        while (propertyCycle != 5)
        {
            //print("i is " + a);
            if (data[(cols * (a + 2)) + 17] == startName)
            {
                startIndex = a;
            }
            if (data[(cols * (a + 2)) + 17] == endName)
            {
                endIndex = a;
                switch (propertyCycle)
                {
                    case 1:
                        startName = "FIRST COMMERCE";
                        endName = "LAST COMMERCE";
                        //print("There are " + (endIndex - startIndex + 1) + " Houses");
                        noOfHouses = (endIndex - startIndex + 1);
                        break;
                    case 2:
                        startName = "FIRST DECO";
                        endName = "LAST DECO";
                        //print("There are " + (endIndex - startIndex + 1) + " Commerces");
                        noOfCommerces = (endIndex - startIndex + 1);
                        break;
                    case 3:
                        startName = "FIRST WONDER";
                        endName = "LAST WONDER";
                        //print("There are " + (endIndex - startIndex + 1) + " Decos");
                        noOfDecos = (endIndex - startIndex + 1);
                        break;
                    case 4:
                        //print("There are " + (endIndex - startIndex + 1) + " Wonders");
                        noOfWonders = (endIndex - startIndex + 1);
                        break;
                    default: break;
                }
                propertyCycle++;
            }
            a++;
        }
        int totalNoOfProperties = noOfHouses + noOfCommerces + noOfDecos + noOfWonders;
        // -------------------------------------------------------------------------------------------------

        // Goes through each column in the CSV and gets all data -------------------------------------------
        PropertyCard[] tempArray = new PropertyCard[noOfHouses]; // Since first rows are houses in the CSV, tempArray will have total house count first
        int masterNo = 0; // Total noOfProperties so that for loop knows how many time to loop, the actual counter in the for loop.
        for (int i = 0; masterNo < totalNoOfProperties; i++) 
        {
            //int i is not the counter, instead its used to allow correct entry into each Property Type arrays. It is reset to 0 for each prop Type.
            tempArray[i] = new PropertyCard();
            string propImagePath = data[cols * (masterNo + 2)]; // Prop URL
            tempArray[i].propName = data[cols * (masterNo + 2)]; print("propname is " + tempArray[i].propName);
            tempArray[i].cost = data[cols * (masterNo + 2) + 1];
            tempArray[i].space = data[cols * (masterNo + 2) + 2];
            tempArray[i].tenants = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 3]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 3]);
            //--- Rent per time chosen --------
            tempArray[i].threemins = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 4]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 4]);
            tempArray[i].thirtymins = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 5]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 5]);
            tempArray[i].onehour = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 6]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 6]);
            tempArray[i].fourhours = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 7]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 7]);
            tempArray[i].eighthours = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 8]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 8]);
            tempArray[i].twelvehours = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 9]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 9]);
            tempArray[i].oneday = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 10]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 10]);
            tempArray[i].twodays = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 11]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 11]);
            tempArray[i].threedays = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 12]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 12]);
            //---------------------------------
            string bgImagePath = data[(cols * (masterNo + 2)) + 13]; // Card URL
            tempArray[i].rentPerTenant = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 14]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 14]);
            tempArray[i].influence = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 15]) ? "" : data[(cols * (masterNo + 2)) + 15];
            tempArray[i].decoBonus = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 16]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 16]);
            //-- FIRST AND LAST HOUSE COLUMN --
            tempArray[i].displayName = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 18]) ? "" : data[(cols * (masterNo + 2)) + 18];
            //-- RADIUS: NO OF INFLUENCE TILES WIDTH MINUS PROPERTY WIDTH --- EG: PIZZERIA 7X7 INFLUENCE - 3X3 PLOT = 4 RADIUS
            tempArray[i].buildTime = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 20]) ? "" : data[(cols * (masterNo + 2)) + 20];
            tempArray[i].XP = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 21]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 21]);
            tempArray[i].level = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 22]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 22]);
            //-- VARIOUS COLUMNS -----
            tempArray[i].wonderBonus = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 34]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 34]);
            //---------------------------------

            //--- Determine type of property ------
            if (tempArray[i].threemins != 0)
            {
                tempArray[i].type = "House";
            }
            else if (tempArray[i].decoBonus != 0)
            {
                tempArray[i].type = "Deco";
                // Check if current deco reach is higher than current record !!REMEMBER TO CHANGE ALL INFLUENCE TO 01x01 ie 5 digits long!!!!
                if ((float.Parse(tempArray[i].influence.Substring(0, 2)) > maxDecoReach))
                {
                    maxDecoReach = float.Parse(tempArray[i].influence.Substring(0, 2));
                }
                if (float.Parse(tempArray[i].influence.Substring(tempArray[i].influence.Length - 2)) > maxDecoReach){
                    maxDecoReach = float.Parse(tempArray[i].influence.Substring(tempArray[i].influence.Length - 2));
                }
            }
            else if (tempArray[i].buildTime.Contains("days"))
            {
                tempArray[i].type = "Wonder";
            }
            else
            {
                tempArray[i].type = "Commerce";
            }
            // ------------------------------------

            // Addition of PropertyCard Image and Property Image ---------------------------------------------
            // Addition of Property Card ie bgImage ---------------------------------
            if (bgSprites.ContainsKey(bgImagePath)) //check if cannot find url
            {
                tempArray[i].bgImage = bgSprites[bgImagePath];
            }
            else
            {
                tempArray[i].bgImage = bgSprites["bungalowCard"];
                print("Cannot locate card sprite for " + bgImagePath);
            }
            // Addition of Property Image ie propImage ------------------------------------
            if (propSprites.ContainsKey(propImagePath)) //check if cannot find url
            {
                tempArray[i].propImage = propSprites[propImagePath];
            }
            else
            {
                tempArray[i].propImage = propSprites["bungalow"];
                print("Cannot locate property sprite for " + propImagePath);
            }
            // -----------------------------------------------------------------------------------------------
            CardDatabase.Add(tempArray[i].displayName, tempArray[i]);
            // END OF INITIALISATION OF PROPERTY, BELOW IS TO DETECT LAST OF EACH TYPE OF PROPERTY ----------

            // IF REACH LAST OF EACH TYPE, RESET i SO THAT WHEN IT LOOPS AGAIN IT IS i=0, SO NEXT ARRAY WILL START FROM i=0.
            if (data[cols * (masterNo + 2) + 17] == "LAST HOUSE")
            {
                //print("Reached last house: " + tempArray[i].displayName);
                propertyCardArrays[0] = tempArray; // Set houseArray to tempArray
                i = -1;
                tempArray = new PropertyCard[noOfCommerces];
            }
            if (data[cols * (masterNo + 2) + 17] == "LAST COMMERCE")
            {
                //print("Reached last commerce: " + tempArray[i].displayName);
                propertyCardArrays[1] = tempArray; // Set commerceArray to tempArray
                i = -1;
                tempArray = new PropertyCard[noOfDecos];
            }
            if (data[cols * (masterNo + 2) + 17] == "LAST DECO")
            {
                //print("Reached last deco: " + tempArray[i].displayName);
                propertyCardArrays[2] = tempArray; // Set decoArray to tempArray
                i = -1;
                tempArray = new PropertyCard[noOfWonders];
            }
            if (data[cols * (masterNo + 2) + 17] == "LAST WONDER")
            {
                //print("Reached last wonder: " + tempArray[i].displayName);
                propertyCardArrays[3] = tempArray; // Set wonderArray to tempArray
                i = -1;
            }

            masterNo++;
        }

        // Getting xp for levels
        for (int i = 1; i < data.Length; i++)
        {
            //print("reading " + data[(cols * (i)) + 32]);
            levelValues.Add(i, long.Parse(data[(cols * (i)) + 32]));
            /* FormatException: Input string was not in a correct format.
             * 
             * To fix issue, make sure levels in csv file do not have the E+11 to it, but in full numbers
             * select the level values with E+xx, format cells > numbers > decimal places = 0
             * 
             */
            if (data[(cols * (i)) + 33] == "Last Level"){
                //print("reached last level");
                break;
            }
        }
        //print("end of csv, max reach is " + maxDecoReach);
        print("max deco reach is " + maxDecoReach);
        if (maxDecoReach % 2 == 0)
        {
            maxDecoReach = maxDecoReach / 2;
        } else
        {
            maxDecoReach = (maxDecoReach - 1) / 2;
        }
        print("max deco reach after calc is " + maxDecoReach);

        print("Completed reading CSV data");
    }

}
