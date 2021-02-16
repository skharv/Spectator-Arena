using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public class Character : MonoBehaviour
{
    public CharacterStats charStats { get; private set; }
    public string charName { get; set; }
    public int charLevel { get; private set; }

    Character(string CharacterName)
    {

    }

    void Start()
    {
        charStats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        
    }
}
