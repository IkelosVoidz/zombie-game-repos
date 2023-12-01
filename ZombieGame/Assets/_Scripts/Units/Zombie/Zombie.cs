using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Zombie : MonoBehaviour
{
    protected const int SPAWN = 0;
    protected const int CHASE = 1;
    protected const int ATTACK = 2;
    protected const int TAKE_DAMAGE = 3;
    protected const int DIE = 4;
    protected const int DANCE = 5;
    protected const int BUSY = 10;

    public int testState; //Fer Proves

    protected int state = 100;

    //Hola ferran, he posat algunes coses en public pq en [SerializeField] es accesible desde el inspector pero no desde les clases que penjen.
    //Protected es accesibler desde les clases q penjen pero no desde el inspector. Asi que he puesto public hasta q encontremos una solucion mas elengante.
    public NavMeshAgent navMeshAgent;
    public GameObject target;
    public Animator animator;
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] BoxCollider AttackTrigger;

    private bool hit = false;
    private bool isAttacking = false;
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
    protected virtual void Update()
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
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !dead)
        {
            if(hit && isAttacking)
            {
                Debug.Log("Ha fet pupa!!");
                isAttacking = false;
            }
            if(!hit && !isAttacking)
            {
                state = ATTACK;
                isAttacking = true;
            }
        }
    }



    //Detecta si el Player deixa d'estar a davant
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !dead)
        {
            state = CHASE;
            hit = false;
            isAttacking = false;
        }
    }

    //----------------------[Public Methods]--------------------------
    public void Chase()
    {
        navMeshAgent.destination = target.transform.position;
        animator.Play("Zombie_Walk");
    }

    virtual public void Attack()
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
        //Debug.Log("Pasiot Zombie");
        if (navMeshAgent.speed > 0)
            navMeshAgent.speed = 0;
        else
            navMeshAgent.speed = maxSpeed;
    }

    public void FinishAnimEvent()
    {
        if (!dead)
        {
            isAttacking = false;
            ToChase();
        }
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
