using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType
{
    WeaponUpgrade,
    Stomach_Key,
    Brain_Key,
    HealthPack
}

public class Collectible : MonoBehaviour
{
    public CollectibleType collectibleType;
}