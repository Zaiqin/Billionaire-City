using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingScreen : MonoBehaviour
{
    public AudioSource cam;
    public GameObject t, bgPan, titleImage, cover, skipCover, extAudio, tutorialScreen, internetObj, webObj, wheel, dlBar, stats, errorCode;
    public AudioClip introAudio;
    public bool canSkip = false;

    // Start is called before the first frame update
    void Awake()
    {
        skipCover.SetActive(true);
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            internetObj.SetActive(true);
            skipCover.SetActive(true);
        }
        else
        {
            print("call get version");
            webObj.GetComponent<webDownloader>().getVersion();
        }
    }

    public void startIntro()
    {
        wheel.GetComponent<CanvasGroup>().LeanAlpha(0f, 0.5f);
        StartCoroutine(Intro());
        skipCover.SetActive(false);
        cover.SetActive(true);
        dlBar.SetActive(false);
        errorCode.GetComponent<CanvasGroup>().LeanAlpha(0f, 0.5f);
    }

    private void Update()
    {
        wheel.transform.Rotate(0, 0, 360 * Time.deltaTime);
    }

    private IEnumerator Intro()
    {
        canSkip = true;

        yield return new WaitForSeconds(0.5f);
        extAudio.GetComponent<AudioSource>().PlayOneShot(introAudio);

        bgPan.GetComponent<CanvasGroup>().LeanAlpha(1f, 3f);
        bgPan.transform.LeanMoveLocalY(titleImage.transform.position.y - 350, 25f);
        titleImage.GetComponent<CanvasGroup>().LeanAlpha(1f, 1f);
        titleImage.transform.LeanScale(new Vector2(1.6f, 1.6f), 6f);
        yield return new WaitForSeconds(10f);

        titleImage.GetComponent<CanvasGroup>().LeanAlpha(0f, 1f);
        yield return new WaitForSeconds(2.5f);
        bgPan.GetComponent<CanvasGroup>().LeanAlpha(0.2f, 1.5f);
        yield return new WaitForSeconds(2f);

        t.GetComponent<Text>().text = "Created by ZQStudios";
        t.GetComponent<CanvasGroup>().LeanAlpha(1f, 0.5f);
        yield return new WaitForSeconds(3f);
        t.GetComponent<CanvasGroup>().LeanAlpha(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);

        t.GetComponent<Text>().text = "All rights reserved to Digital Chocolate";
        t.GetComponent<CanvasGroup>().LeanAlpha(1f, 0.5f);
        yield return new WaitForSeconds(3f);
        t.GetComponent<CanvasGroup>().LeanAlpha(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);

        t.GetComponent<Text>().text = "Credits to Rock You Inc";
        t.GetComponent<CanvasGroup>().LeanAlpha(1f, 0.5f);
        yield return new WaitForSeconds(3f);
        t.GetComponent<CanvasGroup>().LeanAlpha(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);

        bgPan.GetComponent<CanvasGroup>().LeanAlpha(0f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        this.gameObject.GetComponent<CanvasGroup>().LeanAlpha(0f, 0.5f);
        
        while (extAudio.GetComponent<AudioSource>().volume > 0.01f)
        {
            extAudio.GetComponent<AudioSource>().volume -= 2*(Time.deltaTime / 1.0f);
            yield return null;
        }
        extAudio.GetComponent<AudioSource>().Stop();

        //yield return new WaitForSeconds(0.5f);
        closeIntro();
    }

    private IEnumerator skipIntroAnim()
    {
        skipCover.SetActive(true);
        skipCover.GetComponent<CanvasGroup>().LeanAlpha(1f, 0.5f);
        yield return new WaitForSeconds(0.5f);

        bgPan.SetActive(false);
        titleImage.SetActive(false);
        t.SetActive(false);

        while (extAudio.GetComponent<AudioSource>().volume > 0.01f)
        {
            extAudio.GetComponent<AudioSource>().volume -= 2 * (Time.deltaTime / 1.0f);
            yield return null;
        }
        extAudio.GetComponent<AudioSource>().Stop();

        this.gameObject.GetComponent<CanvasGroup>().LeanAlpha(0f, 0.2f);
        yield return new WaitForSeconds(0.3f);
        closeIntro();
    }

    void closeIntro()
    {
        print("closing intro");
        if (tutorialScreen.activeSelf == false)
        {
            cover.SetActive(false);
        }
        this.gameObject.SetActive(false);
        if (stats.GetComponent<Statistics>().muted == false)
        {
            extAudio.GetComponent<AudioSource>().volume = 1.0f;
        } else
        {
            extAudio.GetComponent<AudioSource>().volume = 0f;
        }
        cam.Play();
        GameObject.Find("airplane").GetComponent<aeroplane>().refreshAirplane();
    }

    public void skipIntro()
    {
        if (canSkip == true)
        {
            StartCoroutine(skipIntroAnim());
        }
    }
}
