using System.Collections.Generic;
using UnityEngine;

public class GenerateObjects : MonoBehaviour
{
    public ObjectGenerationSettings objectGenerationSettings;
    private Dictionary<GameObject, int> currentObjects = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, int> numberOfObjects = new Dictionary<GameObject, int>();
    private Renderer r;

    private float randomX;
    private float randomZ;

    private void Start()
    {
        r = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (objectGenerationSettings.objects.Length != 0)
        {
            var objects = objectGenerationSettings.objects;
            for (var i = 0; i < objects.Length; i++)
            {
                if (!numberOfObjects.ContainsKey(objects[i].gameObject) ||
                    !currentObjects.ContainsKey(objects[i].gameObject))
                {
                    numberOfObjects.Add(objects[i].gameObject,
                        Random.Range(objects[i].minimumAmount, objects[i].maximumAmount));
                    currentObjects.Add(objects[i].gameObject, 0);
                }

                RaycastHit hit;
                if (currentObjects[objects[i].gameObject] <= numberOfObjects[objects[i].gameObject])
                {
                    randomX = Random.Range(r.bounds.min.x, r.bounds.max.x);
                    randomZ = Random.Range(r.bounds.min.z, r.bounds.max.z);
                    if (Physics.Raycast(new Vector3(randomX, r.bounds.max.y + 5f, randomZ), -Vector3.up, out hit))
                        if (hit.point.y >= objects[i].minimumHeight && hit.point.y <= objects[i].maximumHeight)
                        {
                            GameObject newObject;
                            if (objectGenerationSettings.objects[i].followRotation)
                            {
                                newObject = Instantiate(objects[i].gameObject, hit.point,
                                    Quaternion.FromToRotation(Vector3.up, hit.normal));
                            }
                            else
                            {
                                newObject = Instantiate(objects[i].gameObject, hit.point, Quaternion.identity);
                            }

                            newObject.transform.SetParent(transform);
                            currentObjects[objects[i].gameObject]++;
                        }
                }
            }
        }
    }

    private void AssignObjectGeneratorSettings(ObjectGenerationSettings objectGenerationSettings)
    {
        this.objectGenerationSettings = objectGenerationSettings;
    }
}