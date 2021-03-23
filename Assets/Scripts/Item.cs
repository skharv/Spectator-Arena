using System.Collections.Generic;
using UnityEngine;

public enum EquipSlot { HEAD, BODY, LEFTHAND, RIGHTHAND }
public enum ItemType { ARMOR, WEAPON }

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public EquipSlot slot = EquipSlot.BODY;
    public ItemType type = ItemType.ARMOR;
    public List<StatModifier> modifiers;
}
