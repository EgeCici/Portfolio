using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharUIManager : MonoBehaviour
{
    [SerializeField] CharacterDisplayData[] characters;

    [SerializeField] Image characterImg;
    [SerializeField] TextMeshProUGUI characterName;

    [SerializeField] GameObject buyButton;

    [SerializeField] Button equipButton;
    [SerializeField] TextMeshProUGUI eqiupButtonText;

    [SerializeField] GameObject price;
    [SerializeField] TextMeshProUGUI priceText;

    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;

    [SerializeField] Slider speedSlider;
    [SerializeField] TextMeshProUGUI speedText;

    [SerializeField] GameObject watchAdPopup;
    private void Start()
    {
        LoadCharacter(characters[DataManager.Instance.lastSelectedCharacterIndex]);
    }

    private void LoadCharacter(CharacterDisplayData data)
    {
        characterImg.sprite = data.sprite;
        characterName.text = data.name;

        healthSlider.value = data.health;
        speedSlider.value = data.speed;

        healthText.text = data.health.ToString();
        speedText.text = data.speed.ToString();

        buyButton.SetActive(!data.isBuyed);
        equipButton.gameObject.SetActive(data.isBuyed);

        price.SetActive(buyButton.activeSelf);
        priceText.text = data.price + " COIN";
        if (DataManager.Instance.lastSelectedCharacterIndex == DataManager.Instance.currentEquippedCharacterIndex) //
        {
            equipButton.interactable = false;
            eqiupButtonText.text = "EQUIPPED";
        }
        else
        {
            equipButton.interactable = true;
            eqiupButtonText.text = "EQUIP";
        }

        DataManager.Instance.SaveData();
    }
    public void LeftButton()
    {
        if(DataManager.Instance.lastSelectedCharacterIndex > 0)
        {
            DataManager.Instance.lastSelectedCharacterIndex--;
        }
        else
        {
            DataManager.Instance.lastSelectedCharacterIndex = characters.Length - 1;
        }

        LoadCharacter(characters[DataManager.Instance.lastSelectedCharacterIndex]);
    }

    public void RightButton()
    {
        if(DataManager.Instance.lastSelectedCharacterIndex >= characters.Length - 1)
        {
            DataManager.Instance.lastSelectedCharacterIndex = 0;
        }
        else
        {
            DataManager.Instance.lastSelectedCharacterIndex++;
        }

        LoadCharacter(characters[DataManager.Instance.lastSelectedCharacterIndex]);
    }


    public void BuyButton()
    {
        var character = characters[DataManager.Instance.lastSelectedCharacterIndex];
        int price = character.price;
        if (DataManager.Instance.MoneyAmount >= price)
        {
            DataManager.Instance.MoneyAmount -= price;
            character.isBuyed = true;
            LoadCharacter(character);

            string log = $"character_{DataManager.Instance.lastSelectedCharacterIndex}_buyed";
            Debug.Log(log);
            Firebase.Analytics.FirebaseAnalytics.LogEvent(log);
        }
        else
        {
            Debug.Log("you dont have enough money");
            watchAdPopup.SetActive(true);
        }
    }

    public void EquipButton()
    {
        var character = characters[DataManager.Instance.lastSelectedCharacterIndex];
        if (character.isBuyed)
        {
            DataManager.Instance.currentEquippedCharacterIndex = DataManager.Instance.lastSelectedCharacterIndex;
            LoadCharacter(character);
        }
    }
}
