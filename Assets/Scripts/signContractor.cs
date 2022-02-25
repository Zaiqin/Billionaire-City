using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class signContractor : MonoBehaviour
{
    public GameObject props, signController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void signAll()
    {
        print("Called sign all");
        foreach (Transform child in props.transform)
        {
            if (child.GetComponent<Property>() != null && child.GetComponent<Property>().Card.type == "House" && child.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder == 2)
            {
                print("Need to sign " + child.name);
                signController.GetComponent<signController>().selProperty = child.gameObject;
                signController.GetComponent<signController>().signer(0);
            }
        }
    }
}
