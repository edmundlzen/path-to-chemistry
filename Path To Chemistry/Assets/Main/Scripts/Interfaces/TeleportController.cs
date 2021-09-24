using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class TeleportController : MonoBehaviour
{
    private GameObject hotbar;
    private GameObject teleportItems;

    private void Awake()
    {
        teleportItems = transform.Find("Teleport Items").Find("Viewport").Find("Teleports").gameObject;
    }

    private void OnEnable()
    {
        UpdateTeleports();
    }

    public void TeleportClickHandler(GameObject teleport)
    {
        var playerData = PlayerData.Instance();
        var teleports = playerData.teleports;
        
        //Load scene here later
    }

    private void UpdateTeleports()
    {
        var firstTeleport = teleportItems.transform.GetChild(0);
        firstTeleport.gameObject.SetActive(false);
        foreach (Transform materialSlot in teleportItems.transform)
        {
            if (materialSlot.GetSiblingIndex() != 0)
            {
                Destroy(materialSlot.gameObject);
            }
        }
        
        var playerData = PlayerData.Instance();
        var teleports = playerData.teleports;
        var nonResellable = playerData.nonResellable;

        foreach (var teleport in teleports)
        {
            if (!nonResellable[teleport.Key]) return;
            if (firstTeleport.gameObject.activeSelf)
            {
                firstTeleport.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + teleport.Value["image"]);
                firstTeleport.Find("Title").GetComponent<Text>().text = teleport.Key;
                firstTeleport.Find("Description").GetComponent<Text>().text = teleport.Value["description"].ToString();

                firstTeleport.name = teleport.Key;
                firstTeleport.gameObject.SetActive(true);
                continue;
            }

            Transform newMaterialSlot = Instantiate(firstTeleport);
            newMaterialSlot.SetParent(teleportItems.transform, false);
            
            newMaterialSlot.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + teleport.Value["image"]);
            newMaterialSlot.Find("Title").GetComponent<Text>().text = teleport.Key;
            newMaterialSlot.Find("Description").GetComponent<Text>().text = teleport.Value["description"].ToString();
            
            newMaterialSlot.name = teleport.Key;
            newMaterialSlot.gameObject.SetActive(true);
        }
    }
}