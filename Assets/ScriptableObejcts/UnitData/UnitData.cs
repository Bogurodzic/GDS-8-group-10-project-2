﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class UnitData : ScriptableObject
{
    public int minAttack;
    public int maxAttack;
    public int defend;
    public int maxHp;
    public int movementRange;
    public int minAttackRange;
    public int maxAttackRange;
    public AbilitiesData unitAbility;
    public Sprite unitSpriteTeam1;
    public Sprite unitSpriteTeam2;
    public Sprite unitPickerSprite;
    public Sprite unitListSprite;
} 