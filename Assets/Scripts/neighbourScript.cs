using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class neighbourScript : MonoBehaviour
{

    [SerializeField]
    public GameObject cover, dragCover;
    public void OnButtonClick()
    {
        if (cover.activeSelf == false)
        {
            dragCover.SetActive(true);
            cover.SetActive(true);
        } else
        {
            dragCover.SetActive(false);
            cover.SetActive(false);
        }
    }
}
