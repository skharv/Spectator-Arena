using UnityEngine;
using UnityEngine.Events;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth { get; private set; }
    public int currentHealth { get; private set; }
    public int maxStamina { get; private set; }
    public int currentStamina { get; private set; }
    public int currentAbility { get; private set; }
    public int maxAbility { get; private set; }

    public int damage { get; private set; }

    [SerializeField]
    private Stat bulk;
    [SerializeField]
    private Stat endurance;
    [SerializeField]
    private Stat speed;

    public void Start()
    {
        bulk.onStatChanged += OnBulkChanged;
    }

    void OnBulkChanged()
    {
        maxHealth = Mathf.RoundToInt(bulk.Value * 10);
        print("Bulk has changed! Now bulk is " + bulk.Value + " and HP is now " + maxHealth);
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
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        print(transform.name + " takes " + damage + " damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
     
    public virtual void Die()
    {
        //please override
        print(transform.name + " died.");
    }
}
