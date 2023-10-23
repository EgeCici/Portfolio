using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageAmounTextController : MonoBehaviour
{
    public float destroyTime;
    public float movementSpeed;

    [SerializeField] TextMeshPro text;

    private void Awake()
    {
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
    }

    public void SetText(string txt)
    {
        text.text = txt;
    }
}
