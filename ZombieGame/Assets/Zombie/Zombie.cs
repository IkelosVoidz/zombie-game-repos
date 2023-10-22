using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Zombie : MonoBehaviour
{
    const int SPAWN = 0;
    const int CHASE = 1;
    const int ATTACK = 2;
    const int TAKE_DAMAGE = 3;
    const int DIE = 4;

    /* IGNORAR AIXO :D
    const float IDLE = 0;
    const float WALK = 0.5f;
    const float RUN = 1;
    */

    [SerializeField]int state = SPAWN;

    [SerializeField] NavMeshAgent navMeshAgent;     
    [SerializeField] GameObject target;
    [SerializeField] Animator animator;

    [SerializeField] HealthComponent healthComponent;

    //float style = IDLE;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case SPAWN:
                state = CHASE;
                break;
            case CHASE:
                Chase();
                break;
            case ATTACK:
                Attack();
                break;
            case TAKE_DAMAGE:
                TakeDamage();
                break;
            case DIE:
                Die();
                break;
        }
    }

    //----------------------[Private Methods]--------------------------//
    private void Chase()
    {
        navMeshAgent.destination = target.transform.position;
        animator.Play("Zombie_Walk");
    }

    private void Attack()
    {

    }
    private void TakeDamage()
    {
        //Animar TakeDamage
    }

    private void Die()
    {
        //Animar Muerte
    }

    //Detecta si esta el Player  davant per atacarlo
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            state = ATTACK;
        }
    }

    //Detecta si el Player deixa d'estar a davant
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            state = CHASE;
        }
    }

    //----------------------[Public Methods]--------------------------

    //Metodes per els events
    public void ChangeStateTo(int _state)
    {
        state = _state;
    }
}
