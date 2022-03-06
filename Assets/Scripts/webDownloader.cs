using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class webDownloader : MonoBehaviour
{
    public GameObject csvObj;
    public GameObject scroller;
    public TextAsset localAsset;

    public void downloadStats()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://zaiqin.github.io/ZQStudios/BCstats.csv");
        yield return www.SendWebRequest();

        
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
            Application.Quit();
        }
        else
        {
            // Show results as text
            print(www.downloadHandler.text);
            csvObj.GetComponent<CSVReader>().csvText = www.downloadHandler.text;
            scroller.GetComponent<RecyclableScrollerDemo>().initCSV();
        }
    }
}
