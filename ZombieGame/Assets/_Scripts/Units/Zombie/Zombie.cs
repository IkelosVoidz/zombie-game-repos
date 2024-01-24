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
    [SerializeField] CapsuleCollider Collider;

    private bool hit = false;
    private bool isAttacking = false;
    private bool dead = false;
    private float maxSpeed;
    protected bool isWalking = false;


    [SerializeField] SpriteRenderer sr;

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

        if (isWalking && target != null)
            navMeshAgent.destination = target.transform.position;

        if ((state != BUSY) && !dead)
        {
            if (state != CHASE)
                isWalking = false;
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Romper");
    }

    private void OnTriggerStay(Collider other)
    {
        //Aixo s'aguanta d'un fil nois a la que canviem com funciona el player o les portes aixo peta espectacularment

        if ((other.gameObject.name == "PlayerObj" || (other.CompareTag("Door") && !other.gameObject.GetComponent<DoorController>().IsOpen())) && !dead)
        {
            if (hit && isAttacking)
            {
                Debug.Log("Ha fet pupa!!");
                HealthComponent HC = other.GetComponentInParent<HealthComponent>();
                if (HC is not null)
                {
                    Debug.Log("Daño al jugador");
                    HC.TakeDamage(5, new Vector3());
                }
                isAttacking = false;
            }
            if (!hit && !isAttacking)
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

    public bool isDead()
    {
        return dead;
    }
    public void Chase()
    {
        if (target != null)
            navMeshAgent.destination = target.transform.position;
        state = BUSY;

        //Debug.Log(isWalking);
        if (!isWalking)
        {
            isWalking = true;
            animator.CrossFade("Zombie_Walk", 0.2f, -1, 0);
        }
        else
        {
            isWalking = true;
            animator.Play("Zombie_Walk");
        }
    }

    virtual public void Attack()
    {
        state = BUSY;
        navMeshAgent.speed = 0;
        //animator.Play("ATTACK" + Random.Range(1, 6));
        animator.CrossFade("ATTACK" + Random.Range(1, 6), 0.2f, -1, 0);
    }
    public void TakeDamage()
    {
        if (!dead)
        {
            navMeshAgent.speed = 0;
            state = BUSY;
            //animator.Play("GET_HIT" + Random.Range(1, 3));
            animator.CrossFade("GET_HIT" + Random.Range(1, 3), 0.2f, -1, 0);
        }

    }

    virtual public void Die()
    {
        if (!dead)
        {
            //Animar Muerte
            state = BUSY;
            navMeshAgent.speed = 0;
            //animator.Play("DIE" + Random.Range(1, 6));
            animator.CrossFade("DIE" + Random.Range(1, 6), 0.2f, -1, 0);
            dead = true;
            //Debug.Log("Diablo mami me mato");
            Collider.enabled = false;


            FadeAndDestroyMesh[] destroyMesh = GetComponentsInChildren<FadeAndDestroyMesh>();
            sr.enabled = false;

            foreach (FadeAndDestroyMesh m in destroyMesh)
            {
                m.StartFade(5.0f, 3.0f);
            }

            Destroy(this.gameObject, 5 + 3); //chapuza, pero me estoy quedando sin tiempo
        }
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

    virtual public void HitAnimEvent()
    {
        hit = !hit;
    }
}
