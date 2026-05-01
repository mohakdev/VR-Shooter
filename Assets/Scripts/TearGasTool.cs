using UnityEngine;

public class TearGasTool : MonoBehaviour, ITool
{
    public float radius = 5f;
    public float duration = 4f;
    public float damagePerSecond = 5f;
    public LayerMask enemyLayer;

    public void OnPrimaryAction(bool isHeld, bool pressedThisFrame)
    {
        if (!pressedThisFrame) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        foreach (var hit in hits)
        {
            var ai = hit.GetComponentInParent<AIBotController>();
            if (ai != null)
            {
                //ai.ApplyGas(duration, damagePerSecond);
            }
        }
    }
}