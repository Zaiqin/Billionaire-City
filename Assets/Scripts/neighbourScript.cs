using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class neighbourScript : MonoBehaviour
{

    [SerializeField]
    public GameObject cover, dragCover, PropertiesParent, expParent, saveloadsystemobj, expPopup, shop, cam, aeroplane, transitionCover, backCityToggle;
    public Tilemap map;
    public void OnButtonClick()
    {
        
        if (cover.activeSelf == false)
        {
            cam.GetComponent<SpriteDetector>().inNeighbour = true;
            transitionCover.SetActive(true);
            dragCover.SetActive(true);
            cover.SetActive(true);
            StartCoroutine(loadMap(true));
            shop.GetComponent<PurchaseController>().visitNeighbour();
        }
        else
        {
            dragCover.SetActive(false);
            cover.SetActive(false);
            transitionCover.SetActive(true);
            StartCoroutine(loadMap(false));
            GameObject.Find("ExternalAudioPlayer").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/touchSound"));
            shop.GetComponent<PurchaseController>().quitNeighbour();
            
        }
    }

    private IEnumerator loadMap(bool neighbour)
    {
        transitionCover.GetComponent<CanvasGroup>().LeanAlpha(1f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        if (neighbour == true)
        {
            backCityToggle.SetActive(true);
        }
        else
        {
            backCityToggle.SetActive(false);
        }
        foreach (Transform child in PropertiesParent.transform)
        {
            if (child.name != "HQ")
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        foreach (Transform child in expParent.transform)
        {
            child.gameObject.SetActive(true);
        }
        List<tileSaveForm> tilelist = new List<tileSaveForm>();
        List<propertySaveForm> list = new List<propertySaveForm>();
        if (neighbour == true)
        {
            tilelist = FileHandler.ReadListFromJSON<tileSaveForm>("Map/tileSave.json");
            list = FileHandler.ReadListFromJSON<propertySaveForm>("Map/propsSave.json");
        } else
        {
            tilelist = FileHandler.ReadListFromJSON<tileSaveForm>("tileSave.json");
            list = FileHandler.ReadListFromJSON<propertySaveForm>("propsSave.json");
        }
        foreach (var item in tilelist)
        {
            Vector3Int pos = new Vector3Int((int)item.x, (int)item.y, 0);
            if (item.texName.Contains("road") || item.texName.Contains("Grass") || item.texName.Contains("Below"))
            {
                map.SetTile(pos, Resources.Load<TileBase>("roadTiles/" + item.texName));
                //print("loaded road" + item.texName + "at pos " + pos);
            }
            else if (item.texName.Contains("plot"))
            {
                map.SetTile(pos, Resources.Load<TileBase>("plotTiles/" + item.texName));
                //print("loaded plot" + item.texName + "at pos " + pos);
            }
        }
        foreach (var p in list)
        {
            //print("locX: " + p.locX + "locY: " + p.locY);
            if (p.locX == 0 && p.locY == 0) // To prevent spawning in center of world
            {
                print("Found erroneous property" + p.propName + p.locX + p.locY);
            }
            else
            {
                if (neighbour == true)
                {
                    saveloadsystemobj.GetComponent<saveloadsystem>().loadProperty(p.propName, new Vector2Int(p.locX, p.locY), p.signTime, p.signIndex, p.signCreationTime, p.comSignTime, p.comSignCreationTime, p.constructStart, p.constructEnd, true);
                }
                else
                {
                    saveloadsystemobj.GetComponent<saveloadsystem>().loadProperty(p.propName, new Vector2Int(p.locX, p.locY), p.signTime, p.signIndex, p.signCreationTime, p.comSignTime, p.comSignCreationTime, p.constructStart, p.constructEnd);
                }
                
            }
            //print("loaded: " + p.propName);
        }
        if (neighbour == true)
        {
            expPopup.GetComponent<expansion>().deletedExp = FileHandler.ReadListFromJSON<string>("Map/deletedExp.json");
        }
        else
        {
            expPopup.GetComponent<expansion>().deletedExp = FileHandler.ReadListFromJSON<string>("deletedExp.json");
        }
        foreach (string s in expPopup.GetComponent<expansion>().deletedExp)
        {
            GameObject.Find(s).SetActive(false);
        }
        expPopup.GetComponent<expansion>().updateSprite();
        aeroplane.transform.position = new Vector3Int(25, 0, -8);
        transitionCover.GetComponent<CanvasGroup>().LeanAlpha(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        transitionCover.SetActive(false);
    }
}
