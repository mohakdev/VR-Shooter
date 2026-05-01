using UnityEngine;

public class AIBotController : MonoBehaviour
{
    public bool isArrested = false; 

    [Header("References")]
    Transform target;
    public Gun gun;
    public Transform firePoint;
    Animator animator;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 8f;
    public float stopDistance = 12f;

    [Header("Combat")]
    public float fireCooldown = 0.2f;

    [Header("AI Realism")]
    public float reactionTime = 0.5f;
    public float aimInaccuracy = 0.05f;
    public float movementPenalty = 0.1f;

    private float lastFireTime;
    private float seenPlayerTime;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;    
    }

    void Update()
    {
        if (target == null || gun == null) return;
        if(!animator) { animator = GetComponent<Animator>(); }
        float distance = Vector3.Distance(transform.position, target.position);

        if(isArrested) { return; }

        HandleMovement(distance);
        HandleRotation();

        if (CheckLineOfSight())
        {
            if (seenPlayerTime == 0f)
                seenPlayerTime = Time.time;

            if (Time.time - seenPlayerTime >= reactionTime)
            {
                AimAtTarget();
                TryShoot(distance);
            }
        }
        else
        {
            seenPlayerTime = 0f;
        }
        if(gun.currentAmmo == 0) { gun.Reload(); }
        if(animator.GetBool("IsRunning") != IsRunning()) { animator.SetBool("IsRunning", IsRunning()); }
    }

    void HandleMovement(float distance)
    {
        Vector3 direction = (target.position - transform.position).normalized;

        if (distance > stopDistance)
        {
            Move(direction);
        }
    }

    void Move(Vector3 direction)
    {
        // Basic obstacle avoidance

        if (Physics.Raycast(transform.position + Vector3.forward, direction, out RaycastHit hit, 3f))
        {
            Debug.LogWarning("Obstacle " + hit.collider.gameObject);
            // try sidestep
            direction = Vector3.Cross(direction, Vector3.up);
        }

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void HandleRotation()
    {
        Vector3 lookDir = target.position - transform.position;
        lookDir.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }

    bool CheckLineOfSight()
    {
        Vector3 direction = (target.position - firePoint.position).normalized;

        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, 100f))
        {
            return hit.transform.root == target.root;
        }

        return false;
    }

    void AimAtTarget()
    {
        Vector3 direction = target.position - firePoint.position;

        float spread = aimInaccuracy;

        if (IsRunning())
            spread += movementPenalty;

        direction += Random.insideUnitSphere * spread;

        firePoint.forward = direction.normalized;
    }

    void TryShoot(float distance)
    {
        if (Time.time - lastFireTime < fireCooldown) return;

        float distanceFactor = distance * 0.01f;

        if (Random.value < 1f - distanceFactor)
        {
            if(!animator.GetBool("IsShooting")){ animator.SetBool("IsShooting", true); }
            gun.TryShoot();
        }

        lastFireTime = Time.time;
    }

    bool IsRunning()
    {
        // crude but effective
        return Vector3.Distance(transform.position, target.position) > stopDistance;
    }
}