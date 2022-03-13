using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class redeemCode : MonoBehaviour
{
    public Text inputText;
    public InputField field;
    public Dictionary<string, string> codes = new Dictionary<string, string>();
    public List<string> usedCodes;
    public AudioSource extAudio;
    public AudioClip redeemSound;
    public GameObject stats, storageController, saveObj;
    public void redeem()
    {
        bool used = false;
        foreach (KeyValuePair<string, string> code in codes)
        {
            if (code.Key == inputText.text)
            {
                foreach (string x in usedCodes)
                {
                    if (x == code.Key)
                    {
                        used = true;
                        StartCoroutine(usedError());
                        break;
                    }
                }
                if (used == false)
                {
                    extAudio.PlayOneShot(redeemSound);
                    print("Redeemed code:" + inputText.text);

                    if (code.Value.Contains("Gold"))
                    {
                        stats.GetComponent<Statistics>().updateStats(diffgold: int.Parse(code.Value.Substring(0, code.Value.Length - 5)));
                    }
                    else if (code.Value.Contains("XP"))
                    {
                        stats.GetComponent<Statistics>().updateStats(diffxp: int.Parse(code.Value.Substring(0, code.Value.Length - 3)));
                    }
                    else if (code.Value.Contains("Prop"))
                    {
                        storageController.GetComponent<RecyclableScrollerStorage>().addIntoStorage(code.Value.Substring(4, code.Value.Length - 4));
                    }
                    else
                    {
                        stats.GetComponent<Statistics>().updateStats(diffmoney: int.Parse(code.Value));
                    }
                    usedCodes.Add(code.Key);
                    saveObj.GetComponent<saveloadsystem>().saveCode();
                }
            }
        }
    }

    private IEnumerator usedError()
    {
        field.interactable = false;
        field.textComponent.color = Color.red;
        field.text = "Code already used";
        yield return new WaitForSeconds(0.3f);
        field.text = "";
        yield return new WaitForSeconds(0.3f);
        field.text = "Code already used";
        yield return new WaitForSeconds(0.3f);
        field.text = "";
        yield return new WaitForSeconds(0.3f);
        field.text = "Code already used";
        yield return new WaitForSeconds(0.3f);
        field.text = "";
        field.textComponent.color = Color.white;
        field.interactable = true;
    }
}
