using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject storePanel;


    [SerializeField] TextMeshProUGUI coinText;

    [Space(10)]
    [SerializeField] StorePanel equipsPanel;
    [SerializeField] StorePanel charactersPanel;
    [SerializeField] StorePanel coinsPanel;

    [SerializeField] TextMeshProUGUI panelText;

    [Space(10)]
    [SerializeField] GameObject watchAdPopup;
    [SerializeField] GameObject noInternetPopup;

    public bool isRewardedComeFromGift;
    [SerializeField] GiftAnimationController giftAnimationController;
    private void Awake()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("mainmenu_opened");

#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
      Debug.unityLogger.logEnabled = false;
#endif

        mainMenuPanel.SetActive(true);
        storePanel.SetActive(false);

        equipsPanel.Toggle(true);
        charactersPanel.Toggle(false);
        coinsPanel.Toggle(false);

        panelText.text = equipsPanel.text.text;
    }

    private void Start()
    {
        MoneyChanged();

        DataManager.Instance.MoneyChangedAction += MoneyChanged;
        GoogleAdMobController.Instance.OnAdClosedEvent.AddListener(InterstitialAdClosed);
        GoogleAdMobController.Instance.OnUserEarnedRewardEvent.AddListener(RewardedTaked);
    }

    private void OnDisable()
    {
        DataManager.Instance.MoneyChangedAction -= MoneyChanged;
        GoogleAdMobController.Instance.OnAdClosedEvent.RemoveListener(InterstitialAdClosed);
        GoogleAdMobController.Instance.OnUserEarnedRewardEvent.RemoveListener(RewardedTaked);
    }


    public void InterstitialAdClosed()
    {
        GoogleAdMobController.Instance.RequestAndLoadInterstitialAd();
    }

    public void MoneyChanged()
    {
        coinText.text = DataManager.Instance.MoneyAmount + " COIN";
        //moneyText.text = "Money: " + DataManager.Instance.MoneyAmount;
        //moneyText2.text = "Money: " + DataManager.Instance.MoneyAmount;
    }

    public void StoreButtonClicked()
    {
        mainMenuPanel.SetActive(false);
        storePanel.SetActive(true);
        Firebase.Analytics.FirebaseAnalytics.LogEvent("store_opened");
    }

    public void BackButtonClicked()
    {
        mainMenuPanel.SetActive(true);
        storePanel.SetActive(false);
    }

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

//#if UNITY_ANDROID && !UNITY_EDITOR
//            GoogleAdMobController.Instance.ShowInterstitialAd();
//#endif
    }

    public void WatchAdForMoneyButton()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("coin_rewarded_clicked");
        StartCoroutine(CheckForInternetConnection());
    }

    public IEnumerator CheckForInternetConnection()
    {
        UnityWebRequest request = new UnityWebRequest("https://www.google.com/");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            noInternetPopup.SetActive(true);
        }
        else
        {
            GoogleAdMobController.Instance.ShowRewardedAd();
        }
    }

    public void WatchAdForMoneyButtonWithPopup()
    {
        watchAdPopup.SetActive(false);
        WatchAdForMoneyButton();
    }

    private void RewardedTaked()
    {
        if (isRewardedComeFromGift)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("freegift_rewarded_confirmed");
            giftAnimationController.RewardedTaked();
        }
        else
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("coin_rewarded_confirmed");
            DataManager.Instance.MoneyAmount += 1000;
        }

        GoogleAdMobController.Instance.RequestAndLoadRewardedAd();
    }

    public void EquipsClicked()
    {
        equipsPanel.Toggle(true);
        charactersPanel.Toggle(false);
        coinsPanel.Toggle(false);

        panelText.text = equipsPanel.text.text;
    }

    public void CharactersClicked()
    {
        equipsPanel.Toggle(false);
        charactersPanel.Toggle(true);
        coinsPanel.Toggle(false);

        panelText.text = charactersPanel.text.text;
    }

    public void CoinsClicked()
    {
        equipsPanel.Toggle(false);
        charactersPanel.Toggle(false);
        coinsPanel.Toggle(true);

        panelText.text = coinsPanel.text.text;
    }

    public void GiftButtonClicked()
    {
        giftAnimationController.gameObject.SetActive(true);
        giftAnimationController.GiftButtonClicked();
        Firebase.Analytics.FirebaseAnalytics.LogEvent("freegift_rewarded_clicked");
    }
}

[Serializable]
public class StorePanel
{
    public GameObject panelBase;
    public TextMeshProUGUI text;
    public Image logo;
    public Color logoEnabled;
    public Color logoDisabled;


    public void Toggle(bool value)
    {
        panelBase.SetActive(value);
        text.color = new Color(1, 1, 1, value ? 1 : .5f);
        logo.color = value ? logoEnabled : logoDisabled;
    }
}