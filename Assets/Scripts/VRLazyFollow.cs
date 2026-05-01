using UnityEngine;

public class VRLazyFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float distance = 2.0f; // How far in front of the player
    [SerializeField] private float followSpeed = 5.0f; // Smoothness speed
    [SerializeField] private float verticalOffset = -0.5f; // Adjust height

    void Start()
    {
        // Auto-assign the Main Camera if not set
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // 1. Calculate the target position in front of the camera
        Vector3 targetPosition = cameraTransform.position + (cameraTransform.forward * distance);
        targetPosition.y += verticalOffset;

        // 2. Smoothly move the UI to that position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        // 3. Make the UI face the player
        // We use the camera's position but keep the UI's own Up vector for stability
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}

