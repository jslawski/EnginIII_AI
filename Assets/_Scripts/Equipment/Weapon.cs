using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Equipment/Weapon")]
public class Weapon : Equipment
{
    public int attackPower;
    [Range(0.0f, 1.0f)]
    public float critChance;
    public Vector2 attackZoneDimensions;        
}
