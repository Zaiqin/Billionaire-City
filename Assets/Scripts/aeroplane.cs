using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aeroplane : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    { 
        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(-50f, 0f, -8f), 2f * Time.deltaTime);

        if (this.transform.position.x == -50f)
        {
            Destroy(this.gameObject);
        }
    }
}
