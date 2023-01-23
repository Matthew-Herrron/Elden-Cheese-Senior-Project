using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public float lookRadius = 90f;
    public Transform target;
    NavMeshAgent agent;
    Animator animator;
    bool isAttackin;
    void Start()
    {

        animator = GetComponent<Animator>();
        //target = playerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttacking", false);
        isAttackin = false;
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsAttacking", false);
            isAttackin = false;
            if (distance <= 30f)
            {
                // attack animation
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsAttacking", true);
                isAttackin = true;

            }
        }
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}