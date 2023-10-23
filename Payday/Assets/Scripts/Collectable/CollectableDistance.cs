using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectableDistance : MonoBehaviour
{
    public Transform playerTransform;
    public Transform objectTransform;
    public TextMeshProUGUI distanceText;
    public float distance;

    private void Start()
    {
        playerTransform = PlayerController.Instance.transform;
    }

    void Update()
    {
        distanceText.text = distance.ToString("0") + "m";
        distance = Vector3.Distance(playerTransform.position, objectTransform.position);
    }
}
