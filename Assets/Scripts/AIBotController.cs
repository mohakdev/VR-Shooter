using System.Collections;
using UnityEngine;

public class AIBotController : MonoBehaviour
{
    [Header("States")]
    public bool isArrested = false;
    public bool isStunned = false;
    public bool isGassed = false;

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

    private Coroutine stunRoutine;
    private Coroutine gasRoutine;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (target == null || gun == null) return;

        if (isArrested)
        {
            SetAnim("IsArrested", true);
            return;
        }

        if (isStunned)
        {
            SetAnim("IsStunned", true);
            return;
        }
        else
        {
            SetAnim("IsStunned", false);
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);

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

        // Auto reload
        if (gun.currentAmmo == 0)
            gun.Reload();

        SetAnim("IsRunning", IsRunning());
    }

    // ---------------- MOVEMENT ----------------

    void HandleMovement(float distance)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;

        if (distance > stopDistance)
        {
            Move(direction);
        }
    }

    void Move(Vector3 direction)
    {
        // Simple obstacle avoidance
        if (Physics.Raycast(transform.position + Vector3.up, direction, 3f))
        {
            direction = Vector3.Cross(direction, Vector3.up);
        }

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void HandleRotation()
    {
        Vector3 lookDir = target.transform.position - transform.position;
        lookDir.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }

    // ---------------- COMBAT ----------------

    bool CheckLineOfSight()
    {
        Vector3 direction = (target.transform.position - firePoint.position).normalized;

        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, 100f))
        {
            return hit.transform.root == target.root;        
        }

        return false;
    }

    void AimAtTarget()
    {
        Vector3 direction = target.transform.position - firePoint.position;

        float spread = aimInaccuracy;

        if (IsRunning())
            spread += movementPenalty;

        direction += Random.insideUnitSphere * spread;

        firePoint.forward = direction.normalized;
    }

    void TryShoot(float distance)
    {
        if (isGassed) return;

        if (Time.time - lastFireTime < fireCooldown) return;

        float distanceFactor = distance * 0.01f;

        if (Random.value < 1f - distanceFactor)
        {
            SetAnim("IsShooting", true);
            gun.TryShoot();
        }

        lastFireTime = Time.time;
    }

    bool IsRunning()
    {
        return Vector3.Distance(transform.position, target.transform.position) > stopDistance;
    }

    // ---------------- EFFECTS ----------------

    public void ApplyStun(float duration)
    {
        if (isArrested) return;

        if (stunRoutine != null)
            StopCoroutine(stunRoutine);

        stunRoutine = StartCoroutine(StunRoutine(duration));
    }

    IEnumerator StunRoutine(float duration)
    {
        isStunned = true;

        yield return new WaitForSeconds(duration);

        isStunned = false;
    }

    public void ApplyGas(float duration, float dps)
    {
        if (isArrested) return;

        if (gasRoutine != null)
            StopCoroutine(gasRoutine);

        gasRoutine = StartCoroutine(GasRoutine(duration, dps));
    }

    IEnumerator GasRoutine(float duration, float dps)
    {
        isGassed = true;

        float timer = 0f;
        var health = GetComponent<Health>();

        while (timer < duration)
        {
            if (health != null)
            {
                health.TakeDamage(dps * Time.deltaTime);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        isGassed = false;
    }

    public void Arrest()
    {
        if (isArrested) return;

        isArrested = true;
        print("Arrested Enemy");
        // Stop everything
        isStunned = false;
        isGassed = false;

        if (stunRoutine != null) StopCoroutine(stunRoutine);
        if (gasRoutine != null) StopCoroutine(gasRoutine);

        SetAnim("IsArrested", true);
    }

    // ---------------- UTIL ----------------

    void SetAnim(string param, bool value)
    {
        if (!animator) return;

        if (animator.GetBool(param) != value)
            animator.SetBool(param, value);
    }
}