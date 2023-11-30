using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDistance : Zombie
{
    private float timeBetweenAttacks;
    private bool alreadyAttacked;

    private float attackRange = 5f;
    private bool playerInAttackRange;
    [SerializeField] GameObject projectile;

    [SerializeField] LayerMask whatIsGround, whatIsPlayer;

    protected override void Update()
    {

        base.Update();

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (playerInAttackRange)
        {
            if(state!=BUSY)Attack();
            Debug.Log("ESTA DENTRO XAVALE");
        }
        else
            Debug.Log("ESTA FUERA XAVALE");


    }

    public override void Attack()
    {
        navMeshAgent.speed = 0;
        animator.Play("ATTACK" + Random.Range(1, 6));
        state = BUSY;

        Vector3 throwPos = new Vector3(transform.position.x, transform.position.y+1,transform.position.z);
        transform.LookAt(target.transform);

        Rigidbody rb = Instantiate(projectile,throwPos,Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 15f, ForceMode.Impulse);
        rb.AddForce(transform.up * 5f, ForceMode.Impulse);
        
    }

}
