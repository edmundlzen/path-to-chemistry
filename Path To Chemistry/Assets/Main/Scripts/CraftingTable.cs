using UnityEngine;

public class CraftingTable : MonoBehaviour, Iinteractable
{
    public void Interactable(Transform playerCamera)
    {
        GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>().ChangeView("inventory");
    }
}