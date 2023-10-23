using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    private PlayerController playerController;
    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    //ANIMATION EVENTTTTTTTTTT
    public void Foot()
    {
        playerController.Foot();
    }

    //ANIMATION EVENTTTTTTTTTT
    public void Launch()
    {
        playerController.Launch();
    }
}
