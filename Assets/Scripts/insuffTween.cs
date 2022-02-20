using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class insuffTween : MonoBehaviour
{
    public void closePanel()
    {
        this.transform.parent.transform.LeanScale(Vector2.zero, 0.2f).setEaseInBack();
        Invoke("setInactive", 0.2f);
    }

    void setInactive()
    {
        this.transform.parent.gameObject.SetActive(false);
        this.transform.parent.gameObject.transform.localScale = Vector2.one;
    }

}
