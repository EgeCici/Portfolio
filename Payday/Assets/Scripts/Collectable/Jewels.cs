using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jewels : MonoBehaviour
{
    public Display display;
    

    private void Awake()
    {
        display = GetComponentInParent<Display>();
    }

    public void Collect()
    {
        Destroy(gameObject);
    }

    //private void Update()
    //{
    //    if (display.enabled == false && PlayerController.Instance.collectButtonPressed)
    //    {
    //        gameObject.AddComponent<BoxCollider>().isTrigger = true;
    //    }
    //}

}
