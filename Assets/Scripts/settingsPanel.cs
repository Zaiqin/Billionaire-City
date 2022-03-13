using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class settingsPanel : MonoBehaviour
{
    public Text fineprint;
    public InputField inputReedem;

    public void initialise()
    {
        inputReedem.text = "";
        fineprint.text = "Version id: gv" + Application.version + "-sv" + int.Parse(File.ReadAllText(Application.persistentDataPath + "/spriteVersion.txt")).ToString();
    }
}
