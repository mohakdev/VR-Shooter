using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimation : MonoBehaviour
{
    [SerializeField] private InputActionReference gripActionReference;
    [SerializeField] private InputActionReference triggerActionReference;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject");
        }
    }
    private void Update()
    {
        if(animator != null) { 
            float gripVal = gripActionReference.action.ReadValue<float>();
            float pinchVal = triggerActionReference.action.ReadValue<float>();
            animator.SetFloat("Grip", gripVal);
            animator.SetFloat("Pinch", pinchVal);
        }
        else
        {
            Debug.LogWarning("No Animation Working"); 
            animator = GetComponent<Animator>(); 
        }
    }
}
