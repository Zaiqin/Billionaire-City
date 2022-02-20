using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInsuff : MonoBehaviour
{

    [SerializeField]
    public GameObject insuffParent, insuff;

    [SerializeField]
    private Sprite moneyInsuff, goldInsuff;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void turnOnInsuff(string type)
    {
        if (type == "money"){
            insuff.GetComponent<Image>().sprite = moneyInsuff;
        } else
        {
            insuff.GetComponent<Image>().sprite = goldInsuff;
        }
        print("turn on");
        insuffParent.SetActive(true);
        insuffParent.transform.localScale = Vector2.zero;
        insuffParent.transform.LeanScale(Vector2.one, 0.2f).setEaseOutBack();
    }

    public void turnOffInsuff()
    {
        //print("turn off");
        insuffParent.SetActive(false);
    }
}
