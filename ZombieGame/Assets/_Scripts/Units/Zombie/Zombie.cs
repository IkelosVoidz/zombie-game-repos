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

    int state = SPAWN;
    int life = 100;

    [SerializeField] NavMeshAgent navMeshAgent;     
    [SerializeField] GameObject target;
    [SerializeField] Animator animator;

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

    }

    private void Die()
    {

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

    //Se supone que cuando el player dispara y hacierta desde el 
    //player se obtiene el zombie acertado y se llama el metodo
    //DecreaseLife() que se le passa por parametro el daño de
    //la arma equipada (por si en algun momento metemos más
    //armas)
    public void DecreaseLife(int damage)
    {
        life -= damage;
        if (life > 0)
            state = TAKE_DAMAGE;
        else
            state = DIE;
    }


}
