using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class debugScript : MonoBehaviour
{
   

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Text>().text = FileHandler.GetPath("propsSave.json");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
