using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class missionPopup : MonoBehaviour
{
    public void func(string name) {
        StartCoroutine(mission(name));
    }

    public IEnumerator mission(string name)
    {
        this.gameObject.SetActive(true);
        this.gameObject.transform.GetChild(1).GetComponent<Text>().text = name;
        Vector3 prevPos = this.gameObject.transform.position;
        this.gameObject.transform.LeanMoveLocalY(this.gameObject.GetComponent<BoxCollider2D>().size.y + this.gameObject.GetComponent<BoxCollider2D>().size.y / 4, 0.2f).setEaseOutBack();
        yield return new WaitForSeconds(2f);
        this.gameObject.transform.LeanMoveLocalY(Screen.height - this.gameObject.GetComponent<BoxCollider2D>().size.y*4, 0.2f).setEaseInBack();
        yield return new WaitForSeconds(0.2f);
        this.gameObject.SetActive(false);
    }
}
