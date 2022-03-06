using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
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

    TextAsset ConvertStringToTextAsset(string text)
    {
        string temporaryTextFileName = "TemporaryTextFile";
        File.WriteAllText(Application.dataPath + "/Resources/" + temporaryTextFileName + ".txt", text);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        TextAsset textAsset = Resources.Load(temporaryTextFileName) as TextAsset;
        return textAsset;
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://zaiqin.github.io/ZQStudios/BCstats.csv");
        yield return www.SendWebRequest();

        
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            print(www.downloadHandler.text);
            TextAsset asset = ConvertStringToTextAsset(www.downloadHandler.text);
            csvObj.GetComponent<CSVReader>().textAssetData = asset;
            scroller.GetComponent<RecyclableScrollerDemo>().initCSV();
        }
    }
}
