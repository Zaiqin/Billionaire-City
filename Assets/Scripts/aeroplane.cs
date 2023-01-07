using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aeroplane : MonoBehaviour
{
    public GameObject loadingScreen;

    public GameObject t1, t2, t3, t4, t5, t6;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (loadingScreen.activeSelf == false)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(-50f, 0f, -8f), 2f * Time.deltaTime);
        }
    }

    public void refreshAirplane()
    {
        t1.GetComponent<planeText>().refreshText();
        t2.GetComponent<planeText>().refreshText();
        t3.GetComponent<planeText>().refreshText();
        t4.GetComponent<planeText>().refreshText();
        t5.GetComponent<planeText>().refreshText();
        t6.GetComponent<planeText>().refreshText();
    }

}
