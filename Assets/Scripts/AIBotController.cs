using UnityEngine;
using UnityEngine.AI;

public class AIBotController : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public Gun gun;
    public Transform firePoint;

    private NavMeshAgent agent;

    [Header("Combat Settings")]
    public float stopDistance = 12f;
    public float retreatDistance = 5f;
    public float fireCooldown = 0.2f;

    [Header("AI Realism")]
    public float reactionTime = 0.5f;      // delay before shooting
    public float aimInaccuracy = 0.05f;    // base spread
    public float movementPenalty = 0.1f;   // extra spread while moving

    private float lastFireTime;
    private float seenPlayerTime;
    private bool hasLineOfSight;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target == null || gun == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        HandleMovement(distance);
        hasLineOfSight = CheckLineOfSight();

        if (hasLineOfSight)
        {
            AimAtTarget();

            // Start reaction timer
            if (seenPlayerTime == 0f)
                seenPlayerTime = Time.time;

            if (Time.time - seenPlayerTime >= reactionTime)
            {
                TryShoot(distance);
            }
        }
        else
        {
            // Reset reaction if player not visible
            seenPlayerTime = 0f;
        }
    }

    void HandleMovement(float distance)
    {
        if (distance > stopDistance)
        {
            agent.SetDestination(target.position);
        }
        else if (distance < retreatDistance)
        {
            Vector3 dir = (transform.position - target.position).normalized;
            agent.SetDestination(transform.position + dir * 3f);
        }
        else
        {
            agent.ResetPath();
        }
    }

    bool CheckLineOfSight()
    {
        Vector3 direction = (target.position - firePoint.position).normalized;

        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, 100f))
        {
            return hit.transform == target;
        }

        return false;
    }

    void AimAtTarget()
    {
        Vector3 direction = target.position - firePoint.position;

        float spread = aimInaccuracy;

        if (agent.velocity.magnitude > 0.1f)
            spread += movementPenalty;

        direction += Random.insideUnitSphere * spread;

        firePoint.forward = direction.normalized;
    }

    void TryShoot(float distance)
    {
        if (Time.time - lastFireTime < fireCooldown) return;

        // Optional: reduce accuracy with distance
        float distanceFactor = distance * 0.01f;

        if (Random.value < 1f - distanceFactor)
        {
            gun.TryShoot();
        }

        lastFireTime = Time.time;
    }
}