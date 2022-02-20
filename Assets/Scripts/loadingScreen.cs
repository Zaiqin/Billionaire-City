using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadingScreen : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Invoke("closeIntro", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void closeIntro()
    {
        this.gameObject.SetActive(false);
    }
}
