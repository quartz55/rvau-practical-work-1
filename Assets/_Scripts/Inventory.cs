using System.Collections.Generic;
using UnityEngine;

public class Inventory : ScriptableObject
{
    public int Bullets;
    public int Bombs;

    public HashSet<Weapon> Weapons;
}