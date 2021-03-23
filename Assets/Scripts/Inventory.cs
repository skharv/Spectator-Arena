using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public void Equip(Item item)
    {
        //search for an item in the list with the same slot
        foreach (Item i in items)
        {
            if (i.slot == item.slot)
            {
                items.Remove(i);
                items.Add(item);
                return;
            }
        }

        items.Add(item);
    }

    public void Unequip(Item item)
    {
        items.Remove(item);
    }

}
