using UnityEngine;
using TMPro;

public class StoreManager : MonoBehaviour
{
    ////Gun Texts
    public TextMeshProUGUI moneyText;
    //public GameObject akPurchaseText;
    //public GameObject mp5PurchaseText;
    //public GameObject shotgunPurchaseText;
    //public GameObject glockPurchaseText;
    ////Gun Icons
    //public GameObject akPurchasedIcon;
    //public GameObject mp5PurchasedIcon;
    //public GameObject shotgunPurchasedIcon;
    //public GameObject glockPurchasedIcon;


    private void OnEnable()
    {
        DataManager.Instance.MoneyChangedAction += OnMoneyChanged;
    }

    private void OnDisable()
    {
        DataManager.Instance.MoneyChangedAction -= OnMoneyChanged;
    }

    //Start kullanmamiz onemli, awake'de datalar cekiliyo oluyor
    private void Start()
    {
        moneyText.text = DataManager.Instance.MoneyAmount + "$";
    }

    public void BuyGunRequest(StoreGunElement gun)
    {
        if(DataManager.Instance.MoneyAmount >= gun.gunData.price)
        {
            gun.Buyed();
            DataManager.Instance.MoneyAmount -= gun.gunData.price;

        }
    }


    public void OnMoneyChanged()
    {
        moneyText.text = DataManager.Instance.MoneyAmount + "$";
    }
    //public void PurchaseAk(int money)
    //{
        
    //    if (moneyCount >= 1000)
    //    {
    //        isGunSwitchButton = true;
    //        moneyCount -= 1000;
    //        akPurchaseText.SetActive(false);
    //        akPurchasedIcon.SetActive(true);
    //    }
    //}

    //public void PurchaseMp5(int money)
    //{

    //    if (moneyCount >= 1500)
    //    {
    //        isGunSwitchButton = true;
    //        moneyCount -= 1500;
    //        mp5PurchaseText.SetActive(false);
    //        mp5PurchasedIcon.SetActive(true);
    //    }
    //}

    //public void PurchaseShotgun(int money)
    //{

    //    if (moneyCount >= 2000)
    //    {
    //        isGunSwitchButton = true;
    //        moneyCount -= 2000;
    //        shotgunPurchaseText.SetActive(false);
    //        shotgunPurchasedIcon.SetActive(true);
    //    }
    //}


    //public void PurchaseGlock(int money)
    //{

    //    if (moneyCount >= 800)
    //    {
    //        isGunSwitchButton = true;
    //        moneyCount -= 800;
    //        glockPurchaseText.SetActive(false);
    //        glockPurchasedIcon.SetActive(true);
    //    }
    //}
}
