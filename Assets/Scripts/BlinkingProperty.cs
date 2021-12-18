using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingProperty : MonoBehaviour
{
    bool stop;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("StartBlink", 0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartBlink()
    {
        StartCoroutine(BlinkCoroutine());
    }

    public void StopBlink()
    {
        CancelInvoke();
        stop = true;

    }
    IEnumerator BlinkCoroutine()
    {
        if (stop == false)
        {
            this.GetComponent<Renderer>().material.color = Color.green;
        }
        yield return new WaitForSeconds(0.5f);
        if (stop == false)
        {
            this.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}

