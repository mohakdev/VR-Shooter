using UnityEngine;
using System;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("References")]
    public Slider healthSlider;
    public ParticleSystem bloodEffect;
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Debug")]
    public bool destroyOnDeath = true;

    public event Action<float> OnDamage;
    public event Action OnDeath;

    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;
        if (bloodEffect) {
            Instantiate(bloodEffect, transform.position + Vector3.up, Quaternion.identity).GetComponent<ParticleSystem>().Play();
        }
        OnDamage?.Invoke(amount);
        if(GetComponent<AIBotController>() != null && GetComponent<AIBotController>().isArrested) { MainMenuScript.TravelToDeathScreen(); }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        OnDeath?.Invoke();

        if(gameObject.tag == "Player") { MainMenuScript.TravelToMainMenu(); }

        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}