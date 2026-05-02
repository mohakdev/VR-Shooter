using UnityEngine;

public class UpdatePlayerBody : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 pos = Camera.main.transform.position;
        transform.position = pos;
    }
}