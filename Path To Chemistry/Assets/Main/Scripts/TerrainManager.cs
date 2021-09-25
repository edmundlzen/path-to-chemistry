using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    private GameObject player;
    private List<GameObject> terrains;
    public int terrainDespawnDistance;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        terrains = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Terrain"))
            {
                terrains.Add(child.gameObject);
            }
        }
    }

    void Start()
    {
        new Task(TerrainDespawn()).Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TerrainDespawn()
    {
        while (true)
        {
            print(Vector3.Distance(player.transform.position, terrains[0].transform.position));
            foreach (var obj in terrains) {
                if (Vector3.Distance(player.transform.position, obj.transform.position) >= terrainDespawnDistance)
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
