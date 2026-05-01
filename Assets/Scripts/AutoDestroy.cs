using System.Collections;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(SelfDestruct), 0.2f);
    }

    void SelfDestruct()
    {
        print("Particle Pos "+ transform.position);
        Destroy(gameObject);
    }
}
