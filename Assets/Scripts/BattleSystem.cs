using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { START, BET, BATTLE, LEFTWIN, RIGHTWIN, PAYOUT }

public class BattleSystem : MonoBehaviour
{
    public GameState State;

    public GameObject leftPrefab;
    public GameObject rightPrefab;

    public Transform leftStartPos;
    public Transform rightStartPos;

    public Text dialougeText;

    public BattleHUD leftHUD;
    public BattleHUD rightHUD;

    private Character leftCharacter;
    private Character rightCharacter;


    // Start is called before the first frame update
    void Start()
    {
        State = GameState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        GameObject leftObj = Instantiate(leftPrefab);
        leftObj.transform.Translate(leftStartPos.position); ;
        leftCharacter = leftObj.GetComponent<Character>();
        leftCharacter.charName = "Lefty";

        GameObject rightObj = Instantiate(rightPrefab);
        rightObj.transform.Translate(rightStartPos.position); ;
        rightCharacter = rightObj.GetComponent<Character>();
        rightCharacter.charName = "Righty";

        dialougeText.text = leftCharacter.charName + " will fight " + rightCharacter.charName;

        leftHUD.SetHUD(leftCharacter);
        rightHUD.SetHUD(rightCharacter);
    }
}
