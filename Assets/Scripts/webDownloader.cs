using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class webDownloader : MonoBehaviour
{
    public GameObject csvObj, scroller, loadingScreen, dlButton;
    public UnityWebRequest spriteWeb;

    public IEnumerator DownloadImage(string MediaUrl, string rawPath, string folder)
    {
        //print("downloading " + MediaUrl);
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(request.error);
        else
        {
            // ImageComponent.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;

            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            yield return saveImage(sprite, rawPath, folder);
            //print("yield return saveImage");
        }
    }

    public IEnumerator saveImage(Sprite itemBGSprite, string filename, string folder)
    {
        Texture2D itemBGTex = itemBGSprite.texture;
        byte[] itemBGBytes = itemBGTex.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "/" + folder + "/" + filename + ".png", itemBGBytes);
        yield return new WaitUntil(() => File.Exists(Application.persistentDataPath + "/" + folder + "/" + filename + ".png"));
        //print("yield return saveImage2");
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
                UnityWebRequest w = UnityWebRequest.Get("https://zaiqin.github.io/ZQStudios/spriteVersion.txt");
                yield return w.SendWebRequest();

                if (w.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(w.error);
                    Application.Quit();
                }
                else
                {
                    if (File.Exists(Application.persistentDataPath+"/spriteVersion.txt") == false)
                    {
                        if (Directory.Exists(Application.persistentDataPath + "/properties") == true)
                        {
                            Directory.Delete(Application.persistentDataPath + "/properties",true);
                        }
                    }
                    // If property folder exists or sprite version is less than server
                    if (Directory.Exists(Application.persistentDataPath + "/properties") == false || int.Parse(File.ReadAllText(Application.persistentDataPath + "/spriteVersion.txt")) < int.Parse(w.downloadHandler.text))
                    {
                            loadingScreen.GetComponent<loadingScreen>().internetObj.SetActive(true);
                            loadingScreen.transform.GetChild(5).gameObject.SetActive(false);
                            loadingScreen.transform.GetChild(3).gameObject.SetActive(true);
                            dlButton.SetActive(true);
                            spriteWeb = w;
                            if (Directory.Exists(Application.persistentDataPath + "/properties") == false)
                            {
                                loadingScreen.GetComponent<loadingScreen>().internetObj.transform.GetChild(0).GetComponent<Text>().text = "Downloads required\n\n\nThe game needs to download assets and files.\nProceed?\n";
                            }
                            else
                            {
                                loadingScreen.GetComponent<loadingScreen>().internetObj.transform.GetChild(0).GetComponent<Text>().text = "New updates!!\n\n\nDo you wish to download new assets in the game?\n";
                            }
                            csvObj.GetComponent<CSVReader>().needToDownload = true;
                    }
                    else
                    {
                        downloadStats();
                    }
                }
            }
        }
    }

    public void dl()
    {
        dlButton.SetActive(false);
        loadingScreen.GetComponent<loadingScreen>().internetObj.SetActive(false);
        loadingScreen.transform.GetChild(5).gameObject.SetActive(true);
        loadingScreen.transform.GetChild(3).gameObject.SetActive(true);
        loadingScreen.transform.GetChild(7).gameObject.SetActive(true);
        if (File.Exists(Application.persistentDataPath + "/spriteVersion.txt") == true)
        {
            File.Delete(Application.persistentDataPath + "/spriteVersion.txt");
        }
        downloadStats();
    }

    public void downloadSpriteVer()
    {
        if (spriteWeb != null)
        {
            File.WriteAllText(Application.persistentDataPath + "/spriteVersion.txt", spriteWeb.downloadHandler.text);
        }
    }

    public void downloadStats()
    {
        StartCoroutine(GetCSV());
    }

    IEnumerator GetCSV()
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
