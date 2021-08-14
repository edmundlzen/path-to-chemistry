using System.Collections.Generic;
using UnityEngine;

public class GenerateObjects : MonoBehaviour
{
    public Dictionary<GameObject, int> numberOfObjects = new Dictionary<GameObject, int>();
    public Dictionary<GameObject, int> currentObjects = new Dictionary<GameObject, int>();
    public ObjectGenerationSettings objectGenerationSettings;

    private float randomX;
    private float randomZ;
    private Renderer r;

    void Start()
    {
        r = GetComponent<Renderer>();
    }

    void Update()
    {
        if (objectGenerationSettings.objects != null)
        {
            var objects = objectGenerationSettings.objects;
            for (int i = 0; i < objects.Length; i++)
            {
                if (!numberOfObjects.ContainsKey(objects[i].gameObject) || !currentObjects.ContainsKey(objects[i].gameObject))
                {
                    numberOfObjects.Add(objects[i].gameObject, Random.Range(objects[i].minimumAmount, objects[i].maximumAmount));
                    currentObjects.Add(objects[i].gameObject, 0); 
                }
                RaycastHit hit;
                if (currentObjects[objects[i].gameObject] <= numberOfObjects[objects[i].gameObject])
                {
                    randomX = Random.Range(r.bounds.min.x, r.bounds.max.x);
                    randomZ = Random.Range(r.bounds.min.z, r.bounds.max.z);
                    if (Physics.Raycast(new Vector3(randomX, r.bounds.max.y + 5f, randomZ), -Vector3.up, out hit))
                    {
                        Instantiate(objects[i].gameObject, hit.point, Quaternion.identity);
                        currentObjects[objects[i].gameObject] += 1;
                    }
                }
            }
        }
    }

    void AssignObjectGeneratorSettings(ObjectGenerationSettings objectGenerationSettings)
    {
        this.objectGenerationSettings = objectGenerationSettings;
    }
}