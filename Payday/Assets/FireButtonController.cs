using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FireButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image logo;
    [SerializeField] Color defaultColor;
    [SerializeField] Color pressedColor;
    public void OnPointerDown(PointerEventData eventData)
    {
        GunManager.Instance.clickingFireButton = true;
        logo.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GunManager.Instance.clickingFireButton = false;
        logo.color = defaultColor;
    }


}
