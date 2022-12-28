using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class expansion : MonoBehaviour
{
    public int i;
    public GameObject expCanvas, stats, saveObj, expPopup, failPopup, nocash;
    public List<int> expInts = new List<int>();
    public List<string> deletedExp = new List<string>();
    public Tilemap map;
    public TileBase grassTile;
    public Sprite saleUnlocked;

    public GameObject sale21, sale22, sale23, sale24, sale31, sale32, sale33, sale34, sale41, sale42, sale43, sale44, sale45, sale46, sale47, sale48, sale51, sale52, sale53, sale54;

    // Start is called before the first frame update
    void Start()
    {
        if (failPopup.gameObject.activeSelf == false)
        {
            foreach (Transform child in expCanvas.transform)
            {
                if (child.transform.gameObject.activeSelf == true)
                {
                    expInts.Add(int.Parse(child.name.Substring(child.name.Length - 2)));
                    print("added " + child.name);
                }
            }
        }
    }

    public void closePanel()
    {
        print("clsoe luck");
        expPopup.transform.LeanScale(Vector2.zero, 0.2f).setEaseInBack();
        Invoke("setInactive", 0.2f);
    }

    void setInactive()
    {
        expPopup.gameObject.SetActive(false);
        expPopup.gameObject.transform.localScale = Vector2.one;
    }

    public void updateSprite()
    {
        if (deletedExp.Count >= 20)
        {
            if (sale51 != null) { sale51.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale52 != null) { sale52.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale53 != null) { sale53.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale54 != null) { sale54.GetComponent<Image>().sprite = saleUnlocked; };
        }
        else if (deletedExp.Count >= 12)
        {
            if (sale41 != null) { sale41.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale42 != null) { sale42.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale43 != null) { sale43.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale44 != null) { sale44.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale45 != null) { sale45.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale46 != null) { sale46.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale47 != null) { sale47.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale48 != null) { sale48.GetComponent<Image>().sprite = saleUnlocked; };
        }
        else if (deletedExp.Count >= 8)
        {
            if (sale31 != null) { sale31.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale32 != null) { sale32.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale33 != null) { sale33.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale34 != null) { sale34.GetComponent<Image>().sprite = saleUnlocked; };
        }
        else if (deletedExp.Count >= 4)
        {
            if (sale21 != null) { sale21.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale22 != null) { sale22.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale23 != null) { sale23.GetComponent<Image>().sprite = saleUnlocked; };
            if (sale24 != null) { sale24.GetComponent<Image>().sprite = saleUnlocked; };
        }
    }

    public void delFunc()
    {
        int cost = 0;
        print("called delfunc with val" + i);
        bool result = true;
        if (i > 10 && i < 20)
        {
            //cost = 1000000;
        }
        if (i > 20 && i < 30)
        {
            foreach (int s in expInts)
            {
                if (s > 10 && s < 20) { result = false; }
            }
        }
        else if (i > 30 && i < 40)
        {
                foreach (int s in expInts)
                {
                    if (s > 20 && s < 30) { result = false; }
                }
            
        }
        else if (i > 40 && i < 50)
        {
            foreach (int s in expInts)
            {
                if (s > 30 && s < 40) { result = false; }
            }
        }
        else if (i > 50 && i < 60)
        {
            foreach (int s in expInts)
            {
                if (s > 40 && s < 50) { result = false; }
            }
        }
        switch (deletedExp.Count)
        {
            case 0: cost = 4000000; break;
            case 1: cost = 6000000; break;
            case 2: cost = 7000000; break;
            case 3: cost = 9000000; break;
            case 4: cost = 30000000; break;
            case 5: cost = 36000000; break;
            case 6: cost = 42000000; break;
            case 7: cost = 48000000; break;
            case 8: cost = 60000000; break;
            case 9: cost = 72000000; break;
            case 10: cost = 84000000; break;
            case 11: cost = 96000000; break;
            case 12: cost = 180000000; break;
            case 13: cost = 195000000; break;
            case 14: cost = 210000000; break;
            case 15: cost = 228000000; break;
            case 16: cost = 300000000; break;
            case 17: cost = 360000000; break;
            case 18: cost = 420000000; break;
            case 19: cost = 480000000; break;
            case 20: cost = 540000000; break;
            case 21: cost = 600000000; break;
            case 22: cost = 660000000; break;
            case 23: cost = 800000000; break;
            default:
                break;
        }
        if (result == true && stats.GetComponent<Statistics>().returnStats()[0] >= cost)
        {
            print("del");
            expPopup.SetActive(false);
            GameObject sel = GameObject.Find("expansion" + i);
            int spaceX = 1, spaceY = 1, x = 1, y = 1, xOrig = 1;
            switch (i)
            {
                case 11: spaceX = 30; spaceY = 10; x = -15; y = 10; break;
                case 12: spaceX = 15; spaceY = 20; x = 15; y = -10; break;
                case 13: spaceX = 30; spaceY = 10; x = -15; y = -20; break;
                case 14: spaceX = 15; spaceY = 20; x = -30; y = -10; break;
                case 21: spaceX = 15; spaceY = 10; x = -30; y = 10; break;
                case 22: spaceX = 15; spaceY = 10; x = 15; y = 10; break;
                case 23: spaceX = 15; spaceY = 10; x = 15; y = -20; break;
                case 24: spaceX = 15; spaceY = 10; x = -30; y = -20; break;
                case 31: spaceX = 30; spaceY = 10; x = -15; y = 20; break;
                case 32: spaceX = 15; spaceY = 20; x = 30; y = -10; break;
                case 33: spaceX = 30; spaceY = 10; x = -15; y = -30; break;
                case 34: spaceX = 15; spaceY = 20; x = -45; y = -10; break;
                case 41: spaceX = 15; spaceY = 10; x = -30; y = 20; break;
                case 42: spaceX = 15; spaceY = 10; x = 15; y = 20; break;
                case 43: spaceX = 15; spaceY = 10; x = 30; y = 10; break;
                case 44: spaceX = 15; spaceY = 10; x = 30; y = -20; break;
                case 45: spaceX = 15; spaceY = 10; x = 15; y = -30; break;
                case 46: spaceX = 15; spaceY = 10; x = -30; y = -30; break;
                case 47: spaceX = 15; spaceY = 10; x = -45; y = -20; break;
                case 48: spaceX = 15; spaceY = 10; x = -45; y = 10; break;
                case 51: spaceX = 15; spaceY = 10; x = 30; y = 20; break;
                case 52: spaceX = 15; spaceY = 10; x = 30; y = -30; break;
                case 53: spaceX = 15; spaceY = 10; x = -45; y = -30; break;
                case 54: spaceX = 15; spaceY = 10; x = -45; y = 20; break;
                default:
                    break;
            }
            xOrig = x;
            for (int i = 0; i < spaceX * spaceY; i++)
            {
                if (map.GetTile(new Vector3Int(x, y, 0)).name == "noBelow")
                {
                    map.SetTile(new Vector3Int(x, y, 0), grassTile);
                }
                x += 1;
                if (x == (xOrig + spaceX))
                {
                    x = xOrig; y += 1;
                }
            }
            Destroy(sel);
            deletedExp.Add("expansion" + i);
            expInts.Remove(i);
            stats.GetComponent<Statistics>().updateStats(diffmoney: -cost);
            saveObj.GetComponent<saveloadsystem>().saveExp();
            saveObj.GetComponent<saveloadsystem>().saveTilemap();
            stats.GetComponent<Statistics>().expCost += cost;
            updateSprite();
        }
        else if (result == true && stats.GetComponent<Statistics>().returnStats()[0] < cost)
        {
            nocash.gameObject.SetActive(true);
            nocash.transform.localScale = Vector2.zero;
            nocash.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
            closePanel();
        }
    }

}
