using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { START, BET, BATTLE, LEFTWIN, RIGHTWIN, PAYOUT }

public class BattleSystem : MonoBehaviour
{
    public GameState state;

    public GameObject leftPrefab;
    public GameObject rightPrefab;

    public Transform leftStartPos;
    public Transform rightStartPos;

    public Text dialougeText;

    public BattleHUD leftHUD;
    public BattleHUD rightHUD;
    public StatsHUD leftStatsHUD;
    public StatsHUD rightStatsHUD;

    private Character leftChar;
    private Character rightChar;

    // Start is called before the first frame update
    void Start()
    {
        state = GameState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject leftObj = Instantiate(leftPrefab);
        leftObj.transform.Translate(leftStartPos.position); ;
        leftChar = leftObj.GetComponent<Character>();
        leftChar.charName = "Lefty";

        GameObject rightObj = Instantiate(rightPrefab);
        rightObj.transform.Translate(rightStartPos.position);
        rightChar = rightObj.GetComponent<Character>();
        rightChar.charName = "Righty";

        dialougeText.text = leftChar.charName + " will now fight " + rightChar.charName;

        leftHUD.SetHUD(leftChar);
        rightHUD.SetHUD(rightChar);
        leftStatsHUD.SetHUD(leftChar);
        rightStatsHUD.SetHUD(rightChar);

        leftChar.onAttack += OnLeftAttack;
        rightChar.onAttack += OnRightAttack;

        yield return new WaitForSeconds(0.5f);

        state = GameState.BATTLE;
    }

    public void OnLeftAttack()
    {
        rightChar.TakeDamage(leftChar.charStats.damage, leftChar.charStats.stun);
    }
    public void OnRightAttack()
    {
        leftChar.TakeDamage(rightChar.charStats.damage, rightChar.charStats.stun);
    }

    void Update()
    {
        if (state == GameState.BATTLE)
        {
            //update HUD
            dialougeText.text = "Now we're battlin'!";
            leftHUD.SetHealth(leftChar.charStats.currentHealth);
            leftHUD.SetStamina((int)leftChar.charStats.currentStamina);
            leftHUD.SetAbility((int)leftChar.charStats.currentAbility);

            rightHUD.SetHealth(rightChar.charStats.currentHealth);
            rightHUD.SetStamina((int)rightChar.charStats.currentStamina);
            rightHUD.SetAbility((int)rightChar.charStats.currentAbility);
        }
    }
}
