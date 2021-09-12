using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Jobs;
using UnityEngine.UI;
using Random = System.Random;

// DO NOT USE, OUTDATED. PLEASE USE METALONENEMY.CS INSTEAD.

public class TestEnemy: MonoBehaviour, IEntity
{
    public int health { get; set; }
    public int damage { get; set; }
    public EntityStates currentState { get; set; }
    public float speed { get; set; }
    public Dictionary<string, int> drops { get; set; }

    public float patrolRange = 10f;
    public Vector2 restTime;
    public int fovRange;
    public int sightRange;
    public int minPlayerDetectDistance;
    public int attackRange;
    private Transform rayOrigin;
    private NavMeshAgent agent;

    private Task patrolTask;
    private Task chaseTask;
    private Task investigateTask;
    private Task attackTask;
    private Task adjustPosTask;
    private List<Task> tasks = new List<Task>();
    
    private Transform player;
    private Vector3 lastSeenPosition;
    private Vector3 patrolPoint;

    //PLES DO NOT COPY THIS, PLEASE COPY METALON ENEMY. THIS IS OLD. (and im lazy to update this kwkwk)
    
    void Awake()
    {
        patrolPoint = transform.position;
        rayOrigin = transform.Find("Raycast origin");
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        agent = GetComponent<NavMeshAgent>();
        health = 100; // Set default health here

        tasks = new List<Task>()
        {
            patrolTask,
            chaseTask,
            investigateTask,
            attackTask
        };
        
        drops = new Dictionary<string, int>()
        {
            
        };
    }

    void Start()
    {
        speed = agent.speed;
        ChangeState(EntityStates.Patrol);
    }
    
    IEnumerator Patrol(Vector3 targetPos)
    {
        // Vector3 targetPos = new Vector3(x, transform.position.y, z);
        // Quaternion targetRot = Quaternion.LookRotation(targetPos - transform.position);
        while (Vector3.Distance(transform.position, targetPos) > 2)
        {
            // transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 3 * Time.deltaTime);
            // StayOnGround();
            Debug.DrawLine(transform.position, targetPos, Color.blue);
            if (agent.destination != targetPos)
            {
                agent.destination = targetPos;
            }
            yield return null;
        }

        yield return new WaitForSeconds(UnityEngine.Random.Range(restTime.x, restTime.y));
    }

