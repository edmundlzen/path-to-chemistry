using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProceduralToolkit;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Jobs;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MetalonEnemy: MonoBehaviour, IEntity
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
    
    private NotificationsController notificationsController;
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
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnDisable()
    {
        if (patrolTask != null) patrolTask.Stop();
        if (chaseTask != null) chaseTask.Stop();
        if (investigateTask != null) investigateTask.Stop();
        if (attackTask != null) attackTask.Stop();
    }

    void Start()
    {
        StartCoroutine(Roar());
        notificationsController = GameObject.FindGameObjectsWithTag("NotificationsController")[0].transform.GetComponent<NotificationsController>();
        var elementData = ElementData.Instance();

        patrolPoint = transform.position;
        rayOrigin = transform.Find("Raycast origin");
        health = Random.Range(100, 150);
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //TODO: Pls load a nice sound here for enemy when hit.
        // audioSource.clip = Resources.Load<AudioClip>("Sounds/Nya");

        // Whatever elements you want this enemy to drop, you put it here. You could even make it
        // drop random elements if you want. 
        drops = new Dictionary<string, int>();

        while (drops.Count < 10)
        {
            string randomElement = elementData.elements.ElementAt(Random.Range(0, 117)).Key;
            if (!drops.ContainsKey(randomElement))
            {
                drops.Add(randomElement, Random.Range(5,10));
            }
        }
        
        speed = agent.speed;
        ChangeState(EntityStates.Patrol);
    }
    
    IEnumerator Roar()
    {
        while (true)
        {
            if (!audioSource.isPlaying && agent.remainingDistance != 0f)
            {
                audioSource.clip = Resources.Load<AudioClip>("Sounds/Crawl_Sound");
                audioSource.Play();
            }

            yield return null;
        }
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
                animator.SetBool("Walk Forward", true);
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
        if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, 5f, 1))
        {
            agent.destination = hit.position;
        }
        yield return null;
    }
    
    IEnumerator Investigate()
    {
        if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, 5f, 1))
        {
            lastSeenPosition = hit.position;
        }
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

        patrolPoint = lastSeenPosition;
        ChangeState(EntityStates.Patrol);
    }

    IEnumerator Attack()
    {
        // Attack here
        audioSource.clip = Resources.Load<AudioClip>("Sounds/Hiss_" + Random.Range(1,3));
        audioSource.Play();
        agent.destination = transform.position;
        PlayerData.Instance().survivalHealth -= damage;
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
        while (true)
        {
            yield return new WaitForSeconds(4f);
            var playerData = PlayerData.Instance();
            foreach (var elementDrop in drops)
            {
                notificationsController.SendTextImageNotification(elementDrop.Key, "+ " + elementDrop.Value, "green");
                playerData.Inventory[elementDrop.Key] += elementDrop.Value;
            }

            Destroy(gameObject);
            yield break;
        }
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
                animator.SetBool("Run Forward", true);
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
                animator.SetBool("Walk Forward", true);
                ChangeState(EntityStates.Investigate);
            }
        }
        // Chase a player if you see them
        else if (playerVisible && Vector3.Distance(player.position, transform.position) > attackRange)
        {
            if (currentState != EntityStates.Chase)
            {
                DisableAllAnimations();
                animator.SetBool("Run Forward", true);
                ChangeState(EntityStates.Chase);
            }
        }
        // Attack a player if they are in attack range
        else if (playerVisible && Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            if (currentState != EntityStates.Attack)
            {
                DisableAllAnimations();
                animator.SetTrigger("Stab Attack");
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