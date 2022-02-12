using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class wait : MonoBehaviour
{
    public VideoPlayer videoplayer;
    public int sceneIndex;

    void Start()
    {
        videoplayer = videoplayer.GetComponent<VideoPlayer>();
        StartCoroutine(WaitforVideoEnd());
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    IEnumerator WaitforVideoEnd()
    {
        float videolength = (float)videoplayer.length;
        yield return new WaitForSeconds(videolength);
        SceneManager.LoadScene(sceneIndex);
    }
}
