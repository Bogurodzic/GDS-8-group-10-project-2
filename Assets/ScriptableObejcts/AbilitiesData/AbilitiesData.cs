using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class AbilitiesData : ScriptableObject
{
    public string description;
    public string abilityName;
    public int minRange;
    public int maxRange;
    public int areaOfEffect;
    public int cooldown;
    public int damage;
    public int heal;
    public AbilityType abilityType;
    public Sprite abilityButtonNormal;
    public Sprite abilityButtonHover;
    public Sprite abilityButtonActive;
    public AudioClip abilityAudio;
}