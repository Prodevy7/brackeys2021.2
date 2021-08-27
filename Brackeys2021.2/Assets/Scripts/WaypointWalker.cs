using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointWalker : MonoBehaviour
{
    [SerializeField] private float walkCooldown = 5f;
    [SerializeField] [Range(0, 95)] private int percentageWalkCooldownMaximumChaos = 75;
    [SerializeField] private float restWalkCooldown = 0f;
    [SerializeField] private Waypoints waypoints;

    private int currentWaypoint;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private PlayerController playerController;


    float GetWalkCooldown()
    {
        float currentCaosPoints = playerController.GetCaosPoints();
        return walkCooldown - (walkCooldown * (percentageWalkCooldownMaximumChaos * currentCaosPoints / 100));
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        agent.autoBraking = true;

        restWalkCooldown = Random.Range(0, walkCooldown);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            agent.enabled = true;
        }
    }

    private void Update()
    {
        if (!rb.useGravity)
        {
            agent.enabled = false;
        }

        if (!waypoints)
            return;

        Walking();
    }

    void Walking()
    {
        if (isDestination())
        {
            restWalkCooldown -= Time.deltaTime;
        }

        if (restWalkCooldown <= 0)
        {
            restWalkCooldown = GetWalkCooldown();
            currentWaypoint = Random.Range(0, waypoints.waypoints.Count);
            agent.destination = waypoints.waypoints[currentWaypoint].transform.position;
        }
    }

    bool isDestination()
    {
        return agent.remainingDistance < 0.5f;
    }
}
