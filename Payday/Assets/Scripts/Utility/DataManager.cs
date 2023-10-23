using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public const string MONEY_KEY = "MONEY";

    public static DataManager Instance;


    private int _moneyAmount;
    public int MoneyAmount
    {
        get
        {
            return _moneyAmount;
        }
        set
        {
            _moneyAmount = value;
            MoneyChangedAction?.Invoke();
        }
    }

    public const string IS_ADS_REMOVED = "IS_ADS_REMOVED";

    public const string CHARACTER_DISPLAY_INDEX_KEY = "CHARACTER_DISPLAY_INDEX";
    public const string CHARACTER_EQUIP_INDEX_KEY = "CHARACTER_EQUIP_INDEX";
    public const string GUN_DISPLAY_INDEX_KEY = "GUN_DISPLAY_INDEX";
    public const string GUN_EQUIP_INDEX_KEY = "GUN_EQUIP_INDEX";

    public const string SOUND_KEY = "SOUND";
    public const string SENSIBILITY_KEY = "SENSIBILITY";

    public const string IS_SHOTGUN_GIFT = "IS_SHOTGUN_GIFT";
    public const string IS_AK_GIFT = "IS_AK_GIFT";
    public const string IS_QUEEN_GIFT = "IS_QUEEN_GIFT";
    public const string IS_BONDE_GIFT = "IS_BONDE_GIFT";

    public int lastSelectedCharacterIndex;
    public int currentEquippedCharacterIndex;

    public int lastSelectedGunIndex;
    public int currentEquippedGunIndex;

    public float currentSoundVolume;
    public float currentSensibilityValue;

    public GunData[] currentGuns;
    public CharacterDisplayData[] currentCharacterDisplays;

    //Money degeri her degistiginde bunu invoke'luyorum ve boylece bu action'u dinleyen class'lar paranin degistigini bilip ona gore islemlerini yapabiliyor
    public Action MoneyChangedAction;

    public bool isAdsRemoved;

    public bool isShotgunGiftTaked;
    public bool isAkGiftTaked;
    public bool isQueenGiftTaked;
    public bool isBondeGiftTaked;


    public int giftCharacterIndex = -1;
    public int giftGunIndex = -1;


    private void Awake()
    {

        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        if (!PlayerPrefs.HasKey(SENSIBILITY_KEY))
        {
            PlayerPrefs.SetFloat(SENSIBILITY_KEY, 1);
        }

        PlayerPrefs.SetInt("PISTOL_BUY", 1);
        PlayerPrefs.SetInt("HOODIE_BUY", 1);

        GetData();
    }


    private void OnDisable()
    {
        SaveData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoneyAmount += 500;
            Debug.Log(MoneyAmount);
        }
#endif
    }

    public void GetData()
    {
        MoneyAmount = PlayerPrefs.GetInt(MONEY_KEY);
        lastSelectedCharacterIndex = PlayerPrefs.GetInt(CHARACTER_DISPLAY_INDEX_KEY);
        lastSelectedGunIndex = PlayerPrefs.GetInt(GUN_DISPLAY_INDEX_KEY);
        currentSoundVolume = PlayerPrefs.GetFloat(SOUND_KEY);
        currentSensibilityValue = PlayerPrefs.GetFloat(SENSIBILITY_KEY);
        currentEquippedCharacterIndex = PlayerPrefs.GetInt(CHARACTER_EQUIP_INDEX_KEY);
        currentEquippedGunIndex = PlayerPrefs.GetInt(GUN_EQUIP_INDEX_KEY);

        foreach (var gunData in currentGuns)
        {
            gunData.isBuyed = IntToBool(PlayerPrefs.GetInt(gunData.buyed_key));
        }

        foreach (var characterData in currentCharacterDisplays)
        {
            characterData.isBuyed = IntToBool(PlayerPrefs.GetInt(characterData.buy_key));
        }

        isAdsRemoved = IntToBool(PlayerPrefs.GetInt(IS_ADS_REMOVED));

        isShotgunGiftTaked = IntToBool(PlayerPrefs.GetInt(IS_SHOTGUN_GIFT));
        isAkGiftTaked = IntToBool(PlayerPrefs.GetInt(IS_AK_GIFT));
        isQueenGiftTaked = IntToBool(PlayerPrefs.GetInt(IS_QUEEN_GIFT));
        isBondeGiftTaked = IntToBool(PlayerPrefs.GetInt(IS_BONDE_GIFT));
    }
    public void SaveData()
    {
        PlayerPrefs.SetInt(MONEY_KEY, MoneyAmount);
        PlayerPrefs.SetInt(CHARACTER_DISPLAY_INDEX_KEY, lastSelectedCharacterIndex);
        PlayerPrefs.SetInt(GUN_DISPLAY_INDEX_KEY, lastSelectedGunIndex);
        PlayerPrefs.SetFloat(SOUND_KEY, currentSoundVolume);
        PlayerPrefs.SetFloat(SENSIBILITY_KEY, currentSensibilityValue);
        PlayerPrefs.SetInt(CHARACTER_EQUIP_INDEX_KEY, currentEquippedCharacterIndex);
        PlayerPrefs.SetInt(GUN_EQUIP_INDEX_KEY, currentEquippedGunIndex);

        foreach (var gunData in currentGuns)
        {
            PlayerPrefs.SetInt(gunData.buyed_key, BoolToInt(gunData.isBuyed));
        }

        foreach (var characterData in currentCharacterDisplays)
        {
            PlayerPrefs.SetInt(characterData.buy_key, BoolToInt(characterData.isBuyed));
        }

        PlayerPrefs.SetInt(IS_SHOTGUN_GIFT, BoolToInt(isShotgunGiftTaked));
        PlayerPrefs.SetInt(IS_AK_GIFT, BoolToInt(isAkGiftTaked));
        PlayerPrefs.SetInt(IS_QUEEN_GIFT, BoolToInt(isQueenGiftTaked));
        PlayerPrefs.SetInt(IS_BONDE_GIFT, BoolToInt(isBondeGiftTaked));

        PlayerPrefs.SetInt(IS_ADS_REMOVED, BoolToInt(isAdsRemoved));

    }

    private int BoolToInt(bool value)
    {
        return value == true ? 1 : 0;
    }

    private bool IntToBool(int value)
    {
        if (value > 1)
        {
            Debug.LogError("BU DEGER SADECE 0 VEYA 1 OLABILIR! BURADA BIR YANLISLIK VAR");
            return false;
        }

        return value == 1;
    }


}
