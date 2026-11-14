using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState { GoToBase, AttackBase, ChasePlayer, AttackPlayer }
    public EnemyState currentState;
    public Sight sightSensor;
    public Transform baseTransform;
    public float baseAttackDistance;
    public float playerAttackDistance;
    private NavMeshAgent agent;

    public GameObject bulletPrefab;
    public GameObject shootPoint;
    public float fireRate;
    public float lastShootTime;
    public ParticleSystem muzzleEffect;
    public AudioSource shootSound;
    Animator animator;

    void Awake()
    {
        baseTransform = GameObject.Find("BaseHeart").transform;
        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        if (currentState == EnemyState.GoToBase) { GoToBase(); }
        else if (currentState == EnemyState.AttackBase) { AttackBase(); }
        else if (currentState == EnemyState.ChasePlayer) { ChasePlayer(); }
        else { AttackPlayer(); }
    }

    void GoToBase()
    {
        animator.SetBool("Shooting", false);
        if (agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.SetDestination(baseTransform.position);
        }

        if (sightSensor.detectedObject != null)
        {
            currentState = EnemyState.ChasePlayer;
        }

        float distanceToBase = Vector3.Distance(transform.position, baseTransform.position);

        if (distanceToBase < baseAttackDistance)
        {
            currentState = EnemyState.AttackBase;
        }
    }

    void AttackBase()
    {
        agent.isStopped = true;
        LookTo(baseTransform.position);
        Shoot();
    }

    void ChasePlayer()
    {
        animator.SetBool("Shooting", false);
        if (sightSensor.detectedObject == null)
        {
            currentState = EnemyState.GoToBase;
            return;
        }
        if (agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.SetDestination(sightSensor.detectedObject.transform.position);
        }

        float distanceToPlayer = Vector3.Distance(transform.position, sightSensor.detectedObject.transform.position);

        if (distanceToPlayer <= playerAttackDistance)
        {
            currentState = EnemyState.AttackPlayer;
        }
    }

    void AttackPlayer()
    {
        agent.isStopped = true;

        if (sightSensor.detectedObject == null)
        {
            currentState = EnemyState.GoToBase;
            return;
        }

        LookTo(sightSensor.detectedObject.transform.position);
        Shoot();

        float distanceToPlayer = Vector3.Distance(transform.position, sightSensor.detectedObject.transform.position);

        if (distanceToPlayer > playerAttackDistance * 1.1f)
        {
            currentState = EnemyState.ChasePlayer;
        }
    }

    void Shoot()
    {
        animator.SetBool("Shooting", true);
        var timeSinceLastShoot = Time.time - lastShootTime;
        if (timeSinceLastShoot > fireRate && Time.timeScale > 0)
        {
            lastShootTime = Time.time;
            Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
            muzzleEffect.Play();
            shootSound.Play();
        }
    }

    void LookTo(Vector3 targetPosition)
    {
        Vector3 directionToPosition = Vector3.Normalize(targetPosition - transform.parent.position);
        directionToPosition.y = 0;
        transform.parent.forward = directionToPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerAttackDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, baseAttackDistance);
    }
}
