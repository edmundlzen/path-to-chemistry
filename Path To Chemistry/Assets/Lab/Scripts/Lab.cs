using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Lab : MonoBehaviour
{
    public GameObject objectToSpawn;
    public GameObject Target;
    private GameObject objectSpawn;
    private Pose Pose;
    private ARRaycastManager arRaycastManager;
    private bool isPoseValid = false;
    int Hotbar = 1;

    void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
    }
    void Update()
    {
        if (objectSpawn == null && isPoseValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            placeObject();
        }
        updatePose();
        updateCrosshair();
    }
    void placeObject()
    {
        objectSpawn = Instantiate(objectToSpawn, Pose.position, Pose.rotation);
    }
    void updatePose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var Hit = new List<ARRaycastHit>();
        arRaycastManager.Raycast(screenCenter, Hit, TrackableType.Planes);
        isPoseValid = Hit.Count > 0;
        if (isPoseValid)
        {
            Pose = Hit[0].pose;
        }
    }
    void updateCrosshair()
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