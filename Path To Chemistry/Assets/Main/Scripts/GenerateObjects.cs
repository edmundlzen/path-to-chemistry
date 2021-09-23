using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

public class GenerateObjects : MonoBehaviour
{
    public ObjectGenerationSettings objectGenerationSettings;
    private Dictionary<GameObject, int> currentObjects = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, int> numberOfObjects = new Dictionary<GameObject, int>();
    private List<GameObject> generatedObjects = new List<GameObject>();
    private Transform player;
    private TerrainData terrainData;
    private TerrainTextureDetector terrainTextureDetector;
    private float randomX;
    private float randomZ;

    private void Start()
    {
        terrainData = GetComponent<Terrain>().terrainData;
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        new Task(DisableOnDistance()).Start();
        terrainTextureDetector = gameObject.AddComponent<TerrainTextureDetector>();
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
                    randomX = Random.Range(terrainData.bounds.min.x, terrainData.bounds.max.x) + transform.position.x;
                    randomZ = Random.Range(terrainData.bounds.min.z, terrainData.bounds.max.z) + transform.position.z;
                    if (Physics.Raycast(new Vector3(randomX, terrainData.bounds.max.y + 5f, randomZ), -Vector3.up, out hit))
                        if (objectGenerationSettings.objects[i].followLayers)
                        {
                            if (!objectGenerationSettings.objects[i].layers.Contains(terrainTextureDetector.GetDominantTextureIndexAt(hit.point)+1))
                            {
                                return;
                            }
                        }
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

                            bool newObjDestroyed = false;
                            foreach (GameObject obj in generatedObjects)
                            {
                                if (Vector3.Distance(newObject.transform.position, obj.transform.position) < objectGenerationSettings.objects[i].minimumDistance)
                                {
                                    Destroy(newObject);
                                    newObjDestroyed = true;
                                    break;
                                }
                            }

                            if (!newObjDestroyed)
                            {
                                generatedObjects.Add(newObject);
                                newObject.transform.SetParent(transform);
                                var newEulerAngles = new Quaternion().eulerAngles;
                                newEulerAngles.y = Random.Range(0f, 360f);
                                newObject.transform.eulerAngles = newEulerAngles;
                                // newObject.AddComponent<TestCollectable>();
                                currentObjects[objects[i].gameObject]++;
                            }
                        }
                }
            }
        }
    }

    IEnumerator DisableOnDistance()
    {
        while (true) 
        {
            foreach (GameObject obj in generatedObjects)
            {
                if (Vector3.Distance(player.position, obj.transform.position) >= objectGenerationSettings.despawnDistance)
                {
                    obj.SetActive(false);
                }
                else
                {
                    obj.SetActive(true);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}