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

    protected override void Update()
    {

        base.Update();

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (playerInAttackRange && state != BUSY)
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

    public override void Die()
    {
        base.Die();
        //this.transform.position = new Vector3(transform.position.x,-10.83f,transform.position.z);
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        //navMeshAgent.height = 0.1f;

        //capsuleCollider.height = 0.1f;
        if(capsuleCollider!=null)
            Destroy(capsuleCollider);

        //ESTO ES UN ASCO ME QUIERO MORIR NO SE EN QUE MOMENTO DIJE QUE SERIA BUENA IDEA METERME A UNA CARRERA DE PROGRAMACION. MAMA QUIERO SER CAMAREERO.

        //float initialValue = navMeshAgent.baseOffset;
        //float finalValue = navMeshAgent.baseOffset - 0.8f;

        //navMeshAgent.baseOffset = Mathf.Lerp(navMeshAgent.baseOffset, finalValue, 0.1f * Time.deltaTime);

        //Debug.Log(navMeshAgent.baseOffset);

        //if (!transitionStarted)
        //{
        //    transitionStarted = true;
        //    StartCoroutine(goDown());
        //}

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
