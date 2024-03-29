using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

public class CSVReader : MonoBehaviour
{
    public GameObject saveloadobj, statsObj, webDL, dlBar;
    public TextAsset textAssetData;
    public string csvText;
    public List<Mission> missionList = new List<Mission>();
    public GameObject redeem;
    public bool needToDownload;

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

    public List<string> ratios = new List<string>();

    public IEnumerator ReadCSV()
    {
        //print("Reading CSV File");
        if (!Directory.Exists(Application.persistentDataPath + "/properties"))
        {
            print("No property sprites in app");
            Directory.CreateDirectory(Application.persistentDataPath + "/properties");
            //Directory.CreateDirectory(Application.persistentDataPath + "/propertyCards");
            needToDownload = true;
            dlBar.SetActive(true);
        } else
        {
            print("Property files are present in app");
        }

        //Setup for reading CSV files ----------------------------------------------------------------------
        string TitleLine = csvText.Split(new string[] { "\n" }, StringSplitOptions.None)[0]; //If use local excel change to \r\n
        string[] data = csvText.Split(new string[] { ",", "\n" }, StringSplitOptions.None);  //If use local excel change to \r\n
        int cols = 1;
        
        //To find out how many kinds of Attributes ---------------------------------------------------------
        for (int dig = 0; dig < TitleLine.Length; dig++)
        {
            if (TitleLine[dig] == ',')
            {
                cols++;
            }
        }
        print("There are " + cols + " columns in the stats file");
        // -------------------------------------------------------------------------------------------------

        //COUNTING NO OF PROPERTIES AND HOW MANY OF EACH ---------------------------------------------------
        int startIndex = 0; int endIndex = 0;
        int noOfHouses = 0; int noOfCommerces = 0; int noOfDecos = 0; int noOfWonders = 0;
        int a = 0; int propertyCycle = 1; string startName = "FIRST HOUSE"; string endName = "LAST HOUSE";
        while (propertyCycle != 5)
        {
            // If there is an error detecting how many houses, check missions titles and descriptions and ensure no commas ----
            if (data[(cols * (a + 2)) + 17] == startName)
            {
                startIndex = a;
            }
            if (data[(cols * (a + 2)) + 17] == endName)
            {
                //print("hit last name");
                endIndex = a;
                switch (propertyCycle)
                {
                    case 1:
                        startName = "FIRST COMMERCE";
                        endName = "LAST COMMERCE";
                        print("There are " + (endIndex - startIndex + 1) + " Houses");
                        noOfHouses = (endIndex - startIndex + 1);
                        break;
                    case 2:
                        startName = "FIRST DECO";
                        endName = "LAST DECO";
                        print("There are " + (endIndex - startIndex + 1) + " Commerces");
                        noOfCommerces = (endIndex - startIndex + 1);
                        break;
                    case 3:
                        startName = "FIRST WONDER";
                        endName = "LAST WONDER";
                        print("There are " + (endIndex - startIndex + 1) + " Decos");
                        noOfDecos = (endIndex - startIndex + 1);
                        break;
                    case 4:
                        print("There are " + (endIndex - startIndex + 1) + " Wonders");
                        noOfWonders = (endIndex - startIndex + 1);
                        break;
                    default: break;
                }
                propertyCycle++;
            }
            a++;
        }
        //print("done calc");
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
            tempArray[i].propName = data[cols * (masterNo + 2)];
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
            tempArray[i].xpthreemins = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 23]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 23]);
            tempArray[i].xpthirtymins = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 24]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 24]);
            tempArray[i].xponehour = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 25]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 25]);
            tempArray[i].xpfourhours = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 26]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 26]);
            tempArray[i].xpeighthours = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 27]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 27]);
            tempArray[i].xptwelvehours = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 28]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 28]);
            tempArray[i].xponeday = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 29]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 29]);
            tempArray[i].xptwodays = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 30]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 30]);
            tempArray[i].xpthreedays = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 31]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 31]);
            tempArray[i].limited = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 32]) ? "" : data[(cols * (masterNo + 2)) + 32];
            //-- Level values and First/last level column ---
            tempArray[i].wonderBonus = string.IsNullOrEmpty(data[(cols * (masterNo + 2)) + 35]) ? 0 : int.Parse(data[(cols * (masterNo + 2)) + 35]);
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
            else if (tempArray[i].buildTime.Contains("day"))
            {
                tempArray[i].type = "Wonder";
            }
            else
            {
                tempArray[i].type = "Commerce";
            }
            // ------------------------------------

            if (tempArray[i].type != "Deco")
            {
                float rN = float.Parse(tempArray[i].space.Substring(0, 1)) / float.Parse(tempArray[i].space.Substring(tempArray[i].space.Length - 1));

                ratios.Add(rN.ToString());
            }

            if (needToDownload == true)
            {
                print("Fetching " + propImagePath);
                yield return StartCoroutine(webDL.GetComponent<webDownloader>().DownloadImage("https://zaiqin.github.io/ZQStudios/properties/" + propImagePath + ".png", propImagePath, "properties"));
                dlBar.transform.GetChild(0).GetComponent<Image>().fillAmount = (float)((float)masterNo / (float)totalNoOfProperties);
            }
            //string path = Application.persistentDataPath + "/propertyCards/" + bgImagePath + ".png";
            //tempArray[i].bgImage = LoadSprite(path);
            string path2 = Application.persistentDataPath + "/properties/" + propImagePath + ".png";
            tempArray[i].propImage = LoadSprite(path2);
            // -----------------------------------------------------------------------------------------------
            CardDatabase.Add(tempArray[i].displayName, tempArray[i]);
            //print("Adding Card " + tempArray[i].displayName + " to card database");
            // END OF INITIALISATION OF PROPERTY, BELOW IS TO DETECT LAST OF EACH TYPE OF PROPERTY ----------

            if (tempArray[i].limited != null && tempArray[i].limited == "ENDED")
            {
                print("Removing " + tempArray[i].displayName + " from shop");
                RemoveAt(ref tempArray, i);
                i--;
            }
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
            levelValues.Add(i, long.Parse(data[(cols * (i)) + 33]));
            /* FormatException: Input string was not in a correct format.
             * 
             * To fix issue, make sure levels in csv file do not have the E+11 to it, but in full numbers
             * select the level values with E+xx, format cells > numbers > decimal places = 0
             * 
             */
            if (data[(cols * (i)) + 34] == "Last Level")
            {
                //print("reached last level");
                break;
            }
        }

        // Getting mission details
        
        for (int i = 1; i < data.Length; i++)
        {
            missionList.Add(new Mission(data[(cols * (i)) + 36], data[(cols * (i)) + 37], float.Parse(data[(cols * (i)) + 38]), data[(cols * (i)) + 39], false, data[(cols * (i)) + 41]));
            if (data[(cols * (i)) + 40] == "Last Mission"){
                print("There are " + i + " Missions");
                break;
            }
        }

        Dictionary<string, string> codes = new Dictionary<string, string>();
        for (int i = 1; i < data.Length; i++)
        {
            if (data[(cols * (i)) + 42] == "No Code")
            {
                print("There are " + (i-1) + " Codes");
                break;
            }
            codes.Add(data[(cols * (i)) + 42], data[(cols * (i)) + 43]);
            print("Added code: " + data[(cols * (i)) + 42]);
        }
        redeem.GetComponent<redeemCode>().codes = codes;

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
        saveloadobj.GetComponent<saveloadsystem>().StartGame();
        statsObj.GetComponent<Statistics>().initStats();
    }

    public static void RemoveAt<T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {
            // moving elements downwards, to fill the gap at [index]
            arr[a] = arr[a + 1];
        }
        // finally, let's decrement Array's size by one
        Array.Resize(ref arr, arr.Length - 1);
        //print("There is now " + arr.Length);
    }

    private Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        if (File.Exists(Application.persistentDataPath + "/spriteVersion.txt") == false)
        {
            File.Delete(Application.persistentDataPath + "/spriteVersion.txt");
        }
        if (Directory.Exists(Application.persistentDataPath + "/properties") == true)
        {
            Directory.Delete(Application.persistentDataPath + "/properties", true);
        }
        print("crashing");
        Application.Quit();
        return null;
    }
}
