using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hideNeighbour : MonoBehaviour
{

    public GameObject neighbourBar;
    public bool hidden = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hide()
    {
        if (hidden == false)
        {
            LeanTween.moveLocalY(neighbourBar, neighbourBar.transform.localPosition.y - 115, 0.05f);
            hidden = true;
        } else
        {
            LeanTween.moveLocalY(neighbourBar, neighbourBar.transform.localPosition.y + 115, 0.05f);
            hidden = false;
        }
    }
}
