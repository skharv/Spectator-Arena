using UnityEngine;
using UnityEngine.Events;
using System;

public class CharacterStats : MonoBehaviour
{
    public float currentHealth { get; private set; }
    public float currentStamina { get; private set; }
    public float maxCooldown { get; private set; }
    public float currentCooldown { get; private set; }

    public string abilityName { get; private set; }

    public Inventory inventory;

    //Stats
    [SerializeField]
    private Stat bulk;
    [SerializeField]
    private ChildStat maxHealth;
    [SerializeField]
    private ChildStat damage;
    [SerializeField]
    private ChildStat stunDuration;

    [SerializeField]
    private Stat endurance;
    [SerializeField]
    private ChildStat maxStamina;
    [SerializeField]
    private ChildStat stunRecovery;
    [SerializeField]
    private ChildStat armor;


    [SerializeField]
    private Stat speed;
    [SerializeField]
    private ChildStat cooldownRecovery;
    [SerializeField]
    private ChildStat attackSpeed;
    [SerializeField]
    private ChildStat blockChance;

    public void Awake()
    {
        inventory = GetComponent<Inventory>();

        bulk.Init();
        endurance.Init();
        speed.Init();
        abilityName = "Test Ability";

        bulk.onStatChangedCallback += OnBulkChanged;
        endurance.onStatChangedCallback += OnEnduranceChanged;
        speed.onStatChangedCallback += OnSpeedChanged;
        //inventory.onItemChangedCallback += OnItemChanged;

        damage.Init(bulk, 0.25f, StatModifierType.flat);
        stunDuration.Init(bulk, 0.01f, StatModifierType.flat);
        maxHealth.Init(bulk, 5.0f, StatModifierType.flat);

        armor.Init(endurance, 0.1f, StatModifierType.flat);
        maxStamina.Init(endurance, 5.0f, StatModifierType.flat);
        stunRecovery.Init(endurance, 0.01f, StatModifierType.flat);

        blockChance.Init(speed, 0.05f, StatModifierType.flat);
        cooldownRecovery.Init(speed, 0.01f, StatModifierType.flat);
        attackSpeed.Init(speed, 0.15f, StatModifierType.flat);

        currentHealth = maxHealth.Value;
        currentStamina = maxStamina.Value;
        maxCooldown = 100;
        currentCooldown = 0;

        //OnBulkChanged();
        //OnEnduranceChanged();
        //OnSpeedChanged();
    }

    void OnBulkChanged()
    {
    }
    void OnEnduranceChanged()
    {
    }
    void OnSpeedChanged()
    {
    }

    void OnItemChanged(Item newItem, Item OldItem)
    {
    }

    public float GetBulkValue()
    {
        return bulk.Value;
    }
    public float GetEnduranceValue()
    {
        return endurance.Value;
    }
    public float GetSpeedValue()
    {
        return speed.Value;
    }

    public float GetDamageValue()
    {
        return damage.Value;
    }
    public float GetStunDurationValue()
    {
        return stunDuration.Value;
    }
    public float GetStunRecoveryValue()
    {
        return stunRecovery.Value;
    }
    public float GetBlockChanceValue()
    {
        return blockChance.Value;
    }
    public float GetArmorValue()
    {
        return armor.Value;
    }
    public float GetCooldownRecoveryValue()
    {
        return cooldownRecovery.Value;
    }
    public float GetMaxStaminaValue()
    {
        return maxStamina.Value;
    }
    public float GetMaxHealthValue()
    {
        return maxHealth.Value;
    }

    public void SetCurrentCooldown(float amount)
    {
        currentCooldown = amount;
    }
    public void SetCurrentStamina(float amount)
    {
        currentStamina = amount;
    }
    public void SetCurrentHealth(int amount)
    {
        currentHealth = amount;
    }

    public void AddTempEndMod(StatModifier modifier)
    {
        endurance.AddModifiers(modifier);
    }
    public void AddTempBulkMod(StatModifier modifier)
    {
        bulk.AddModifiers(modifier);
    }
    public void AddTempSpeedMod(StatModifier modifier)
    {
        speed.AddModifiers(modifier);
    }
}
