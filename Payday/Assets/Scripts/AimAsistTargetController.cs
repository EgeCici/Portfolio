using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAsistTargetController : MonoBehaviour
{
    public Transform target { get; set; }

    public float zOffset;
    public float screenXoffset;
    public float yOffset;
    public float lerpTime;

    private Transform playerTransform;
    private void Start()
    {
        playerTransform = PlayerController.Instance.transform;
    }

    private void Update()
    {
        if (target == null)
        {
            Vector3 playerPos = new Vector3(playerTransform.position.x, playerTransform.position.y + yOffset, playerTransform.position.z);
            Vector3 targetPos = playerPos + (playerTransform.forward * zOffset) - (playerTransform.right * screenXoffset);
            Vector3 currentPos = transform.position;

            transform.position = Vector3.Lerp(currentPos, targetPos, lerpTime);
        }
        else
        {
            Vector3 targetPos = target.position;
            Vector3 currentPos = transform.position;

            transform.position = Vector3.Lerp(currentPos, targetPos, lerpTime);
        }
    }
}
