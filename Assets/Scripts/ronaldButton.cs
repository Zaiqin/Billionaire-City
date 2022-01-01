using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ronaldButton : MonoBehaviour
{
    [SerializeField]
    private GameObject levelUpScreen;

    [SerializeField]
    private Statistics stats;

    [SerializeField]
    Text levelText;

    [SerializeField]
    ParticleSystem particle;

    public void OnButtonClick()
    {
        if (levelUpScreen.activeSelf == false)
        {
            levelUpScreen.SetActive(true);
            stats.updateStats(diffmoney: 1000000, diffgold: 100);
            levelText.text = "Level " + stats.level.ToString();
            particle.Play();
        }
        else if (levelUpScreen.activeSelf == true)
        {
            levelUpScreen.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
