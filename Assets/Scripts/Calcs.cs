using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calcs
{
    private static float critical = 1.2f;
    private static float armoured = 0.6f;
    private static float effective = 1.2f;
    private static float resisted = 0.6f;

    public static float damage(float damage,bool isCritical,bool isArmoured,bool isEffective,bool isResisted)
    {
        return damage * (isCritical ? critical : 1f) * (isArmoured ? armoured : 1f) * (isEffective ? effective : 1f) * (isResisted ? resisted : 1f); 
    }
}
