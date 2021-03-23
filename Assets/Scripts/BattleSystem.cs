using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { START, BET, BATTLE, LEFTWIN, RIGHTWIN, CLEANUP }

public class BattleSystem : MonoBehaviour
{
    public GameState state;

    public List<GameObject> prefabList;

    public Transform leftStartPos;
    public Transform rightStartPos;

    public Text dialougeText;

    public BattleHUD leftHUD;
    public BattleHUD rightHUD;
    public StatsHUD leftStatsHUD;
    public StatsHUD rightStatsHUD;

    private Character leftChar;
    private Character rightChar;
    private GameObject rightObj;
    private GameObject leftObj;

    private int GetUsableIndex(int avoid = -1)
    {
        int index = Random.Range(0, prefabList.Count);

        while (index == avoid)
        {
            index = Random.Range(0, prefabList.Count);
        }

        return index;
    }
    // Start is called before the first frame update
    void Start()
    {
        int leftIndex = GetUsableIndex();
        int rightIndex = GetUsableIndex(leftIndex);

        GameObject leftPrefab = prefabList[leftIndex];
        GameObject rightPrefab = prefabList[rightIndex];

        StartCoroutine(SetupBattle(leftPrefab, rightPrefab));
    }

    IEnumerator SetupBattle(GameObject LeftChar, GameObject RightChar)
    {
        state = GameState.START;

        Random.InitState((int)Time.time);

        Vector3 newPos = leftStartPos.position;
        newPos.x += Random.Range(-100, 100) / 100.0f;
        newPos.y += Random.Range(-100, 100) / 100.0f;

        leftObj = Instantiate(LeftChar);
        leftObj.transform.Translate(newPos); ;
        leftChar = leftObj.GetComponent<Character>();

        newPos = rightStartPos.position;
        newPos.x += Random.Range(-100, 100) / 100.0f;
        newPos.y += Random.Range(-100, 100) / 100.0f;

        rightObj = Instantiate(RightChar);
        rightObj.transform.Translate(newPos);
        rightChar = rightObj.GetComponent<Character>();

        dialougeText.text = leftChar.charName + " will now fight " + rightChar.charName;

        leftHUD.SetHUD(leftChar);
        rightHUD.SetHUD(rightChar);
        leftStatsHUD.SetHUD(leftChar);
        rightStatsHUD.SetHUD(rightChar);

        leftChar.onAttackCallback += OnLeftAttack;
        leftChar.onDeathCallback += OnLeftDie;
        rightChar.onAttackCallback += OnRightAttack;
        rightChar.onDeathCallback += OnRightDie;


        yield return new WaitForSeconds(0.5f);

        StartCoroutine(Betting());
    }

    IEnumerator Betting()
    {
        state = GameState.BET;
        //start predictions
        dialougeText.text = "Place your bets!";
        yield return new WaitForSeconds(5.0f);
        //close predictions

        dialougeText.text = "Now we're battlin'!";
        state = GameState.BATTLE;
        leftChar.Go();
        rightChar.Go();
    }

    public void OnLeftAttack()
    {
        float damage = Random.Range(leftChar.charStats.GetDamageValue(), leftChar.charStats.GetDamageValue() * 2);

        rightChar.TakeDamage(damage, leftChar.charStats.GetStunDurationValue(), 1.0f);
    }
    public void OnRightAttack()
    {
        float damage = Random.Range(rightChar.charStats.GetDamageValue(), rightChar.charStats.GetDamageValue() * 2);

        leftChar.TakeDamage(damage, rightChar.charStats.GetStunDurationValue(), 1.0f);
    }

    public void OnLeftDie()
    {
        state = GameState.RIGHTWIN;
        rightChar.Stop();
        StartCoroutine(End());
    }

    public void OnRightDie()
    {
        state = GameState.LEFTWIN;
        leftChar.Stop();
        StartCoroutine(End());
    }

    void Update()
    {
        if (state == GameState.BATTLE)
        {
            //update HUD
            leftHUD.SetHealth((int)leftChar.charStats.currentHealth);
            leftHUD.SetStamina((int)leftChar.charStats.currentStamina);
            leftHUD.SetAbility((int)leftChar.charStats.currentCooldown);

            rightHUD.SetHealth((int)rightChar.charStats.currentHealth);
            rightHUD.SetStamina((int)rightChar.charStats.currentStamina);
            rightHUD.SetAbility((int)rightChar.charStats.currentCooldown);
        }
    }

    IEnumerator End()
    {
        if (state == GameState.LEFTWIN)
        {
            dialougeText.text = leftChar.charName + " WINS!";

            //payout
        }
        else if (state == GameState.RIGHTWIN)
        {
            dialougeText.text = rightChar.charName + " WINS!";

            //payout
        }

        state = GameState.CLEANUP;
        Cleanup();
        yield return new WaitForSeconds(5.0f);

        int leftIndex = GetUsableIndex();
        int rightIndex = GetUsableIndex(leftIndex);

        GameObject leftPrefab = prefabList[leftIndex];
        GameObject rightPrefab = prefabList[rightIndex];

        StartCoroutine(SetupBattle(leftPrefab, rightPrefab));
    }

    public void Cleanup()
    {
        Destroy(leftChar);
        Destroy(rightChar);
        Destroy(leftObj);
        Destroy(rightObj);
    }
}
