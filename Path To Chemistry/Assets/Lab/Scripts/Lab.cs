using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Lab : MonoBehaviour
{
    public GameObject objectToSpawn;
    public GameObject Target;
    private ARRaycastManager arRaycastManager;
    private bool isPoseValid;
    private GameObject objectSpawn;
    private Pose Pose;

    private void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            GameObject.Find("Label").GetComponent<Text>().text = Input.GetTouch(0).position.ToString();
        } 
        if (objectSpawn == null && isPoseValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) placeObject();
        {
            updatePose();
            updateCrosshair();
        }
    }

    private void placeObject()
    {
        objectSpawn = Instantiate(objectToSpawn, Pose.position, Pose.rotation);
    }

    private void updatePose()
    {
        //var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var Hit = new List<ARRaycastHit>();
        arRaycastManager.Raycast(Input.GetTouch(0).position, Hit, TrackableType.Planes);
        isPoseValid = Hit.Count > 0;
        if (isPoseValid)
        {
            Pose = Hit[0].pose;
        }
    }

    private void updateCrosshair()
    {
        if (objectSpawn == null && isPoseValid)
        {
            Target.SetActive(true);
            Target.transform.SetPositionAndRotation(Pose.position, Pose.rotation);
        }
        else
        {
            Target.SetActive(false);
        }
    }
}