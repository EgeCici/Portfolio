using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GunUIElementController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image cardImg;
    [SerializeField] Image gunImg;
    [SerializeField] TextMeshProUGUI gunName;
    [SerializeField] GameObject equipped;
    [SerializeField] GameObject unbuyed;
    [SerializeField] TextMeshProUGUI priceText;

    public GunData gunData;
    public void SetGunSprite(Sprite sprite, Vector2 scale)
    {
        gunImg.sprite = sprite;
        gunImg.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scale.x);
        gunImg.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scale.y);
    }

    public void SetGunName(string text)
    {
        gunName.text = text;
    }

    public void SetBuyed(bool value, Sprite cardsprite, int price)
    {
        cardImg.sprite = cardsprite;
        float a = value ? 1 : .5f;
        Color c = new Color(1, 1, 1, a);
        cardImg.color = c;
        gunImg.color = c;
        gunName.color = c;
        unbuyed.SetActive(!value);
        priceText.text = price + " COIN";
    }

    public void SetEquipped(bool value, Sprite sprite)
    {
        cardImg.sprite = sprite;
        equipped.SetActive(value);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GunUIManager.Instance.SelectGun(this);
    }
}
