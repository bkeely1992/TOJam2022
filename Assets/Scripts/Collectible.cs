using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType
{
    WeaponUpgrade,
    KeyItem
}

public class Collectible : MonoBehaviour
{

    public CollectibleType collectibleType;
}
