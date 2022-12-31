using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class webDownloader : MonoBehaviour
{
    public GameObject csvObj, scroller, loadingScreen, dlButton, neighbourParent;
    public UnityWebRequest spriteWeb;

    public IEnumerator DownloadImage(string MediaUrl, string rawPath, string folder)
    {
        print("downloading " + MediaUrl);
        loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (1/28)...";
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError) {
            Debug.Log(request.error);
            loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Error 1";
        } else {
            // ImageComponent.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;

            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            //print("yield return saveImage");
            loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (2/28)...";
            yield return saveImage(sprite, rawPath, folder);
        }
    }

    public IEnumerator saveImage(Sprite itemBGSprite, string filename, string folder)
    {
        Texture2D itemBGTex = itemBGSprite.texture;
        byte[] itemBGBytes = itemBGTex.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "/" + folder + "/" + filename + ".png", itemBGBytes);
        //print("yield return saveImage2");
        loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (3/28)...";
        yield return new WaitUntil(() => File.Exists(Application.persistentDataPath + "/" + folder + "/" + filename + ".png"));
        
    }

    public void getVersion()
    {
        print("starting downloadversion coroutine");
        loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (4/28)...";
        StartCoroutine(DownloadVersion());
    }

    IEnumerator DownloadVersion()
    {
        loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (5/28)...";
        UnityWebRequest www = UnityWebRequest.Get("https://zaiqin.github.io/ZQStudios/reqVersion.txt");
        loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (6/28)...";
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
            print("connection error");
            loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (7/28)...";
            Application.Quit();
        }
        else
        {
            // Show results as text
            print("Local Version " + float.Parse(Application.version.Substring(2)) + ", Server version " + float.Parse(www.downloadHandler.text.Substring(2)));
            loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (8/28)...";
            if (float.Parse(Application.version.Substring(2)) < float.Parse(www.downloadHandler.text.Substring(2)))
            {
                loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (9/28)...";
                loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (10/28)...";
                loadingScreen.GetComponent<loadingScreen>().internetObj.SetActive(true);
                loadingScreen.transform.GetChild(5).gameObject.SetActive(false);
                loadingScreen.transform.GetChild(3).gameObject.SetActive(true);
                loadingScreen.GetComponent<loadingScreen>().internetObj.transform.GetChild(0).GetComponent<Text>().text = "Outdated version\n\n\nPlease update your game in the Google Play store to version " + www.downloadHandler.text + "\n";
            }
            else
            {
                loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (11/28)...";
                UnityWebRequest w = UnityWebRequest.Get("https://zaiqin.github.io/ZQStudios/spriteVersion.txt");
                yield return w.SendWebRequest();
                loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (12/28)...";
                if (w.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(w.error);
                    loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (13/28)...";
                    Application.Quit();
                }
                else
                {
                    if (File.Exists(Application.persistentDataPath+"/spriteVersion.txt") == false)
                    {
                        loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (14/28)...";
                        if (Directory.Exists(Application.persistentDataPath + "/properties") == true)
                        {
                            loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (15/28)...";
                            Directory.Delete(Application.persistentDataPath + "/properties",true);
                        }
                    }
                    // If property folder exists or sprite version is less than server
                    if (Directory.Exists(Application.persistentDataPath + "/properties") == false || int.Parse(File.ReadAllText(Application.persistentDataPath + "/spriteVersion.txt")) < int.Parse(w.downloadHandler.text))
                    {
                            loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (16/28)...";
                            loadingScreen.GetComponent<loadingScreen>().internetObj.SetActive(true);
                            loadingScreen.transform.GetChild(5).gameObject.SetActive(false);
                            loadingScreen.transform.GetChild(3).gameObject.SetActive(true);
                            dlButton.SetActive(true);
                            spriteWeb = w;
                            if (Directory.Exists(Application.persistentDataPath + "/properties") == false)
                            {
                                loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (17/28)...";
                                loadingScreen.GetComponent<loadingScreen>().internetObj.transform.GetChild(0).GetComponent<Text>().text = "Downloads required\n\n\nThe game needs to download assets and files.\nProceed?\n";
                            }
                            else
                            {
                                loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (18/28)...";
                                loadingScreen.GetComponent<loadingScreen>().internetObj.transform.GetChild(0).GetComponent<Text>().text = "New updates!!\n\n\nDo you wish to download new assets in the game?\n";
                            }
                                loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (19/28)...";
                                csvObj.GetComponent<CSVReader>().needToDownload = true;
                    }
                    else
                    {
                        loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (20/28)...";
                        downloadStats();
                    }
                }
            }
        }
    }

    public void dl()
    {
        loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (21/28)...";
        dlButton.SetActive(false);
        loadingScreen.GetComponent<loadingScreen>().internetObj.SetActive(false);
        loadingScreen.transform.GetChild(5).gameObject.SetActive(true);
        loadingScreen.transform.GetChild(3).gameObject.SetActive(true);
        loadingScreen.transform.GetChild(7).gameObject.SetActive(true);
        if (File.Exists(Application.persistentDataPath + "/spriteVersion.txt") == true)
        {
            loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (22/28)...";
            File.Delete(Application.persistentDataPath + "/spriteVersion.txt");
        }
        loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (23/28)...";
        downloadStats();
    }

    public void downloadSpriteVer()
    {
        loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (24/28)...";
        if (spriteWeb != null)
        {
            loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (25/28)...";
            File.WriteAllText(Application.persistentDataPath + "/spriteVersion.txt", spriteWeb.downloadHandler.text);
        }
    }

    public void downloadStats()
    {
        loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (26/28)...";
        StartCoroutine(GetCSV());
    }

    IEnumerator GetCSV()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://zaiqin.github.io/ZQStudios/BCstats.csv");
        loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (27/28)...";
        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Error 2";
            Debug.Log(www.error);
            Application.Quit();
        }
        else
        {
            // Show results as text
            print("dl:"+www.downloadHandler.text);
            loadingScreen.GetComponent<loadingScreen>().errorCode.GetComponent<Text>().text = "Loading (28/28)...";
            csvObj.GetComponent<CSVReader>().csvText = www.downloadHandler.text;
            scroller.GetComponent<RecyclableScrollerDemo>().initCSV();
        }
        UnityWebRequest propRonald = UnityWebRequest.Get("https://zaiqin.github.io/ZQStudios/propsSaveRonald.json");
        yield return propRonald.SendWebRequest();
        neighbourParent.GetComponent<neighbourScript>().propsSaveRonald = propRonald.downloadHandler.text;
        UnityWebRequest tileRonald = UnityWebRequest.Get("https://zaiqin.github.io/ZQStudios/tileSaveRonald.json");
        yield return tileRonald.SendWebRequest();
        neighbourParent.GetComponent<neighbourScript>().tileSaveRonald = tileRonald.downloadHandler.text;
        UnityWebRequest expRonald = UnityWebRequest.Get("https://zaiqin.github.io/ZQStudios/deletedExpRonald.json");
        yield return expRonald.SendWebRequest();
        neighbourParent.GetComponent<neighbourScript>().deletedExpRonald = expRonald.downloadHandler.text;
    }
}
