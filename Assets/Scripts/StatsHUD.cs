using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsHUD : MonoBehaviour
{
    private Character targetChar;

    public Text bulkText;
    public Text enduranceText;
    public Text speedText;
    public Text damageText;
    public Text abilityText;

    public void SetHUD(Character Char)
    {
        targetChar = Char;
        bulkText.text = targetChar.charStats.GetBulkValue().ToString();
        enduranceText.text = targetChar.charStats.GetEnduranceValue().ToString();
        speedText.text = targetChar.charStats.GetSpeedValue().ToString();
        damageText.text = targetChar.charStats.GetDamageValue() + " - " + (targetChar.charStats.GetDamageValue() * 2);
        abilityText.text = targetChar.charStats.abilityName;
    }

    private void Update()
    {
        bulkText.text = targetChar.charStats.GetBulkValue().ToString();
        enduranceText.text = targetChar.charStats.GetEnduranceValue().ToString();
        speedText.text = targetChar.charStats.GetSpeedValue().ToString();
    }
}
