using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expScript : MonoBehaviour
{

    public GameObject expPopup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void pressExp()
    {
        print("name is " + this.name);
        expPopup.GetComponent<expansion>().i = int.Parse(this.name.Substring(this.name.Length - 2));
    }
}
