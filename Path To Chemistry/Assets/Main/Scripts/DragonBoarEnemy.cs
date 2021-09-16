using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProceduralToolkit;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Jobs;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DragonBoarEnemy: MonoBehaviour, IEntity
{
    public int health { get; set; }
    public int damage { get; set; }
    public EntityStates currentState { get; set; }
    public float speed { get; set; }
    public Dictionary<string, int> drops { get; set; }

    public float patrolRange = 40f;
    public Vector2 restTime = new Vector2(1, 3);
    public int fovRange = 30;
    public int sightRange = 30;
    public int minPlayerDetectDistance = 20;
    public int attackRange = 5;
    
    private Transform rayOrigin;
    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;
    private Task patrolTask;
    private Task chaseTask;
    private Task investigateTask;
    private Task attackTask;
    private Task adjustPosTask;
    private Task attackedTask;
    private Task fleeTask;
    private Task deadTask;
    private List<Task> tasks = new List<Task>();
    private Transform player;
    private Vector3 lastSeenPosition;
    private Vector3 patrolPoint;
    private float lastYRotation;

    void Awake()
    {
        // Assign health and damage here
        health = 100;
        damage = 10;
    }
    
    private void Load()
    {
        var filePath = Path.Combine(Application.dataPath, "Elements.json");
        var fileContent = File.ReadAllText(filePath);
        var elementData = JsonConvert.DeserializeObject<ElementData>(fileContent);
        ElementData.Instance().UpdateElementData(elementData);
    }
    
    private void Save()
    {
        print(Application.persistentDataPath);
        var playerData = PlayerData.Instance();
        var directory = $"{Application.persistentDataPath}/Data";
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        var Settings = new JsonSerializerSettings();
        Settings.Formatting = Formatting.Indented;
        Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        var Json = JsonConvert.SerializeObject(playerData, Settings);
        var filePath = Path.Combine(directory, "Saves.json");
        File.WriteAllText(filePath, Json);
    }

    void Start()
    {
        var elementData = ElementData.Instance();

        patrolPoint = transform.position;
        rayOrigin = transform.Find("Raycast origin");
        health = Random.Range(100, 150);
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //TODO: Pls load a nice sound here for enemy when hit.
        // audioSource.clip = Resources.Load<AudioClip>("Sounds/Nya");

        // Whatever elements you want this enemy to drop, you put it here. You could even make it
        // drop random elements if you want.
        
        //TODO: Change this thing
        drops = new Dictionary<string, int>()
        {
            {elementData.elements.ElementAt(1).Key, Random.Range(1,100)},
            {elementData.elements.ElementAt(2).Key, Random.Range(1,100)},
            {elementData.elements.ElementAt(3).Key, Random.Range(1,100)},
            {elementData.elements.ElementAt(4).Key, Random.Range(1,100)},
            {elementData.elements.ElementAt(5).Key, Random.Range(1,100)}
        };
        
        speed = agent.speed;
        ChangeState(EntityStates.Patrol);
    }

    public void Attacked(int damage)
    {
        if (!(attackedTask is {Running: true}))
        {
            attackedTask = new Task(AttackedCoroutine(damage));
        }
    }

    IEnumerator AttackedCoroutine(int damage)
    {
        health -= damage;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        yield return null;
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
                DisableAllAnimations();
                animator.SetBool("Walk", true);
                agent.destination = targetPos;
            }
            yield return null;  
        }
        
        DisableAllAnimations();
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
        while (Vector3.Distance(transform.position, lastSeenPosition) > 5)
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
        agent.destination = transform.position;
        PlayerData.Instance().survivalHealth -= damage;
        print(PlayerData.Instance().survivalHealth);
        yield return new WaitForSeconds(2); //Attack speed
    }

    IEnumerator Flee()
    {
        Vector3 runTo = transform.position + ((transform.position = player.position) * 30);
        float distance = Vector3.Distance(transform.position, player.position);
        while (distance > 10)
        {
            agent.destination = runTo;
            yield return null;
        }
    }

    IEnumerator Dead()
    {
        DisableAllAnimations();
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(2f);
        var playerData = PlayerData.Instance();
        foreach (var elementDrop in drops)
        {
            playerData.Inventory[elementDrop.Key] += elementDrop.Value;
            // print("Added " + elementDrop.Value + " to " + elementDrop.Key);
        }

        Save();
        Destroy(gameObject);
        yield return null;
    }

    public void ChangeState(EntityStates state)
    {
        if (patrolTask != null) patrolTask.Stop();
        if (chaseTask != null) chaseTask.Stop();
        if (investigateTask != null) investigateTask.Stop();
        if (attackTask != null) attackTask.Stop();
        
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

    bool CanSeePlayer()
    {
        RaycastHit hit;
        var rayDirection = player.position - transform.position;
        if(Vector3.Distance(transform.position, player.position) <= minPlayerDetectDistance){ // If the player is too close to the enemy
            return true;
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

    void DisableAllAnimations()
    {
        foreach(AnimatorControllerParameter parameter in animator.parameters) {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(parameter.name, false); 
            }
        }
    }

    void DetermineState()
    {
        bool playerVisible = CanSeePlayer();
        // Flee if health less than 15%
        if (health <= health * 0.15f)
        {
            if (currentState != EntityStates.Flee)
            {
                DisableAllAnimations();
                animator.SetBool("Run", true);
                ChangeState(EntityStates.Flee);
            }
        }
        // Patrol in normal circumstances
        else if (!playerVisible && currentState != EntityStates.Chase && currentState != EntityStates.Investigate && currentState != EntityStates.Flee)
        {
            if (currentState != EntityStates.Patrol)
            {
                ChangeState(EntityStates.Patrol);
            }
        }
        // Investigate if a player goes missing after seeing them
        else if (!playerVisible && (currentState == EntityStates.Attack || currentState == EntityStates.Chase))
        {
            if (currentState != EntityStates.Investigate)
            {
                lastSeenPosition = player.position;
                DisableAllAnimations();
                animator.SetBool("Walk", true);
                ChangeState(EntityStates.Investigate);
            }
        }
        // Chase a player if you see them
        else if (playerVisible && Vector3.Distance(player.position, transform.position) > attackRange)
        {
            if (currentState != EntityStates.Chase)
            {
                DisableAllAnimations();
                animator.SetBool("Run", true);
                ChangeState(EntityStates.Chase);
            }
        }
        // Attack a player if they are in attack range
        else if (playerVisible && Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            if (currentState != EntityStates.Attack)
            {
                DisableAllAnimations();
                animator.SetBool("Basic Attack", true);
                ChangeState(EntityStates.Attack);
            }
        }
    }

    void Update()
    {
        // If too far from player then don't do anything.
        if (Vector3.Distance(player.position, transform.position) > 500) return;
        
        // Die if dead
        if (health <= 0)
        {
            if (!(deadTask is {Running: true}))
            {
                deadTask = new Task(Dead());
            }
            return;
        }
        
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
                if (!(fleeTask is {Running: true}))
                {
                    fleeTask = new Task(Flee());
                }
                break;
            default:

                break;
        }

        // if (lastYRotation != transform.rotation.eulerAngles.y)
        // {
        //     if (lastYRotation > transform.rotation.eulerAngles.y)
        //     {
        //         animator.SetTrigger("Turn Left");
        //     }
        //     else
        //     {
        //         animator.SetTrigger("Turn Right");
        //     }
        // }
        // lastYRotation = transform.rotation.eulerAngles.y;
    }
}