    IEnumerator Chase()
    {
        // Quaternion targetRot = Quaternion.LookRotation(player.position - transform.position);
        // transform.position = Vector3.MoveTowards(transform.position, player.position, (speed + speed * 0.2f) * Time.deltaTime);
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10 * Time.deltaTime);
        // StayOnGround();
        agent.speed = speed + speed * 0.4f;
        agent.destination = player.position;
        yield return null;
    }
    
    IEnumerator Investigate()
    {
        // Quaternion targetRot = Quaternion.LookRotation(lastSeenPosition - transform.position);
        while (Vector3.Distance(transform.position, lastSeenPosition) > 2)
        {
            // transform.position = Vector3.MoveTowards(transform.position, lastSeenPosition, speed * Time.deltaTime);
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 3 * Time.deltaTime);
            Debug.DrawLine(transform.position, lastSeenPosition);
            if (agent.destination != lastSeenPosition)
            {
                agent.destination = lastSeenPosition;
            }
            yield return null;
        }

        float startRot = transform.eulerAngles.y;
        float endRot = transform.eulerAngles.y + 360.0f;
        float t = 0f;
        while (t < 5)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRot, endRot, t / 3) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
            yield return null;
        }

        patrolPoint = transform.position;
        ChangeState(EntityStates.Patrol);
    }

    IEnumerator Attack()
    {
        // Attack here
        var playerData = PlayerData.Instance();
        playerData.survivalHealth -= damage;
        yield return new WaitForSeconds(2); // Attack speed
    }

    void Flee()
    {
        
    }
    
    void StayOnGround()
    {
        Ray downwardsRay = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(downwardsRay, out hit, 1.0f))
        {
 
            //You could check hit.collider.transform/tag to make sure
            //you're only trying to stand on the terrain
            transform.position = hit.point;
 
        }
 
    }
    
    public void ChangeState(EntityStates state)
    {
        foreach (Task task in tasks)
        {
            if (task != null)
            {
                print("YEYSJSHKJSNKSN");
                task.Stop();
            }
        }
        switch (state)
        {
            case EntityStates.Patrol:
                agent.speed = speed;
                currentState = EntityStates.Patrol;
                break;
            case EntityStates.Chase:
                agent.speed = speed + speed * 0.4f;
                currentState = EntityStates.Chase;
                break;
            case EntityStates.Investigate:
                agent.speed = speed + speed * 0.2f;
                currentState = EntityStates.Investigate;
                break;
            case EntityStates.Attack:
                currentState = EntityStates.Attack;
                break;
            case EntityStates.Flee:
                currentState = EntityStates.Flee;
                break;
            default:

                break;
        }
    }

    public void Attacked(int damage, float fireRate)
    {
        throw new NotImplementedException();
    }

    public void Attack(int damage, float fireRate)
    {
        return;
    }

    bool CanSeePlayer()
    {
        RaycastHit hit;
        var rayDirection = player.position - transform.position;
        if(Physics.Raycast(rayOrigin.position, rayDirection, out hit)){ // If the player is very close behind the enemy and not in view the enemy will detect the player
            if((hit.transform.tag == "Player") && hit.distance <= minPlayerDetectDistance){
                return true;
            }
        }
        
        if((Vector3.Angle(rayDirection, rayOrigin.forward)) < fovRange){ // Detect if player is within the field of view
            if (Physics.Raycast(transform.position, rayDirection, out hit, fovRange))
            {
                if (hit.transform.tag == "Player" && hit.distance <= sightRange) {
                    return true;
                }
                return false;
            }
        }

        return false;
    }
    
    public Vector3 RandomNavmeshLocation() {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * patrolRange;
        randomDirection += patrolPoint;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRange, 1)) {
            finalPosition = hit.position;            
        }
        return finalPosition;
    }

    void DetermineState()
    {
        bool playerVisible = CanSeePlayer();
        if (!playerVisible && currentState != EntityStates.Chase && currentState != EntityStates.Investigate && currentState != EntityStates.Flee)
        {
            if (currentState != EntityStates.Patrol)
            {
                ChangeState(EntityStates.Patrol);
            }
        }
        else if (!playerVisible && (currentState == EntityStates.Attack || currentState == EntityStates.Chase))
        {
            if (currentState != EntityStates.Investigate)
            {
                lastSeenPosition = player.position;
                ChangeState(EntityStates.Investigate);
            }
        }
        else if (playerVisible && Vector3.Distance(player.position, transform.position) > attackRange)
        {
            if (currentState != EntityStates.Chase)
            {
                ChangeState(EntityStates.Chase);
            }
        }
        else if (playerVisible && Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            if (currentState != EntityStates.Attack)
            {
                ChangeState(EntityStates.Attack);
            }
        }
    }

    void Update()
    {
        var leftRay = Quaternion.Euler(0, fovRange, 0) * rayOrigin.forward;
        var rightRay = Quaternion.Euler(0, -fovRange, 0) * rayOrigin.forward;

        if (currentState == EntityStates.Patrol)
        {
            Debug.DrawRay(transform.position,  leftRay * sightRange, Color.green);
            Debug.DrawRay(transform.position, rightRay * sightRange, Color.green);
        } else if (currentState == EntityStates.Investigate)
        {
            Debug.DrawRay(transform.position,  leftRay * sightRange, Color.yellow);
            Debug.DrawRay(transform.position, rightRay * sightRange, Color.yellow);
        }
        else
        {
            Debug.DrawRay(transform.position,  leftRay * sightRange, Color.red);
            Debug.DrawRay(transform.position, rightRay * sightRange, Color.red);
        }

        DetermineState();
        switch (currentState)
        {
            case EntityStates.Patrol:
                // Keep patrolling the area around you.
                if (!(patrolTask is {Running: true}))
                {
                    Vector3 randomNavMeshLocation = RandomNavmeshLocation();
                    patrolTask = new Task(Patrol(randomNavMeshLocation));
                }
                break;
            case EntityStates.Chase:
                if (!(chaseTask is {Running: true}))
                {
                    chaseTask = new Task(Chase());
                }
                break;
            case EntityStates.Investigate:
                if (!(investigateTask is {Running: true}))
                {
                    investigateTask = new Task(Investigate());
                }
                break;
            case EntityStates.Attack:
                if (!(attackTask is {Running: true}))
                {
                    attackTask = new Task(Attack());
                }
                break;
            case EntityStates.Flee:
                Flee();
                break;
            default:

                break;
        }
    }
}