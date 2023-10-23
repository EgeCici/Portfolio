using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class GiftAnimationController : MonoBehaviour
{
    public Transform[] giftElements;
    [SerializeField] GunData akData;
    [SerializeField] GunData shotgunData;
    [SerializeField] CharacterDisplayData queenData;
    [SerializeField] CharacterDisplayData bondeData;

    public int turnCount;

    [SerializeField] GameObject giftHolder;
    [SerializeField] GameObject takeGiftPanel;
    [SerializeField] Image winnerGiftImg;
    [SerializeField] Sprite caseSprite;
    [SerializeField] TextMeshProUGUI giftText;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] MainMenuController mainMenu;
    [SerializeField] GameObject noInternetPanel;

    private RectTransform winnerGiftRect;


    private bool isCoinTaked;
    private void Start()
    {
        winnerGiftRect = winnerGiftImg.GetComponent<RectTransform>();
        
    }

    public void GiftButtonClicked()
    {
        giftHolder.SetActive(false);
        noInternetPanel.SetActive(false);
        takeGiftPanel.SetActive(false);

        StartCoroutine(CheckForInternetConnection());

    }

    public IEnumerator CheckForInternetConnection()
    {
        UnityWebRequest request = new UnityWebRequest("https://www.google.com/");
        yield return request.SendWebRequest();

        if(request.error != null)
        {
            noInternetPanel.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            mainMenu.isRewardedComeFromGift = true;
            GoogleAdMobController.Instance.ShowRewardedAd();
        }
    }

    public void RewardedTaked()
    {
        giftHolder.SetActive(true);
        mainMenu.isRewardedComeFromGift = false;
        StartCoroutine(RunAnimation());
    }

    private IEnumerator RunAnimation()
    {
        for (int j = 0; j < turnCount; j++)
        {
            for (int i = 0; i < giftElements.Length; i++)
            {
                giftElements[i].SetAsLastSibling();
                yield return new WaitForSeconds(.01f);
            }
        }
        giftHolder.SetActive(false);
        takeGiftPanel.SetActive(true);
        buttonText.text = "TRY IT";
        if (shotgunData.isBuyed == false && DataManager.Instance.isShotgunGiftTaked == false)
        {
            winnerGiftImg.sprite = shotgunData.sprite;
            winnerGiftRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, shotgunData.spriteScale.x * 3);
            winnerGiftRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, shotgunData.spriteScale.y * 3);
            giftText.text = $"You win {shotgunData.name} for one game!";
            DataManager.Instance.isShotgunGiftTaked = true;
            DataManager.Instance.giftGunIndex = 4;
        }
        else if (akData.isBuyed == false && DataManager.Instance.isAkGiftTaked == false)
        {
            winnerGiftImg.sprite = akData.sprite;
            winnerGiftRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, akData.spriteScale.x * 3);
            winnerGiftRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, akData.spriteScale.y * 3);
            giftText.text = $"You win {akData.name} for one game!";
            DataManager.Instance.isAkGiftTaked = true;
            DataManager.Instance.giftGunIndex = 3;
        }
        else if (queenData.isBuyed == false && DataManager.Instance.isQueenGiftTaked == false)
        {
            winnerGiftImg.sprite = queenData.sprite;
            winnerGiftRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 113);
            winnerGiftRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 250);
            giftText.text = $"You win {queenData.name} for one game!";
            DataManager.Instance.isQueenGiftTaked = true;
            DataManager.Instance.giftCharacterIndex = 4;
        }
        else if (bondeData.isBuyed == false && DataManager.Instance.isBondeGiftTaked == false)
        {
            winnerGiftImg.sprite = bondeData.sprite;
            winnerGiftRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 113);
            winnerGiftRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 250);
            giftText.text = $"You win {bondeData.name} for one game!";
            DataManager.Instance.isBondeGiftTaked = true;
            DataManager.Instance.giftCharacterIndex = 3;
        }
        else
        {
            winnerGiftImg.sprite = caseSprite;
            winnerGiftRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
            winnerGiftRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 300);
            int goldAmount = Random.Range(200, 700);
            DataManager.Instance.MoneyAmount += goldAmount;
            giftText.text = $"You win {goldAmount} COIN!";
            isCoinTaked = true;
            buttonText.text = "PERFECT";
            DataManager.Instance.giftGunIndex = -1;
            DataManager.Instance.giftCharacterIndex = -1;
        }

    }

    public void TryItButton()
    {
        if (isCoinTaked)
        {
            gameObject.SetActive(false);

        }
        else
        {
            mainMenu.PlayButtonClicked();
        }
    }

    
}
