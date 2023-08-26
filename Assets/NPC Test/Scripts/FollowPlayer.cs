using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float radius;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        agent.destination = player.transform.position - radius * VectorToPlayer();
    }
    
    Vector3 VectorToPlayer()
    {
        return Vector3.Normalize(player.transform.position - transform.position);
    }
}
