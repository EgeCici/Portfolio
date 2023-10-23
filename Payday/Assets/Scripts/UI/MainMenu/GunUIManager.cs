using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunUIManager : MonoBehaviour
{
    public static GunUIManager Instance;

    [SerializeField] GameObject gunPrefab;
    [SerializeField] Transform gunHolder;

    [SerializeField] GunData[] guns;

    [Space(10)]
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite equippedSprite;
    [SerializeField] Sprite unbuyedSprite;

    [Space(10)]
    [SerializeField] Image gunImg;
    [SerializeField] TextMeshProUGUI gunName;

    [SerializeField] Slider powerSlider;
    [SerializeField] TextMeshProUGUI powerText;

    [SerializeField] Slider bulletSlider;
    [SerializeField] TextMeshProUGUI bulletText;

    [SerializeField] Slider accuracySlider;
    [SerializeField] TextMeshProUGUI accurracyText;

    [SerializeField] Slider speedSlider;
    [SerializeField] TextMeshProUGUI speedText;

    [Space(10)]
    [SerializeField] GameObject buyButton;
    [SerializeField] Button equipButton;
    [SerializeField] TextMeshProUGUI equipButtonText;

    private List<GunUIElementController> gunUIs = new List<GunUIElementController>();
    private GunUIElementController selectedGun;

    [SerializeField] GameObject watchAdPopup;

    private void Awake()
    {
        Instance = this;

        InitializeGuns();
    }

    private void Start()
    {
        EquipFirstGun();
        SelectFirstGun();
    }

    private void InitializeGuns()
    {
        foreach (var gunData in guns)
        {
            GameObject gun = Instantiate(gunPrefab, gunHolder);
            var controller = gun.GetComponent<GunUIElementController>();
            controller.gunData = gunData;
            controller.SetGunName(gunData.name);
            controller.SetGunSprite(gunData.sprite, gunData.spriteScale);
            controller.SetBuyed(gunData.isBuyed, gunData.isBuyed ? defaultSprite : unbuyedSprite, gunData.price);
            gunUIs.Add(controller);
        }

        
    }

    private void SelectFirstGun()
    {
        SelectGun(gunUIs[DataManager.Instance.lastSelectedGunIndex]);
    }

    private void EquipFirstGun()
    {
        gunUIs[DataManager.Instance.currentEquippedGunIndex].SetEquipped(true, equippedSprite);
    }

    public void SelectGun(GunUIElementController gun)
    {
        DataManager.Instance.lastSelectedGunIndex = gunUIs.IndexOf(gun);

        SetGunImg(gun.gunData.sprite, gun.gunData.spriteScale);

        gunName.text = gun.gunData.name;

        powerSlider.value = gun.gunData.damage;
        powerText.text = gun.gunData.damage.ToString();

        bulletSlider.value = gun.gunData.bullet;
        bulletText.text = gun.gunData.bullet.ToString();

        accuracySlider.value = gun.gunData.accuracy;
        accurracyText.text = gun.gunData.accuracy.ToString();

        speedSlider.value = gun.gunData.speed;
        speedText.text = gun.gunData.speed.ToString();

        buyButton.SetActive(!gun.gunData.isBuyed);
        equipButton.gameObject.SetActive(gun.gunData.isBuyed);

        if(DataManager.Instance.currentEquippedGunIndex == DataManager.Instance.lastSelectedGunIndex)
        {
            equipButton.interactable = false;
            equipButtonText.text = "EQUIPPED";
        }
        else
        {
            equipButtonText.text = "EQUIP";
            equipButton.interactable = true;
        }

        selectedGun = gun;

    }

    public void EquipButton()
    {
        gunUIs[DataManager.Instance.currentEquippedGunIndex].SetEquipped(false, defaultSprite);

        selectedGun.SetEquipped(true, equippedSprite);
        DataManager.Instance.currentEquippedGunIndex = gunUIs.IndexOf(selectedGun);
        SelectGun(selectedGun);
    }

    public void BuyButton()
    {
        int price = selectedGun.gunData.price;
        if (DataManager.Instance.MoneyAmount >= price)
        {
            DataManager.Instance.MoneyAmount -= price;
            selectedGun.gunData.isBuyed = true;
            selectedGun.SetBuyed(true, defaultSprite, price);
            SelectGun(selectedGun);

            string log = $"gun_{gunUIs.IndexOf(selectedGun)}_buyed";
            Debug.Log(log);
            Firebase.Analytics.FirebaseAnalytics.LogEvent(log);
        }
        else
        {
            //TODO: you dont have enough money
            Debug.Log("you dont have enough money");
            watchAdPopup.SetActive(true);
        }
    }


    public void SetGunImg(Sprite sprite, Vector2 scale)
    {
        gunImg.sprite = sprite;
        gunImg.sprite = sprite;
        gunImg.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scale.x * 4);
        gunImg.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scale.y * 4);
    }
}
