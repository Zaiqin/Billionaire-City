using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class signContractor : MonoBehaviour
{
    public GameObject props, signController, closeButton, cover;

    public void signAll()
    {
        print("Called sign all");

        print("clicked on contract");
        GameObject canvas = GameObject.Find("Canvas");
        GameObject contractController = GameObject.Find("Contract Scroll Controller");
        GameObject contractMenu = contractController.GetComponent<RecyclableScrollerContract>().contractParent;
        cover.SetActive(true);

        if (contractMenu.activeSelf == true)
        {
            this.GetComponent<Toggle>().isOn = false;
            closeButton.GetComponent<insuffTween>().closePanel();
            cover.SetActive(false);
        }
        else
        {
            contractMenu.SetActive(true);
            contractMenu.transform.localScale = Vector2.zero;
            contractMenu.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
            contractMenu.transform.GetChild(3).gameObject.SetActive(true);
            GameObject infoPanel = canvas.transform.GetChild(0).gameObject;
            infoPanel.SetActive(false);
            contractController.GetComponent<RecyclableScrollerContract>().contractor = true;
            contractController.GetComponent<RecyclableScrollerContract>().goCalc = false;
            //contractController.GetComponent<RecyclableScrollerContract>().userReloadData();

            GameObject.Find("SignController").GetComponent<signController>().selProperty = this.gameObject.transform.parent.gameObject;

            if (GameObject.Find("Canvas").transform.GetChild(0).GetComponent<infoScript>().highlightedProp != null)
            {
                GameObject hProp = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<infoScript>().highlightedProp;
                hProp.GetComponent<SpriteRenderer>().material.color = Color.white;

                if (hProp.GetComponent<Property>().Card.type == "House" && hProp.transform.childCount > 4)
                {
                    Destroy(hProp.transform.GetChild(4).gameObject);
                }
            }

            Invoke("calcAll", 0.2f);
        }
    }

    void calcAll()
    {
        GameObject contractController = GameObject.Find("Contract Scroll Controller");
        contractController.GetComponent<RecyclableScrollerContract>().userReloadData();
    }

    public void calcActual()
    {
        GameObject contractController = GameObject.Find("Contract Scroll Controller");
        contractController.GetComponent<RecyclableScrollerContract>().goCalc = true;
        contractController.GetComponent<RecyclableScrollerContract>().userReloadData();
    }
}
