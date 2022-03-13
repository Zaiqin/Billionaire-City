using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class moneyBarScript : MonoBehaviour, IPointerClickHandler
{
    public GameObject convertMoney, moneyBar, goldBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("tapped");
        convertMoney.gameObject.SetActive(true);
        convertMoney.transform.localScale = Vector2.zero;
        convertMoney.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
        moneyBar.transform.GetChild(0).GetComponent<Text>().text = this.transform.parent.transform.GetChild(0).GetComponent<Text>().text;
        goldBar.transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("GoldBar").transform.GetChild(0).GetComponent<Text>().text;
    }
}
