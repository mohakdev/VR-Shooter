using UnityEngine;

public class ArrestTool : MonoBehaviour, ITool
{
    public float range = 3f;
    public LayerMask enemyLayer;

    public void OnPrimaryAction(bool isHeld, bool pressedThisFrame)
    {
        if (!pressedThisFrame) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, range, enemyLayer);

        foreach (var hit in hits)
        {
            var ai = hit.GetComponentInParent<AIBotController>();

            if (ai != null && !ai.isArrested)
            {
                //ai.Arrest();
                return;
            }
        }
    }
}