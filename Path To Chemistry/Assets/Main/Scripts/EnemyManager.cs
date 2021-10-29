using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameObject player;
    private List<GameObject> enemies;
    public int enemyDespawnDistance;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        enemies = new List<GameObject>();
        foreach (Transform child in transform)
        {
            enemies.Add(child.gameObject);
        }
    }

    void Start()
    {
        new Task(EnemyDespawn()).Start();
    }

    IEnumerator EnemyDespawn()
    {
        while (true)
        {
            List<GameObject> newEnemies = enemies;
            foreach (var obj in enemies) {
                if (obj == null)
                {
                    newEnemies.Remove(obj);
                    continue;
                }
                
                if (Vector3.Distance(player.transform.position, obj.transform.position) >= enemyDespawnDistance)
                {
                    obj.SetActive(false);
                }
                else
                {
                    obj.SetActive(true);
                }
            }

            enemies = newEnemies;
            yield return new WaitForSeconds(1f);
        }
    }
}
