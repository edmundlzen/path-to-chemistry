using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    private bool useButton = false;
    public Transform playerCamera;
    public GameObject interactButton;

    public void InteractButtonDown()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit))
        {
            if (hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interactable(playerCamera);
            }
        }
    }
    
    public void UseButtonDown()
    {
        // useButton = true;
        
        if (GameObject.FindGameObjectsWithTag("HandItem")[0].TryGetComponent<Placeable>(out Placeable placable)) 
        {
            placable.Use(playerCamera);
        } else if (GameObject.FindGameObjectsWithTag("HandItem")[0].TryGetComponent<IUsable>(out IUsable usable))
        {
            usable.Use(playerCamera);
        }
    }

    public void UseButtonUp()
    {
        // useButton = false;
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 2))
        {
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                interactButton.SetActive(true);
                return;
            }
        }
        interactButton.SetActive(false);
    }
}
