using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject[] panels;
    //public GameObject mainMenuPanel;
    //public GameObject selectCharacterPanel;
    //public GameObject selectGunPanel;

    [Header("Character Selection")]
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI selectCharacterButtonText;
    public GameObject locked;

    [Header("Gun selection")]
    public TextMeshProUGUI gunNameText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI gunPowerText;
    public TextMeshProUGUI gunPriceText;
    public TextMeshProUGUI selectGunButtonText;
    public GameObject gunLocked;

    [Header("Other")]
    public CharacterDisplayController[] characterDisplays;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI moneyText2;
    public GameObject backButton;

    [Header("Canvas Groups")]
    public CanvasGroup mainMenuCanvasGroup;
    public CanvasGroup characterSelectionCanvasGroup;
    public CanvasGroup gunSelectionCanvasGroup;

    [Header("Settings")]
    public Slider soundSlider;
    public Slider sensibilitySlider;
    public AudioMixer audioMixer;


    private int charDisplayIndex { get { return DataManager.Instance.lastSelectedCharacterIndex; } set { DataManager.Instance.lastSelectedCharacterIndex = value; } }
    private bool isBuyCharacter; // eger character acik degilse buton select yerine buy'a donusuyor.

    private int gunDisplayIndex { get { return DataManager.Instance.lastSelectedGunIndex; } set { DataManager.Instance.lastSelectedGunIndex = value; } }
    private bool isBuyGun;

    private bool isPressedGunButtonTwice;

    private int currentPanelIndex;
    public int CurrentPanelIndex
    {
        get => currentPanelIndex;
        set
        {
            currentPanelIndex = value;
            CurrentPanelIndexChanged();
        }
    }

    private void CurrentPanelIndexChanged()
    {
        backButton.SetActive(CurrentPanelIndex != 0);

        if (characterDisplays[charDisplayIndex].data.isBuyed == false)
        {
            charDisplayIndex = 0;
        }

        if (characterDisplays[charDisplayIndex].guns[gunDisplayIndex].gunData.isBuyed == false)
        {
            gunDisplayIndex = 0;
        }

        CharacterChanged();
        GunChanged();
    }

    private void Awake()
    {
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
      Debug.unityLogger.logEnabled = false;
#endif
        foreach (var item in panels)
        {
            item.SetActive(false);
        }

        panels[CurrentPanelIndex].SetActive(true);
    }
    private void Start()
    {
        MoneyChanged();

        CurrentPanelIndex = 0;


        soundSlider.value = DataManager.Instance.currentSoundVolume;
        sensibilitySlider.value = DataManager.Instance.currentSensibilityValue;

    }
    //private void OnEnable()
    //{
    //    DataManager.Instance.MoneyChangedAction += MoneyChanged;
    //    GoogleAdMobController.Instance.OnAdClosedEvent.AddListener(InterstitialAdClosed);
    //    GoogleAdMobController.Instance.OnUserEarnedRewardEvent.AddListener(RewardedTaked);
    //}



    //private void OnDisable()
    //{
    //    DataManager.Instance.MoneyChangedAction -= MoneyChanged;
    //    GoogleAdMobController.Instance.OnAdClosedEvent.RemoveListener(InterstitialAdClosed);
    //    GoogleAdMobController.Instance.OnUserEarnedRewardEvent.RemoveListener(RewardedTaked);
    //}


    public void InterstitialAdClosed()
    {
        GoogleAdMobController.Instance.RequestAndLoadInterstitialAd();
    }

    public void MoneyChanged()
    {
        moneyText.text = "Money: " + DataManager.Instance.MoneyAmount;
        moneyText2.text = "Money: " + DataManager.Instance.MoneyAmount;
    }
    public void StartMissionButton()
    {
        //StartCoroutine(StartMissionWaiter());
        panels[CurrentPanelIndex].SetActive(false);
        CurrentPanelIndex++;
        panels[CurrentPanelIndex].SetActive(true);


        //mainMenuPanel.gameObject.SetActive(false);
        //selectCharacterPanel.gameObject.SetActive(true);


        //CharacterChanged();
        //GunChanged();
    }

    //private IEnumerator StartMissionWaiter()
    //{
    //    mainMenuCanvasGroup.interactable = false;
    //    float start = .5f;
    //    float duration = .5f;

    //    while (start > 0)
    //    {
    //        start -= Time.deltaTime;

    //        mainMenuCanvasGroup.alpha = start / duration;

    //        yield return new WaitForEndOfFrame();
    //    }
    //    mainMenuPanel.gameObject.SetActive(false);
    //    selectCharacterPanel.gameObject.SetActive(true);
    //    selectGunPanel.gameObject.SetActive(false);
    //    start = 0;

    //    while (start < duration)
    //    {
    //        start += Time.deltaTime;

    //        characterSelectionCanvasGroup.alpha = start / duration;

    //        yield return new WaitForEndOfFrame();
    //    }

    //}



    public void QuitButton()
    {
        Application.Quit();
    }

    public void SettingsButton()
    {

    }
    public void SelectCharacterButton()
    {
        if (isBuyCharacter)
        {
            if (DataManager.Instance.MoneyAmount >= characterDisplays[charDisplayIndex].data.price)
            {
                DataManager.Instance.MoneyAmount -= characterDisplays[charDisplayIndex].data.price;
                characterDisplays[charDisplayIndex].data.isBuyed = true;
                //TODO: Buy sound and vfx
                CharacterChanged();
                string log = $"character_{charDisplayIndex}_buyed";
                Debug.Log(log);
                Firebase.Analytics.FirebaseAnalytics.LogEvent(log);
            }
            else
            {
                //TODO: "You don't have enough money" notification
            }
        }
        else
        {
            //StartCoroutine(SelectCharacterWaiter());
            panels[CurrentPanelIndex].SetActive(false);
            CurrentPanelIndex++;
            panels[CurrentPanelIndex].SetActive(true);
            GunChanged();
        }

    }

    //private IEnumerator SelectCharacterWaiter()
    //{
    //    characterSelectionCanvasGroup.interactable = false;
    //    float start = .5f;
    //    float duration = .5f;

    //    while (start > 0)
    //    {
    //        start -= Time.deltaTime;

    //        characterSelectionCanvasGroup.alpha = start / duration;

    //        yield return new WaitForEndOfFrame();
    //    }
    //    mainMenuPanel.gameObject.SetActive(false);
    //    selectCharacterPanel.gameObject.SetActive(false);
    //    selectGunPanel.gameObject.SetActive(true);
    //    start = 0;

    //    while (start < duration)
    //    {
    //        start += Time.deltaTime;

    //        gunSelectionCanvasGroup.alpha = start / duration;

    //        yield return new WaitForEndOfFrame();
    //    }

    //}
    public void SelectGunButton()
    {
        if (isBuyGun)
        {
            if (DataManager.Instance.MoneyAmount >= characterDisplays[charDisplayIndex].guns[gunDisplayIndex].gunData.price)
            {
                DataManager.Instance.MoneyAmount -= characterDisplays[charDisplayIndex].guns[gunDisplayIndex].gunData.price;
                characterDisplays[charDisplayIndex].guns[gunDisplayIndex].gunData.isBuyed = true;
                //TODO: Buy sound and vfx
                GunChanged();
                string log = $"gun_{gunDisplayIndex}_buyed";
                Debug.Log(log);
                Firebase.Analytics.FirebaseAnalytics.LogEvent(log);
            }
            else
            {
                //TODO: "You don't have enough money" notification
            }
        }
        else
        {
            //Start game!
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

//#if UNITY_ANDROID && !UNITY_EDITOR
//            GoogleAdMobController.Instance.ShowInterstitialAd();
//#endif
        }

    }

    public void BackButtonPressed()
    {
        CurrentPanelIndex--;
        if (CurrentPanelIndex < 0)
            CurrentPanelIndex = 0;


        foreach (var item in panels)
        {
            item.SetActive(false);
        }

        panels[CurrentPanelIndex].SetActive(true);
    }

    public void SelectCharacterBackButton()
    {
        charDisplayIndex--;

        if (charDisplayIndex < 0)
        {
            charDisplayIndex = characterDisplays.Length - 1;
        }

        CharacterChanged();
        GunChanged();
    }

    public void SelectCharacterNextButton()
    {
        charDisplayIndex++;

        if (charDisplayIndex >= characterDisplays.Length)
        {
            charDisplayIndex = 0;
        }


        CharacterChanged();
        GunChanged();
    }

    public void CharacterChanged()
    {

        foreach (var item in characterDisplays)
        {
            item.gameObject.SetActive(false);
        }

        characterDisplays[charDisplayIndex].gameObject.SetActive(true);


        characterNameText.text = $"Name: {characterDisplays[charDisplayIndex].data.name}";
        healthText.text = $"Health: {characterDisplays[charDisplayIndex].data.health}";
        speedText.text = $"Speed: {characterDisplays[charDisplayIndex].data.speed}";
        priceText.text = $"Price: {characterDisplays[charDisplayIndex].data.price}";

        isBuyCharacter = !characterDisplays[charDisplayIndex].data.isBuyed;
        locked.gameObject.SetActive(isBuyCharacter);

        selectCharacterButtonText.text = isBuyCharacter ? "Buy Character" : "Select Character";

        DataManager.Instance.SaveData();

        //characterDisplays[charDisplayIndex].gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = PlayerController.Instance.currentPlayerMesh.sharedMesh;




    }

    public void SelectGunBackButton()
    {
        gunDisplayIndex--;

        if (gunDisplayIndex < 0)
        {
            gunDisplayIndex = characterDisplays[charDisplayIndex].guns.Length - 1;
        }

        GunChanged();
    }

    public void SelectGunNextButton()
    {
        gunDisplayIndex++;

        if (gunDisplayIndex >= characterDisplays[charDisplayIndex].guns.Length)
        {
            gunDisplayIndex = 0;
        }

        GunChanged();
    }

    private void GunChanged()
    {
        foreach (var gun in characterDisplays[charDisplayIndex].guns)
        {
            gun.gameObject.SetActive(false);
        }

        characterDisplays[charDisplayIndex].guns[gunDisplayIndex].gameObject.SetActive(true);

        gunNameText.text = $"Name: {characterDisplays[charDisplayIndex].guns[gunDisplayIndex].gunData.name}";
        gunPowerText.text = $"Power: {characterDisplays[charDisplayIndex].guns[gunDisplayIndex].gunData.damage}";
        fireRateText.text = $"Fire Rate: {characterDisplays[charDisplayIndex].guns[gunDisplayIndex].gunData.fireRate}";
        gunPriceText.text = $"Price: {characterDisplays[charDisplayIndex].guns[gunDisplayIndex].gunData.price}";

        isBuyGun = !characterDisplays[charDisplayIndex].guns[gunDisplayIndex].gunData.isBuyed;
        gunLocked.gameObject.SetActive(isBuyGun);

        selectGunButtonText.text = isBuyGun ? "Buy Gun" : "Select Gun";

        characterDisplays[charDisplayIndex].animator.SetBool("rifle", characterDisplays[charDisplayIndex].guns[gunDisplayIndex].isRifle);

        DataManager.Instance.SaveData();

    }


    public void SoundSliderValueChanged()
    {
        float value = soundSlider.value;
        audioMixer.SetFloat("volume", value);
        DataManager.Instance.currentSoundVolume = soundSlider.value;
    }

    public void SensibilitySliderValueChanged()
    {
        DataManager.Instance.currentSensibilityValue = sensibilitySlider.value;
    }

    public void WatchAdForMoneyButton()
    {
        GoogleAdMobController.Instance.ShowRewardedAd();
    }

    private void RewardedTaked()
    {
        DataManager.Instance.MoneyAmount += 300;
        GoogleAdMobController.Instance.RequestAndLoadRewardedAd();
    }
}
