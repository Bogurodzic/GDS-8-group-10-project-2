using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
[CreateAssetMenu]
public class AbilitiesData : ScriptableObject
{
    public string description;
    public int minRange;
    public int maxRange;
    public int areaOfEffect;
    public int cooldown;
    public int damage;
    public int heal;
    public AbilityType abilityType;
}