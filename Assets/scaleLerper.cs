using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleLerper : MonoBehaviour
{

    //add public to these varables to allow editing in inspector
    Vector3 minScale = new Vector3(0.9f, 0.9f, 1f);
    Vector3 maxScale = new Vector3(1.2f, 1.2f, 1f);
    bool repeatable = true;
    float speed = 2f;
    float duration = 1f;

    IEnumerator Start()
    {
        while (repeatable)
        {
            yield return RepeatLerp(maxScale, minScale, duration);
            yield return RepeatLerp(minScale, maxScale, duration);
        }
    }

    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }
}
