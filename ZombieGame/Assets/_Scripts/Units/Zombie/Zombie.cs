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
    const int DANCE = 5;

    /* IGNORAR AIXO :D
    const float IDLE = 0;
    const float WALK = 0.5f;
    const float RUN = 1;
    */

    [SerializeField]int state = 100;

    [SerializeField] NavMeshAgent navMeshAgent;     
    [SerializeField] GameObject target;
    [SerializeField] Animator animator;
    [SerializeField] HealthComponent healthComponent;

    private float maxSpeed;

    //float style = IDLE;

    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = navMeshAgent.speed;
        float time = Random.Range(2.0f, 5.0f);
        Debug.Log(time);
        Invoke("ToChase", time);
    }

    // Update is called once per frame
    void Update()
    {   
        switch (state)
        {
            case SPAWN:
                
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
            case DANCE:
                Dance();
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

    private void Dance()
    {
        navMeshAgent.speed = 0;
        animator.Play("Macarena_Dance");
    }

    private void ToChase()
    {
        navMeshAgent.speed = maxSpeed;
        state = CHASE;
    }

    //Detecta si esta el Player  davant per atacarlo
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            //state = ATTACK;
        }
    }

    //Detecta si el Player deixa d'estar a davant
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            //state = CHASE;
        }
    }

    //----------------------[Public Methods]--------------------------

    //Metodes per els events
    public void ChangeStateTo(int _state)
    {
        state = _state;
    }

    public void FootStepEvent()
    {
        Debug.Log("Pasiot Zombie");
        if (navMeshAgent.speed > 0)
            navMeshAgent.speed = 0;
        else
            navMeshAgent.speed = maxSpeed;
    }
}
