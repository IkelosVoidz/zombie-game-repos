using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDistance : Zombie
{
    private float timeBetweenAttacks;
    private bool alreadyAttacked;

    private float attackRange = 7f;
    private bool playerInAttackRange;
    [SerializeField] GameObject projectile;

    [SerializeField] LayerMask whatIsGround, whatIsPlayer;

    protected override void Update()
    {

        base.Update();

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (playerInAttackRange && state != BUSY)
            state = ATTACK;



    }

    public override void Attack()
    {
        state = BUSY;
        navMeshAgent.speed = 0;
        animator.Play("ATTACK" + Random.Range(6, 6));

        
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

}
