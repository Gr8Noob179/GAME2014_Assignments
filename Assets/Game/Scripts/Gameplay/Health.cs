using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class HealthChangedEvent : UnityEvent<float, float> { } // current, max

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private bool destroyOnDeath = true;

    [SerializeField]
    private float destroyDelay = 2.5f;

    [Header("Optional UI")]
    [SerializeField]
    private Slider slider;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    public bool IsDead => CurrentHealth <= 0f;

    public HealthChangedEvent OnHealthChanged;
    public UnityEvent OnDeath;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        UpdateUI();

        if (slider)
        {
            slider.maxValue = maxHealth;
        }
    }

    public void ApplyDamage(float amount)
    {
        if (IsDead)
        {
            return;
        }

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        UpdateUI();

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead)
        {
            return;
        }

        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        UpdateUI();
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();
        AudioManager.Instance.PlaySFX("Explosion");

        if (destroyOnDeath)
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    private void UpdateUI()
    {
        if (slider)
        {
            slider.value = CurrentHealth;
        }
    }
}
