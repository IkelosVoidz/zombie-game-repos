using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Zombie : MonoBehaviour
{
    const int SPAWN = 0;
    const int CHASE = 1;
    const int ATTACK = 2;
    const int TAKE_DAMAGE = 3;
    const int DIE = 4;
    const int DANCE = 5;
    const int BUSY = 10;

    public int testState; //Fer Proves

    [SerializeField] int state = 100;

    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] GameObject target;
    [SerializeField] Animator animator;
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] BoxCollider AttackTrigger;

    private bool hit = false;
    private bool dead = false;
    private float maxSpeed;

    //----------------------[Private Methods]--------------------------//

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
        //Debug.Log(state);
        if (state != BUSY)
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

            //Aixo es per fer test desde l'inspector per veure les animacions
            switch (testState)
            {
                case 1:
                    state = DIE;
                    break;
                case 2:
                    //state = TAKE_DAMAGE;
                    break;
                case 3:
                    state = ATTACK;
                    break;
            }
            testState = 100;
        }
    }

    //Detecta si esta el Player  davant per atacarlo
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER") && !dead)
        {
            if (hit)
            {
                Debug.Log("Ha fet pupa!!");
            }
            else
            {
                state = ATTACK;
            }
            
        }
    }

    //Detecta si el Player deixa d'estar a davant
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLAYER") && !dead)
        {
            state = CHASE;
        }
    }

    //----------------------[Public Methods]--------------------------
    public void Chase()
    {
        navMeshAgent.destination = target.transform.position;
        animator.Play("Zombie_Walk");
    }

    public void Attack()
    {
        navMeshAgent.speed = 0;
        animator.Play("ATTACK" + Random.Range(1, 6));
        state = BUSY;
    }
    public void TakeDamage()
    {
        navMeshAgent.speed = 0;
        animator.Play("GET_HIT" + Random.Range(1, 3));
        state = BUSY;
    }

    public void Die()
    {
        //Animar Muerte
        navMeshAgent.speed = 0;
        animator.Play("DIE" + Random.Range(1, 6));
        dead = true;
        state = BUSY;
    }

    public void Dance()
    {
        navMeshAgent.speed = 0;
        animator.Play("Macarena_Dance");
    }

    public void ToChase()
    {
        navMeshAgent.speed = maxSpeed;
        state = CHASE;
    }

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

    public void FinishAnimEvent()
    {
        if (!dead)
            ToChase();
        else
        {
            //Se queda muerto un saludo
        }
    }

    public void HitAnimEvent()
    {
        hit = !hit;
    }
}
