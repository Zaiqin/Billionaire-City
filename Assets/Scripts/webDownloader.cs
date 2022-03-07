using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class webDownloader : MonoBehaviour
{
    public GameObject csvObj;
    public GameObject scroller;
    public GameObject loadingScreen;

    public void getVersion()
    {
        StartCoroutine(DownloadVersion());
    }

    IEnumerator DownloadVersion()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://zaiqin.github.io/ZQStudios/reqVersion.txt");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
            Application.Quit();
        }
        else
        {
            // Show results as text
            print("Local Version " + Application.version + ", Server version " + www.downloadHandler.text);
            if (Application.version != www.downloadHandler.text)
            {
                loadingScreen.GetComponent<loadingScreen>().internetObj.SetActive(true);
                loadingScreen.transform.GetChild(5).gameObject.SetActive(false);
                loadingScreen.transform.GetChild(3).gameObject.SetActive(true);
                loadingScreen.GetComponent<loadingScreen>().internetObj.transform.GetChild(0).GetComponent<Text>().text = "Outdated version\n\n\nPlease update your game in the Google Play store to version " + www.downloadHandler.text + "\n";
            }
            else
            {
                downloadStats();
            }
        }
    }

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
