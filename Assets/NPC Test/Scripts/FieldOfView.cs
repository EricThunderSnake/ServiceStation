using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] [Range(0,360)] float angle;
    [SerializeField] float waitTime = 0.2f;


    GameObject player;
    [SerializeField] LayerMask targetMask, obstructionMask;

    [SerializeField] bool canSeePlayer;

    public float GetRadius() { return radius; }
    public float GetAngle() { return angle; }
    
    public bool CanSeePlayer() { return canSeePlayer; }
    public GameObject Player() { return player; }

    


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(waitTime);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else 
                    canSeePlayer = false;

            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}
