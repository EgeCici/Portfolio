using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    public string coin1 = "com.gunday.coin";
    public string coin2 = "com.gunday.coinmid";
    public string remove_ads = "com.gunday.removeads";
    public void OnPurchaseComplete(Product product)
    {
        if(product.definition.id == coin1)
        {
            DataManager.Instance.MoneyAmount += 10000; //1 dolar
        }
        else if(product.definition.id == coin2)
        {
            DataManager.Instance.MoneyAmount += 20000; //1.5 dolar
        }
        else if(product.definition.id == remove_ads) //1 dolar
        {
            DataManager.Instance.isAdsRemoved = true;
            //remove ads
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {

    }
}
