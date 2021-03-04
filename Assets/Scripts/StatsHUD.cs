using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsHUD : MonoBehaviour
{
    public Text bulkText;
    public Text enduranceText;
    public Text speedText;
    public Text damageText;
    public Text abilityText;

    public void SetHUD(Character Char)
    {
        bulkText.text = Char.charStats.GetBulkValue().ToString();
        enduranceText.text = Char.charStats.GetEnduranceValue().ToString();
        speedText.text = Char.charStats.GetSpeedValue().ToString();
        damageText.text = Char.charStats.damage.ToString();
        abilityText.text = Char.charStats.abilityName;
    }
}
