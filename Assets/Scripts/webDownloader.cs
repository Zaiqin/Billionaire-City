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

    [ContextMenu("Fetch image")]
    public void getImage(string path, string folder)
    {
        StartCoroutine(DownloadImage("https://zaiqin.github.io/ZQStudios/"+folder+"/"+path+".png",path, folder));
    }
    IEnumerator DownloadImage(string MediaUrl, string rawPath, string folder)
    {
        print("downloading " + MediaUrl);
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(request.error);
        else
        {
            // ImageComponent.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;

            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            saveImage(sprite, rawPath, folder);
        }
    }

    public void saveImage(Sprite itemBGSprite, string filename, string folder)
    {
        Texture2D itemBGTex = itemBGSprite.texture;
        byte[] itemBGBytes = itemBGTex.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "/" + folder + "/" + filename + ".png", itemBGBytes);
    }

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
            if (float.Parse(Application.version) < float.Parse(www.downloadHandler.text))
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
