using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public enum CharState { ATTACK, IDLE, BLOCK, DODGE, PARRIED, HIT, RECOVER, FLEX, STOP, DEAD }

public class Character : MonoBehaviour
{
    public CharacterStats charStats { get; private set; }
    public Inventory inventory { get; private set; }
    public string charName;
    public int charLevel { get; private set; }

    public KeyCode saveKey = KeyCode.F;

    private CharState charState = CharState.STOP;

    public Sprite attackFrame;
    public Sprite parriedFrame;
    public Sprite blockFrame;
    public Sprite idleFrame;
    public Sprite recoveryFrame;
    public Sprite hitFrame;
    public Sprite flexFrame;
    public Sprite deadFrame;

    public delegate void OnAttack();
    public event OnAttack onAttackCallback;

    public delegate void OnDeath();
    public event OnDeath onDeathCallback;

    public delegate void OnSave();
    public event OnSave onSaveCallback; // this is what i'm working on now.

    private SpriteRenderer currentSprite;

    void Awake()
    {
        charStats = GetComponent<CharacterStats>();
        currentSprite = GetComponent<SpriteRenderer>();
        charState = CharState.STOP;
    }

    void Start()
    {
        Resources.Load<Sprite>("Images/Character");
        
    }

    void Update()
    {
        switch (charState)
        {
            case CharState.ATTACK:
                break;
            case CharState.IDLE:
                //Ability
                if (charStats.currentCooldown < charStats.maxCooldown)
                {
                    charStats.SetCurrentCooldown(charStats.currentCooldown + charStats.GetCooldownRecoveryValue());
                }
                else
                {
                    if (charStats.currentStamina - charStats.GetBulkValue() * 5 >= 0) //Ensure there is enough stamina to attack
                    {
                        StartCoroutine(Attack());
                    }
                    else if (charStats.GetBulkValue() * 5 > charStats.GetMaxStaminaValue())
                    {
                        StartCoroutine(Flex());
                    }
                    else
                    {
                        StartCoroutine(Recover());
                    }
                }
                break;
            case CharState.RECOVER:
                charStats.SetCurrentStamina(charStats.currentStamina + ((charStats.GetEnduranceValue() + charStats.GetSpeedValue()) / 100f));
                break;
            case CharState.BLOCK: //Flows through to HIT
            case CharState.HIT: //Flows through to PARRIED
            case CharState.PARRIED:
                break;
            case CharState.STOP:
                break;
            case CharState.DEAD:
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(saveKey))
        {
            onSaveCallback.Invoke();
        }
    }

    public void TakeDamage(float Damage, float Stun, float HitChance)
    {
        StopAllCoroutines();
        switch (charState)
        {
            case CharState.ATTACK:
                StartCoroutine(Parried(Damage, Stun));
                break;
            default:
                if (Random.Range(0, 100) / 100f > Mathf.Min(charStats.GetBlockChanceValue(), BattleEquations.maximumBlockChance))
                    StartCoroutine(Hit(Damage, Stun));
                else
                    StartCoroutine(Block(Stun));
                break;
        }
    }

    public void Go() //Should only be called to start the game;
    {
        charState = CharState.IDLE;
    }

    public void Stop() //Should only be called to end the game;
    {
        StopAllCoroutines();
        charState = CharState.STOP;
    }

    private void Die()
    {
        StopAllCoroutines();
        print(name + " died.");
        charState = CharState.DEAD;
        currentSprite.sprite = deadFrame;
        onDeathCallback.Invoke();
    }

    IEnumerator Attack()
    {
        if (onAttackCallback != null)
        {
            charState = CharState.ATTACK;
            currentSprite.sprite = attackFrame;

            yield return new WaitForSeconds(10f / charStats.GetSpeedValue());
            charStats.SetCurrentStamina(charStats.currentStamina - charStats.GetBulkValue() * 5);
            charStats.SetCurrentCooldown(0);

            if (onAttackCallback != null)
            {
                onAttackCallback.Invoke();
            }
        }
        ReturnToIdle();
    }

    IEnumerator Flex()
    {
        if (onAttackCallback != null)
        {
            charState = CharState.FLEX;
            currentSprite.sprite = flexFrame;

            yield return new WaitForSeconds(0.5f);

            StatModifier mod = new StatModifier(charStats.GetBulkValue(), StatModifierType.flat);
            charStats.AddTempEndMod(mod);
        }
        ReturnToIdle();
    }

    IEnumerator Recover()
    {
        charState = CharState.RECOVER;
        currentSprite.sprite = recoveryFrame;

        yield return new WaitUntil(() => charStats.currentStamina >= charStats.GetMaxStaminaValue());
        charStats.SetCurrentCooldown(0);

        ReturnToIdle();
    }

    IEnumerator Parried(float Damage, float Stun)
    {
        charState = CharState.PARRIED;
        currentSprite.sprite = parriedFrame;

        float newHealth = charStats.currentHealth - (Damage / 2);
        charStats.SetCurrentHealth((int)newHealth);

        float stunAmount = Mathf.Max(BattleEquations.minimumParryStun, Mathf.Max(0, Stun - charStats.GetStunRecoveryValue()));
        yield return new WaitForSeconds(stunAmount);

        if (charStats.currentHealth <= 0)
        {
            Die();
        }

        ReturnToIdle();
    }   

    IEnumerator Hit(float Damage, float Stun)
    {
        charState = CharState.HIT;
        currentSprite.sprite = hitFrame;

        float newHealth = charStats.currentHealth - Damage;
        charStats.SetCurrentHealth((int)newHealth);

        float stunAmount = Mathf.Max(0, Stun - charStats.GetStunRecoveryValue());
        yield return new WaitForSeconds(stunAmount);

        if (charStats.currentHealth <= 0)
        {
            Die();
        }

        ReturnToIdle();
    }

    IEnumerator Block(float Stun)
    {
        charState = CharState.BLOCK;
        currentSprite.sprite = blockFrame;

        float stunAmount = Mathf.Max(0, Stun - charStats.GetStunRecoveryValue());
        yield return new WaitForSeconds(stunAmount);

        ReturnToIdle();
    }

    private void ReturnToIdle()
    {
        currentSprite.sprite = idleFrame;
        charState = CharState.IDLE;
    }


}
