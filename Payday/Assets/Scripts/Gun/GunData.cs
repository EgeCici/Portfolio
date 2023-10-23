using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public Vector2 spriteScale;

    public int damage;
    public int bullet;
    public int accuracy;
    public int speed;

    public int magSize;
    public int fireRate;
    public int reloadTime;
    public bool reloading;
    public float shootDelay;

    public bool isBuyed;
    public string buyed_key;
    public int price;

    public AudioClip audioClip;
}
