using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatistics : MonoBehaviour
{
    public int team = 1;
    public int minAttack = 20;
    public int maxAttack = 40;
    public int defend = 0;
    public int maxHp = 100;
    public int currentHp;
    public int movementRange = 6;
    void Start()
    {
        currentHp = maxHp;
    }

    void Update()
    {
        
    }
}
