using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDisplayController : MonoBehaviour
{
    public CharacterDisplayData data;


    public GunDisplayController[] guns;

    public Animator animator;

    public SkinnedMeshRenderer displayMesh;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        displayMesh = GetComponent<SkinnedMeshRenderer>();
    }
}
