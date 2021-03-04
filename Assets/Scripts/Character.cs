using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public enum CharState { ATTACK, IDLE, BLOCK, DODGE, PARRIED, HIT, RECOVER }

public class Character : MonoBehaviour
{
    public CharacterStats charStats { get; private set; }
    public string charName { get; set; }
    public int charLevel { get; private set; }

    private CharState charState = CharState.IDLE;

    public Sprite attackFrame;
    public Sprite parriedFrame;
    public Sprite idleFrame;
    public Sprite recoveryFrame;
    public Sprite hitFrame;

    public delegate void OnAttack();
    public event OnAttack onAttack;

    private SpriteRenderer currentSprite;

    void Awake()
    {
        charStats = GetComponent<CharacterStats>();
        currentSprite = GetComponent<SpriteRenderer>();
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
                if (charStats.currentAbility < charStats.maxAbility)
                {
                    charStats.SetCurrentAbility(charStats.currentAbility + charStats.GetSpeedValue() / 100f);
                }
                else
                {
                    if (charStats.currentStamina - charStats.GetBulkValue() * 5 >= 0) //Ensure there is enough stamina to attack
                    {
                        StartCoroutine(Attack());
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
            case CharState.HIT:
                break;
            default:
                break;
        }

        //Stamina
        //if (currentStamina < maxStamina)
        //{
        //    currentStamina += endurance.Value / 1000f;
        //}
    }
    public void TakeDamage(float Damage, float Stun)
    {
        StopAllCoroutines();
        switch (charState)
        {
            case CharState.ATTACK:
                StartCoroutine(Parried(Damage, Stun));
                break;
            default:
                StartCoroutine(Hit(Damage, Stun));
                break;
        }
    }
    public virtual void Die()
    {
        //please override
        print(transform.name + " died.");
    }

    IEnumerator Attack()
    {
        if (onAttack != null)
        {
            charState = CharState.ATTACK;
            currentSprite.sprite = attackFrame;

            yield return new WaitForSeconds(10f / charStats.GetSpeedValue());
            charStats.SetCurrentStamina(charStats.currentStamina - charStats.GetBulkValue() * 5);
            charStats.SetCurrentAbility(0);
            onAttack.Invoke();
        }
        ReturnToIdle();
    }

    IEnumerator Recover()
    {
        charState = CharState.RECOVER;
        currentSprite.sprite = recoveryFrame;

        yield return new WaitUntil(() => charStats.currentStamina >= charStats.maxStamina);
        charStats.SetCurrentAbility(0);

        ReturnToIdle();
    }
    IEnumerator Parried(float Damage, float Stun)
    {
        charState = CharState.PARRIED;
        currentSprite.sprite = parriedFrame;

        float newHealth = charStats.currentHealth - (Damage / 2);
        charStats.SetCurrentHealth((int)newHealth);

        float stunAmount = Mathf.Max(0.5f, (Stun / charStats.GetBulkValue()) - (charStats.GetEnduranceValue() / 100.0f));
        charStats.SetCurrentAbility(charStats.maxAbility / 2);
        yield return new WaitForSeconds(stunAmount);

        ReturnToIdle();
    }

    IEnumerator Hit(float Damage, float Stun)
    {
        CharState prevState = charState;
        print(this.name + " was previously " + prevState.ToString());

        charState = CharState.HIT;
        currentSprite.sprite = hitFrame;

        float newHealth = charStats.currentHealth - Damage;
        charStats.SetCurrentHealth((int)newHealth);

        float stunAmount = Mathf.Max(0.25f, Stun - (charStats.GetEnduranceValue() / 100.0f));
        yield return new WaitForSeconds(stunAmount);

        //IF charStats.currentHealth < 0 DO -> Die()

        ReturnToIdle();
    }

    private void ReturnToIdle()
    {
        currentSprite.sprite = idleFrame;
        charState = CharState.IDLE;
    }
}
