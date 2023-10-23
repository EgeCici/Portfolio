using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialMapController : MonoBehaviour
{
    public static TutorialMapController Instance;

    [SerializeField] TextMeshProUGUI defaultText;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject bg;
    [SerializeField] FixedTouchField cameraRotateField;
    [SerializeField] VariableJoystick movementJoystick;
    [SerializeField] Image shootIconImg;
    [SerializeField] Color shootIconColor;

    private float cameraRotateAmount;
    private bool lookForCameraRotation;

    private float movementAmount;
    private bool lookForMovement;
    private bool lookForShoot;


    private void Awake()
    {
        //if(Instance == null)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        bg.SetActive(true);
        playButton.SetActive(false);
        defaultText.color = new Color(1, 1, 1, 0);

        StartCoroutine(StartTutorial());
    }

    private IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(1);
        float textStartTime = 0;
        float duration = 1f;
        while(textStartTime < duration)
        {
            textStartTime += Time.deltaTime;
            defaultText.color = new Color(1, 1, 1, textStartTime / duration);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(.5f);

        string sentence = "Our objective is rob the market, press play to continue.";

        char[] sentenceArray = sentence.ToCharArray();

        int sentenceIndex = 0;

        defaultText.text = "";

        while(sentenceIndex < sentenceArray.Length)
        {
            defaultText.text += sentenceArray[sentenceIndex];
            sentenceIndex++;
            yield return new WaitForSeconds(.1f);
        }

        playButton.SetActive(true);


    }

    public void PlayButtonPressed()
    {
        bg.SetActive(false);

        //Texti yukariya tasi

        defaultText.color = new Color(0, 0, 0, 1);
        defaultText.text = "Use your finger for rotate the camera.";
        lookForCameraRotation = true;
    }


    private void Update()
    {
        if (lookForCameraRotation)
        {
            cameraRotateAmount += Mathf.Abs(cameraRotateField.TouchDist.x) + Mathf.Abs(cameraRotateField.TouchDist.y);


            if(cameraRotateAmount > 200)
            {
                defaultText.text = "Perfect! Now use joystick for movement.";
                lookForCameraRotation = false;
                lookForMovement = true;
            }
            return;
        }

        if (lookForMovement)
        {
            movementAmount += Mathf.Abs(movementJoystick.Vertical) + Mathf.Abs(movementJoystick.Horizontal);

            if(movementAmount > 25)
            {
                defaultText.text = "Great! It's time to shoot now. Press the bullet icon for shoot.";
                lookForMovement = false;
                lookForShoot = true;
                shootIconImg.color = shootIconColor;
            }

            return;
        }

    }

    public void ShootButtonPressed()
    {
        if (lookForShoot)
        {
            defaultText.text = "Excellent! Now you can kill your enemies, walk towards the area marked with yellow.";
            lookForShoot = false;
        }
    }
}
