using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingProperty : MonoBehaviour
{
    public bool stop;
    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("StartBlink", 0f, 0.5f);
    }

    [ContextMenu("startRepeating")]
    public void invokeStart()
    {
        InvokeRepeating("StartBlink", 0f, 0.5f);
    }


    public void StartBlink()
    {
        StartCoroutine(BlinkCoroutine());
        stop = false;
    }

    [ContextMenu("stopBlink")]
    public void StopBlink()
    {
        //print("stopping blink");
        CancelInvoke();
        stop = true;

    }
    IEnumerator BlinkCoroutine()
    {
        if (stop == false)
        {
            this.GetComponent<Renderer>().material.color = Color.yellow;
        }
        yield return new WaitForSeconds(0.25f);
        if (stop == false)
        {
            this.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}

