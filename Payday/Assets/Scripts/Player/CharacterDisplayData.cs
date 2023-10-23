using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="CharacterDisplay")]
public class CharacterDisplayData : ScriptableObject
{
    public new string name;

    public Sprite sprite;
    public int health;
    public int speed;

    public int price;

    public bool isBuyed;
    public string buy_key;

    public SkinnedMeshRenderer meshRenderer;
}
