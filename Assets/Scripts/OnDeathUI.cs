using UnityEngine;
public class OnDeathUI : MonoBehaviour
{
    public Health playerHealth;
    public GameObject deathScreen;
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }
    void Update()
    {
        if(playerHealth.currentHealth <= 0)
        {
            Time.timeScale = 0;
            deathScreen.SetActive(true);
        }
    }
}
