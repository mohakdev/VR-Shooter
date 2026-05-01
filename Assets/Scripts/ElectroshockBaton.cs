using UnityEngine;

public class ElectroshockBaton : MonoBehaviour, ITool
{
    public float stunDuration = 2f;

    public void OnPrimaryAction(bool isHeld, bool pressedThisFrame)
    {
        if (!isHeld) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, 1.5f);

        foreach (var hit in hits)
        {
            var ai = hit.GetComponentInParent<AIBotController>();
            if (ai != null)
            {
                //ai.ApplyStun(stunDuration);
            }
        }
    }
}