using UnityEngine;
using UnityEngine.Events;
using System;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth { get; private set; }
    public int currentHealth { get; private set; }
    public int maxStamina { get; private set; }
    public float currentStamina { get; private set; }
    public int maxAbility { get; private set; }
    public float currentAbility { get; private set; }

    public string abilityName { get; private set; }

    public float damage { get; private set; }
    public float stun { get; private set; }



    [SerializeField]
    private Stat bulk;
    [SerializeField]
    private Stat endurance;
    [SerializeField]
    private Stat speed;

    public void Awake()
    {
        bulk.Init();
        endurance.Init();
        speed.Init();
        abilityName = "Test Ability";

        bulk.onStatChanged += OnBulkChanged;
        endurance.onStatChanged += OnEnduranceChanged;
        speed.onStatChanged += OnSpeedChanged;

        OnBulkChanged();
        OnEnduranceChanged();
        OnSpeedChanged();

        currentHealth = maxHealth;
        currentStamina = maxStamina;
        maxAbility = 100;
        currentAbility = 0;
    }

    void OnBulkChanged()
    {
        CalculateMaxHealth();
        damage = bulk.Value;
        stun = bulk.Value / 10f;
    }
    void OnEnduranceChanged()
    {
        CalculateMaxStamina();
    }
    void OnSpeedChanged()
    {
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StatModifier mod = new StatModifier(2, StatModifierType.flat);
            bulk.AddModifiers(mod);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            StatModifier mod = new StatModifier(0.2f, StatModifierType.percent);
            bulk.AddModifiers(mod);
        }
    }

    public int GetBulkValue()
    {
        return bulk.Value;
    }
    public int GetEnduranceValue()
    {
        return endurance.Value;
    }
    public int GetSpeedValue()
    {
        return speed.Value;
    }

    public void SetCurrentAbility(float amount)
    {
        currentAbility = amount;
    }
    public void SetCurrentStamina(float amount)
    {
        currentStamina = amount;
    }
    public void SetCurrentHealth(int amount)
    {
        currentHealth = amount;
    }

    private void CalculateMaxHealth()
    {
        maxHealth = Mathf.RoundToInt(bulk.Value * 10);
        maxHealth += Mathf.RoundToInt(endurance.Value * 2);
    }
    private void CalculateMaxStamina()
    {
        maxStamina = Mathf.RoundToInt(endurance.Value * 10);
    }
}
