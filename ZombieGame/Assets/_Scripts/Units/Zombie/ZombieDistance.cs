using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDistance : Zombie
{
    private float timeBetweenAttacks;
    private bool alreadyAttacked;

    private float attackRange = 7f;
    private bool playerInAttackRange;
    private bool transitionStarted = false;
    [SerializeField] GameObject projectile;

    [SerializeField] LayerMask whatIsGround, whatIsPlayer;

    private bool isShooting = false;

    protected override void Update()
    {

        base.Update();

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (playerInAttackRange && (state != BUSY || isWalking))
            state = ATTACK;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK" + 6))
        {
            // Asegúrate de que el enemigo esté mirando hacia el objetivo
            transform.LookAt(target.transform);
        }

    }

    public override void Attack()
    {
        state = BUSY;
        isShooting = true;
        navMeshAgent.speed = 0;
        animator.CrossFade("ATTACK6", 0.2f, -1, 0);
    }

    public override void HitAnimEvent()
    {
        base.HitAnimEvent();
        Vector3 throwPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        transform.LookAt(target.transform);
        Debug.Log("Forward Direction: " + transform.forward);

        Rigidbody rb = Instantiate(projectile, throwPos, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 8f, ForceMode.Impulse);
        rb.AddForce(transform.up * 2f, ForceMode.Impulse);

    }

    IEnumerator goDown()
    {
        float finalValue = navMeshAgent.baseOffset - 0.8f;
        bool transition = false;
        while (!transition)
        {
            navMeshAgent.baseOffset = Mathf.Lerp(navMeshAgent.baseOffset, finalValue, 0.7f * Time.deltaTime);

            Debug.Log(navMeshAgent.baseOffset);

            if (Mathf.Approximately(navMeshAgent.baseOffset, finalValue))
            {
               transition = true;
            }

            yield return null;
        }
    }

}
