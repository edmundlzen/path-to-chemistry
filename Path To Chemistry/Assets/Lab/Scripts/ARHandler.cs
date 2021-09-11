using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARHandler : MonoBehaviour
{
    public GameObject Flask;
    public GameObject Indicator;
    private GameObject spawnedObject;
    private Pose placementPose;
    private ARRaycastManager arRaycastManager;
    private bool isPoseValid = false;
    void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
    }
    void Update()
    {
        if (spawnedObject == null && isPoseValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            spawnedObject = Instantiate(Flask, placementPose.position, placementPose.rotation);
        }
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var Hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(screenCenter, Hits, TrackableType.Planes);
        isPoseValid = Hits.Count > 0;
        if (isPoseValid)
        {
            placementPose = Hits[0].pose;
        }
        if (spawnedObject == null && isPoseValid)
        {
            Indicator.SetActive(true);
            Indicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            Indicator.SetActive(false);
        }
    }
}
