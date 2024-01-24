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
            // Aseg�rate de que el enemigo est� mirando hacia el objetivo
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



        //Rigidbody rb = Instantiate(projectile, throwPos, Quaternion.identity).GetComponent<Rigidbody>();
        Rigidbody rb = ObjectPoolingManager.Instance.SpawnObject(projectile, throwPos, Quaternion.identity, PoolType.GameObject).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 8f, ForceMode.Impulse);
        rb.AddForce(transform.up * 2f, ForceMode.Impulse);
    }
}
