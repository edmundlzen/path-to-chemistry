using UnityEngine;

public class CraftingTable : MonoBehaviour, IInteractable
{
    public void Interactable(Transform playerCamera)
    {
        GameObject.FindGameObjectsWithTag("MainGUIController")[0].GetComponent<MainGUIController>().ChangeView("inventory");
    }
}