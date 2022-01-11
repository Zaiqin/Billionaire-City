using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class expansion : MonoBehaviour
{
    public int i;
    public GameObject expCanvas, stats, saveObj, expPopup, failPopup, nocash;
    public List<int> expInts = new List<int>();
    public List<string> deletedExp = new List<string>();
    public Tilemap map;
    public TileBase grassTile;

    // Start is called before the first frame update
    void Start()
    {
        if (failPopup.gameObject.activeSelf == false)
        {
            foreach (Transform child in expCanvas.transform)
            {
                expInts.Add(int.Parse(child.name.Substring(child.name.Length - 2)));
                //print("added " + child.name);
            }
        }
    }

    public void delFunc()
    {
        int cost = 0;
        print("called delfunc with val" + i);
        bool result = true;
        if (i > 10 && i < 20)
        {
            cost = 1000000;
        }
        if (i > 20 && i < 30)
        {
            cost = 5000000;
            foreach (int s in expInts)
            {
                if (s > 10 && s < 20 ) { result = false; }
            }
        }
        else if (i > 30 && i < 40)
        {
            cost = 10000000;
            foreach (int s in expInts)
            {
                if (s > 20 && s < 30) { result = false; }
            }
        }
        else if (i > 40 && i < 50)
        {
            cost = 50000000;
            foreach (int s in expInts)
            {
                if (s > 30 && s < 40) { result = false; }
            }
        }
        else if (i > 50 && i < 60)
        {
            cost = 100000000;
            foreach (int s in expInts)
            {
                if (s > 40 && s < 50) { result = false; }
            }
        }
        if (result == true && stats.GetComponent<Statistics>().returnStats()[0] >= cost)
        {
            print("del");
            expPopup.SetActive(false);
            GameObject sel = GameObject.Find("expansion" + i);
            int spaceX = 1, spaceY = 1, x = 1, y = 1, xOrig = 1;
            switch (i)
            {
                case 11: spaceX = 30; spaceY = 10; x = -15; xOrig = -15; y = 10; break;
                default:
                    break;
            }
            for (int i = 0; i < spaceX * spaceY; i++)
            {
                map.SetTile(new Vector3Int(x, y, 0), grassTile);
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
        }
        else if (result == true && stats.GetComponent<Statistics>().returnStats()[0] < cost)
        {
            nocash.gameObject.SetActive(true);
        } else {
            expPopup.SetActive(false);
            failPopup.SetActive(true);
        }
    }

}
