using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class StoreGunElement : MonoBehaviour, IPointerClickHandler
{
    public GunData gunData;


    public GameObject purchasedIcon;
    public TextMeshProUGUI gunNameText;
    public TextMeshProUGUI priceText;

    private StoreManager storeManager;

    private void Awake()
    {
        storeManager = GetComponentInParent<StoreManager>();
    }
    //Start kullanmamiz onemli, awake'de datalar cekiliyo oluyor
    private void Start()
    {
        purchasedIcon.SetActive(gunData.isBuyed);
        priceText.text = $"{gunData.price} $";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        storeManager.BuyGunRequest(this);
    }

    public void Buyed()
    {
        gunData.isBuyed = true;
        purchasedIcon.gameObject.SetActive(true);
    }
}
