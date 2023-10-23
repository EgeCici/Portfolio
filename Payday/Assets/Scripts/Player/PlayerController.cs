using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using DG.Tweening;
using UnityEngine.Audio;
using System;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    public CharacterController controller;
    public GunManager GunManager;
    public GameObject headshotIcon;


    private float gravityValue = -9.81f;
    public VariableJoystick leftVariableJoystick;
    public VariableJoystick camJoystick;


    [Header("Animation Reference")]
    private Animator playerAnimator;
    int moveXAnimationParameterId;
    int moveZAnimationParameterId;
    [SerializeField]
    private float animationSmoothTime = 0.1f;


    [Header("Camera Reference")]
    private Camera cam;
    private float _cameraAngleY;
    public float cameraAngleSpeed = 0.2f;
    private float _cameraPosY;
    private float _cameraPosX;
    public float cameraPosSpeed = 0.01f;
    private Vector2 _cameraMovement;
    [SerializeField]
    public float cameraSpaceDistance;
    [SerializeField] FixedTouchField touchField;

    public CinemachineVirtualCamera tpsCam;
    public CinemachineVirtualCamera aimCam;


    private CinemachinePOV playerPOVCam;

    public float cameraYSpeed;
    public float cameraXSpeed;
    CinemachineImpulseSource impulse;


    public GameObject targetEnemy;
    public float TopClamp = 90.0f;
    public float BottomClamp = -90.0f;
    public Transform handle;

    //Weapon
    private int activeGun = 0;
    public Gun[] guns;
    public enum gunType { Rifle, Pistol };



    //Fx
    public ParticleSystem moneyParticle;

    //UI
    public TextMeshProUGUI moneyText;
    public GameObject UIpanel;
    public GameObject wastedPanel;
    public GameObject gunSwitchButton;
    public bool isGunSwitchButton;
    public Image crosshair;
    public GameObject hitCrosshair;
    public GameObject reviveObj;
    public bool isRevived;
    public float currentTime = 0;
    public float startingTime = 5;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI endMoneyText;
    public GameObject missionCompletePanel;

    //Get Damage
    [SerializeField] float playerHealth;
    [SerializeField] Image healthbarImg;
    [SerializeField] Image bloodImg;

    private float defaultHealth
    {
        get
        {
            return isUsingGiftCharacter ? meshes[giftCharIndex].data.health : meshes[DataManager.Instance.currentEquippedCharacterIndex].data.health;
        }
    }
    private float playerSpeed
    {
        get
        {
            return isUsingGiftCharacter ? meshes[giftCharIndex].data.speed : meshes[DataManager.Instance.currentEquippedCharacterIndex].data.speed;
        }
    }

    public Collider playerCollider;
    public ParticleSystem blood;
    public GameObject weaponHandler;

    public bool isDead;
    public bool isGettingDamage;
    public bool isReviveButtonPressed;


    private int collectedMoney;
    public int CollectedMoney
    {
        get
        {
            return collectedMoney;
        }
        set
        {
            collectedMoney = value;
            OnMoneyChanged();
        }
    }

    private float gravityY = 0;

    public GameObject collectButton;
    public bool collectButtonPressed = false;

    //Vault
    public GameObject vault;
    public ParticleSystem explosionVfx;
    public AudioSource explosionSfx;



    //Grenade
    //public GameObject grenade;
    //public Transform throwingPose;
    //public float upForce, forwardForce;
    //public LineRenderer lineRenderer;
    //private Rigidbody grenadeRb;
    //private GameObject throwableObject;
    //private LayerMask GrenadeCollisionMask;

    //public bool canThrowGrenade;
    //public bool clickingGrenadeButton;
    //public int grenadeCount = 5;

    // Display controls to draw grenade projectile
    [Header("Display Controls")]
    [SerializeField]
    [Range(10, 100)]
    private int LinePoints = 25;
    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float TimeBetweenPoints = 0.1f;


    [Header("Character Meshes")]
    //public SkinnedMeshRenderer currentPlayerMesh;

    [SerializeField] AudioSource walkAudioSource;
    [SerializeField] AudioClip collectMoneyAudioClip;
    [SerializeField] AudioClip taskCompletedSound;
    public enum PlayerCameras
    {
        TPS,
        Aim
    }

    public PlayerCameras currentActiveCam;

    [Header("Grenade")]
    public Transform grenadeSpawnPoint;
    public GameObject grenade;
    public float range = 10f;
    public Button grenadeThrowButton;
    public bool canThrowGrenade;
    public bool clickingGrenadeButton;
    public LineRenderer lineRenderer;
    public float upForce, forwardForce;
    private LayerMask GrenadeCollisionMask;
    public Image grenadeLogoImg;
    private const float initial_granade_count = 5;
    private float _grenadeCount;
    public float CurrentGrenadeCount { get => _grenadeCount; set { _grenadeCount = value; GrenadeCountChanged(); } }
    [SerializeField] TextMeshProUGUI granadeCountText;

    [Header("Tasks")]
    public Task firstTask;
    public Task collectableTask;

    private RectTransform rectCrosshair;


    private bool isGameFinished;

    private bool dontGetDamage;

    [SerializeField] AudioMixer audioMixer;
    public GameObject damageAmountText;


    public PlayerMesh[] meshes;
    public Transform weaponHolder;
    public Transform ikRightHint;

    private RigBuilder rigBuilder;
    private Rig gunRig;
    private TwoBoneIKConstraint leftHandIK;
    private TwoBoneIKConstraint rightHandIK;


    private bool isUsingGiftCharacter = false;
    private int giftCharIndex = -1;

    private void Awake()
    {
        Instance = this;

        dontGetDamage = false;
        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");

        playerPOVCam = tpsCam.GetCinemachineComponent<CinemachinePOV>();

        tpsCam.Priority = 10;


        if (DataManager.Instance != null)
        {
            int index = DataManager.Instance.currentEquippedCharacterIndex;
            if(DataManager.Instance.giftCharacterIndex != -1)
            {
                index = DataManager.Instance.giftCharacterIndex;
                giftCharIndex = index;
                DataManager.Instance.giftCharacterIndex = -1;
                isUsingGiftCharacter = true;
            }
                
            PlayerMesh meshToUse = meshes[index];

            foreach (var item in meshes)
            {
                item.parent.gameObject.SetActive(false);
            }

            meshToUse.parent.gameObject.SetActive(true);
            //playerAnimator.avatar = meshToUse.avatar;
            meshToUse.meshRenderer.sharedMesh = meshToUse.mesh;
            weaponHolder.SetParent(meshToUse.parent);
            //ikRightHint.SetParent(meshToUse.parent);

        }

        playerHealth = defaultHealth;

        if (DataManager.Instance.giftGunIndex != -1)
        {
            activeGun = DataManager.Instance.giftGunIndex;
            DataManager.Instance.giftGunIndex = -1;
        }
        else
        {
            activeGun = DataManager.Instance.currentEquippedGunIndex;
        }


        CurrentGrenadeCount = initial_granade_count;

        rectCrosshair = crosshair.GetComponent<RectTransform>();


    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        impulse = transform.GetComponent<CinemachineImpulseSource>();

        moneyText.text = collectedMoney + "$";


        firstTask.StartTask();

        currentTime = startingTime;

        playerAnimator = GetComponentInChildren<Animator>();
        rigBuilder = GetComponentInChildren<RigBuilder>();
        gunRig = rigBuilder.layers[0].rig;
        rightHandIK = gunRig.transform.GetChild(0).GetComponent<TwoBoneIKConstraint>();
        leftHandIK = gunRig.transform.GetChild(1).GetComponent<TwoBoneIKConstraint>();

        GunSetActive();

    }

    private void OnEnable()
    {
        if (DataManager.Instance != null)
            DataManager.Instance.SaveData();

        if(GoogleAdMobController.Instance != null)
        {
            GoogleAdMobController.Instance.OnAdClosedEvent.AddListener(InterstitialAdClosed);
            GoogleAdMobController.Instance.OnUserEarnedRewardEvent.AddListener(OnUserEarnedRewardEvent);
        }

    }

    private void InterstitialAdClosed()
    {
        GoogleAdMobController.Instance.RequestAndLoadInterstitialAd();
    }

    private void OnDisable()
    {
        if (DataManager.Instance != null)
            DataManager.Instance.SaveData();


        if(GoogleAdMobController.Instance != null)
        {
            GoogleAdMobController.Instance.OnAdClosedEvent.RemoveListener(InterstitialAdClosed);
            GoogleAdMobController.Instance.OnUserEarnedRewardEvent.RemoveListener(OnUserEarnedRewardEvent);
        }

    }

    public void OnUserEarnedRewardEvent()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("rewive_rewarded_confirmed");
        Revive();
    }

    public void GunSwitch()
    {
        //oldActiveGun = activeGun;
        activeGun += 1;

        if (activeGun >= guns.Length)
        {
            activeGun = 0;

        }

        GunSetActive();

    }

    public void GunSetActive()
    {

        foreach (var item in guns)
        {
            item.gameObject.SetActive(false);
            //item.armRig.weight = 0;
        }

        //guns[oldActiveGun].gameObject.SetActive(false);
        guns[activeGun].gameObject.SetActive(true);

        gunRig.weight = 1;
        //gunRig2.weight = 1;

        //guns[oldActiveGun].armRig.weight = 0;


        rightHandIK.data.target = guns[activeGun].rightHandGrip;
        leftHandIK.data.target = guns[activeGun].leftHandGrip;

        rigBuilder.Build();

        GunManager.selectedGun = guns[activeGun];

    }

    public void CloseAllGunRigs()
    {
        gunRig.weight = 0;
        //gunRig2.weight = 0;
    }

    void Update()
    {
        if (isGameFinished) return;

        #region movement
        bool isGrounded = controller.isGrounded;

        if (isGrounded == false)
        {
            gravityY += gravityValue * Time.deltaTime;
        }
        else
        {
            gravityY = 0f;
        }

        Vector3 direction = (cam.transform.forward * leftVariableJoystick.Vertical + cam.transform.right * leftVariableJoystick.Horizontal);
        direction.y = gravityY;
        controller.Move(direction * Time.deltaTime * playerSpeed);

        if (direction != Vector3.zero)
        {
            gameObject.transform.forward = direction;

        }

        bool isRunning = Mathf.Abs(leftVariableJoystick.Vertical) + Mathf.Abs(leftVariableJoystick.Horizontal) > 0;
        guns[activeGun].animator.SetBool("isRunning", isRunning);

        //Animations
        //currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, handle.localPosition, ref animationVelocity, animationSmoothTime);
        playerAnimator.SetFloat(moveXAnimationParameterId, leftVariableJoystick.Horizontal);
        playerAnimator.SetFloat(moveZAnimationParameterId, leftVariableJoystick.Vertical);


        //Camera
        var CharacterRotation = cam.transform.rotation;
        CharacterRotation.x = 0;
        CharacterRotation.z = 0;

        transform.rotation = CharacterRotation;

        float sensibility = DataManager.Instance != null ? DataManager.Instance.currentSensibilityValue : 1;
        if (Mathf.Abs(camJoystick.Vertical) + Mathf.Abs(camJoystick.Horizontal) <= 0)
        {
            
            playerPOVCam.m_VerticalAxis.Value -= touchField.TouchDist.y * Time.deltaTime * cameraYSpeed * sensibility;

            playerPOVCam.m_HorizontalAxis.Value += touchField.TouchDist.x * Time.deltaTime * cameraXSpeed * sensibility;
        }
        else
        {
            playerPOVCam.m_VerticalAxis.Value -= camJoystick.Vertical * Time.deltaTime * cameraYSpeed * sensibility * 8;

            playerPOVCam.m_HorizontalAxis.Value += camJoystick.Horizontal * Time.deltaTime * cameraXSpeed * sensibility * 8;
        }
        #endregion
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GunSwitch();
        }
#endif
    }




    //Collectible 
    private Jewels lastJewels;
    private Money lastMoney;
    private HealthKit lastHealthKit;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Money"))
        {
            lastMoney = other.gameObject.GetComponent<Money>();
            collectButton.SetActive(true);

        }

        if (other.gameObject.CompareTag("HealthKit"))
        {
            lastHealthKit = other.gameObject.GetComponent<HealthKit>();
            collectButton.SetActive(true);

        }


        if (other.gameObject.CompareTag("Jewels"))
        {
            lastJewels = other.gameObject.GetComponent<Jewels>();
            collectButton.SetActive(true);
        }



        if (other.gameObject.CompareTag("Vault"))
        {

            vault.transform.DOLocalRotate(new Vector3(0, 90, 0), 1f);
            explosionVfx.Play();
            explosionSfx.Play();


        }
        //if (other.gameObject.CompareTag("LastWaypoint"))
        //{

        //    MissionCompleted();
        //}

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Money"))
        {
            lastMoney = other.gameObject.GetComponent<Money>();
            collectButton.SetActive(true);

        }

        if (other.gameObject.CompareTag("HealthKit"))
        {
            lastHealthKit = other.gameObject.GetComponent<HealthKit>();
            collectButton.SetActive(true);

        }


        if (other.gameObject.CompareTag("Jewels"))
        {
            lastJewels = other.gameObject.GetComponent<Jewels>();
            collectButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {


        if (other.CompareTag("Jewels"))
        {
            collectButton.SetActive(false);
        }

        if (other.CompareTag("Money"))
        {
            collectButton.SetActive(false);
        }

        if (other.CompareTag("HealthKit"))
        {
            collectButton.SetActive(false);
        }
    }

    public void ReviveButtonPressed()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("rewive_rewarded_clicked");
        GoogleAdMobController.Instance.ShowRewardedAd();
        GoogleAdMobController.Instance.RequestAndLoadRewardedAd();
    }

    public void OnCollectButtonPressed()
    {
        if (collectButton.activeSelf)
        {
            bool isMoney = false;

            if (lastJewels != null)
            {
                isMoney = true;
                lastJewels.Collect();
                collectButton.SetActive(false);
                CollectedMoney += 10;
            }
            if (lastMoney != null)
            {
                isMoney = true;
                lastMoney.Collect();
                collectButton.SetActive(false);
                CollectedMoney += 5;
            }
            if (lastHealthKit != null)
            {
                lastHealthKit.Collect();

                playerHealth += lastHealthKit.increaseHealthValue;
                if (playerHealth > defaultHealth)
                {
                    playerHealth = defaultHealth;
                }
                collectButton.SetActive(false);
                healthbarImg.fillAmount = playerHealth / defaultHealth;

            }


            collectButtonPressed = true;

            gunRig.weight = 0;
            //gunRig2.weight = 0;

            TaskManager.Instance.TaskProgress(collectableTask.Index);
            StartCoroutine(CollectableWaiter(isMoney));


        }
        else
        {
            collectButtonPressed = false;
        }

    }

    private IEnumerator CollectableWaiter(bool isMoney)
    {

        playerAnimator.SetTrigger("PickUP");
        if (isMoney)
        {
            moneyParticle.Play();
            walkAudioSource.PlayOneShot(collectMoneyAudioClip);
        }
        else
        {
            walkAudioSource.PlayOneShot(healthKitAudioClip);
        }

        yield return new WaitForSeconds(2f);

        //collectButton.SetActive(false);

        gunRig.weight = 1;
    }

    public void Damage(int damage)
    {
        //#if UNITY_EDITOR
        //        return;
        //#endif

        if (isDead) return;
        if (dontGetDamage) return;

        playerHealth -= damage;
        healthbarImg.fillAmount = playerHealth / defaultHealth;
        blood.Play();
        Shake();

        if (isReviveButtonPressed)
        {
            isGettingDamage = true;

        }

        if (playerHealth <= 0)
        {
            StartCoroutine(Dead());

        }

        // StartCoroutine(DamageWaiter());
    }


    public void EnemyKilled()
    {
        //if (task.isOpened)
        //{
        //    task.goal.EnemyKilled();

        //    if (task.goal.IsReached())
        //    {
        //        DataManager.Instance.MoneyAmount += task.moneyReward;
        //        task.Complete();
        //    }
        //}
    }

    IEnumerator Dead()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("dead");
        DataManager.Instance.MoneyAmount += CollectedMoney;
        wastedPanel.SetActive(true);
        UIpanel.SetActive(false);

        isDead = true;
        playerAnimator.SetBool("dead", true);
        playerCollider.enabled = false;
        reviveObj.SetActive(true);
        CloseAllGunRigs();

        weaponHandler.SetActive(false);
        healthbarImg.enabled = false;
        int time = 5;
        while (time > 0)
        {
            countdownText.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time--;

        }

        if (!isRevived)
        {
            Wasted();
        }

    }

    public void Wasted()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
                    LoadAd();
#endif
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        wastedPanel.SetActive(false);
    }


    public void Revive()
    {
        playerAnimator.SetBool("dead", false);
        playerAnimator.SetBool("idle", true);

        playerCollider.enabled = true;
        wastedPanel.SetActive(false);
        weaponHandler.SetActive(true);
        healthbarImg.enabled = true;
        isRevived = true;
        isDead = false;
        isReviveButtonPressed = true;

        UIpanel.SetActive(true);

        dontGetDamage = true;

        reviveObj.SetActive(false);

        playerHealth = 100;
        healthbarImg.fillAmount = playerHealth;
        GunSetActive();

        StartCoroutine(ReviveWaiter());


    }

    IEnumerator ReviveWaiter()
    {
        yield return new WaitForSeconds(5f);

        isReviveButtonPressed = false;
        dontGetDamage = false;

    }

    private float camLastYValue;
    public void CameraRebound()
    {
        StartCoroutine(CameraReboundCoroutine());
    }

    private IEnumerator CameraReboundCoroutine()
    {
        camLastYValue = playerPOVCam.m_VerticalAxis.Value;

        float timer = 0f;
        float duration = .1f;
        float targetValue = camLastYValue - .5f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            playerPOVCam.m_VerticalAxis.Value = Mathf.Lerp(camLastYValue, targetValue, timer / duration);
            yield return new WaitForEndOfFrame();
        }
    }

    public void Shake()
    {
        impulse.GenerateImpulse();
    }

    public void ToggleAimAssist(bool value)
    {
        //if (value)
        //{
        //    aimCam.Priority = 11;
        //    tpsCam.Priority = 9;
        //}
        //else
        //{
        //    aimCam.Priority = 9;
        //    tpsCam.Priority = 11;
        //}
    }
    private float minDistance = 2; // minimum distance to stop rotation if you get close to target
    private float maxDistance = 30;
    private float maxAngle = 60;



    public void OnMoneyChanged()
    {
        moneyText.text = CollectedMoney + "$";
    }


    public void MissionCompleted()
    {
        Debug.Log("Mission Completed!");
        //isFirstPhaseCompleted = true;
        //secondPhaseUI.gameObject.SetActive(true);

        //Firebase.Analytics.FirebaseAnalytics.LogEvent("win");
        //isGameFinished = true;
        //leftVariableJoystick.enabled = false;
        //playerAnimator.SetFloat(moveXAnimationParameterId, 0);
        //playerAnimator.SetFloat(moveZAnimationParameterId, 0);
        ////TODO: VFX SFX Animation
        //if (DataManager.Instance != null)
        //{
        //    DataManager.Instance.MoneyAmount += CollectedMoney;


        //}
        //endMoneyText.text = CollectedMoney.ToString();
        //endMoneyText.enabled = true;
        //missionCompletePanel.SetActive(true);
        //UIpanel.SetActive(false);

    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        missionCompletePanel.SetActive(false);

    }

    private float smallCrosshairScale = 50;
    private float bigCrosshairScale = 100;

    private bool isCrosshairScaling;
    public void CrosshairScale()
    {
        if (isCrosshairScaling)
        {
            StopCoroutine(ChangeCrosshairScaleInTime(rectCrosshair.rect.width));
        }
        StartCoroutine(ChangeCrosshairScaleInTime(rectCrosshair.rect.width));
    }

    private IEnumerator ChangeCrosshairScaleInTime(float current)
    {
        isCrosshairScaling = true;
        rectCrosshair.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, smallCrosshairScale);
        rectCrosshair.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, smallCrosshairScale);

        float timer = 0f;
        float duration = .25f;
        crosshair.color = new Color(1, 1, 1, 0.4f);
        hitCrosshair.SetActive(true);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            rectCrosshair.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(current, bigCrosshairScale, timer / duration));
            rectCrosshair.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(current, bigCrosshairScale, timer / duration));
            yield return new WaitForEndOfFrame();
        }

        timer = 0;
        hitCrosshair.SetActive(false);
        while (timer < duration)
        {
            timer += Time.deltaTime;
            rectCrosshair.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(current, smallCrosshairScale, timer / duration));
            rectCrosshair.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(current, smallCrosshairScale, timer / duration));
            yield return new WaitForEndOfFrame();
        }

        crosshair.color = new Color(1, 1, 1, 1f);
        isCrosshairScaling = false;
    }

    //private void DrawGrenadeProjection()
    //{
    //    lineRenderer.enabled = true;
    //    lineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
    //    Debug.Log(lineRenderer.positionCount);
    //    Vector3 startPosition = grenadeSpawnPoint.transform.position;

    //    float realForce = (float)Math.Sqrt((float)Math.Pow(forwardForce, 2) + (float)Math.Pow(upForce, 2));
    //    Vector3 startVelocity = realForce * transform.forward;
    //    int i = 0;
    //    lineRenderer.SetPosition(i, startPosition);
    //    for (float time = 0; time < LinePoints; time += TimeBetweenPoints)
    //    {
    //        i++;
    //        Vector3 point = startPosition + time * startVelocity;
    //        point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

    //        lineRenderer.SetPosition(i, point);

    //        Vector3 lastPosition = lineRenderer.GetPosition(i - 1);

    //        if (Physics.Raycast(lastPosition,
    //            (point - lastPosition).normalized,
    //            out RaycastHit hit,
    //            (point - lastPosition).magnitude,
    //            GrenadeCollisionMask))
    //        {
    //            lineRenderer.SetPosition(i, hit.point);
    //            lineRenderer.positionCount = i + 1;
    //            return;
    //        }
    //    }
    //}
    //public LayerMask grenadeThrowControlLayerMask;
    //public void ThrowGrenade()
    //{


    //    RaycastHit hit;

    //    if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f, grenadeThrowControlLayerMask))
    //        transform.LookAt(hit.point);

    //    throwableObject.transform.SetParent(transform.parent);

    //    grenadeRb.isKinematic = false;
    //    grenadeRb.AddForce(cam.transform.forward * forwardForce, ForceMode.Impulse);
    //    grenadeRb.AddForce(cam.transform.up * upForce, ForceMode.Impulse);
    //    canThrowGrenade = false;
    //    grenadeCount--;

    //    if (grenadeCount != 0)
    //        StartCoroutine(InstantiateGrenade());
    //}

    //IEnumerator InstantiateGrenade()
    //{
    //    yield return new WaitForSeconds(3f);

    //    var newBottle1 = Instantiate(grenade, throwingPose.position, throwingPose.rotation, this.transform);
    //    canThrowGrenade = true;
    //    throwableObject = newBottle1;
    //    grenadeRb = throwableObject.GetComponent<Rigidbody>();
    //}

    //Grenade Launch Method

    public void GrenadeButtonClicked()
    {
        if (CurrentGrenadeCount > 0)
        {
            playerAnimator.SetTrigger("throw");
            CloseAllGunRigs();
            StartCoroutine(WaitForGrenade());
            CurrentGrenadeCount--;
        }
    }
    public void Launch()
    {
        GameObject grenadeInstance = Instantiate(grenade, grenadeSpawnPoint.position, Quaternion.identity);
        grenadeInstance.GetComponent<Rigidbody>().AddForce(cam.transform.forward * range, ForceMode.Impulse);
    }

    private void GrenadeCountChanged()
    {
        granadeCountText.text = $"{CurrentGrenadeCount}/{initial_granade_count}";
        if (CurrentGrenadeCount == 0)
        {
            grenadeLogoImg.color = new Color(1, 1, 1, 0.4f);
        }
    }

    private IEnumerator WaitForGrenade()
    {
        yield return new WaitForSeconds(2f);
        GunSetActive();
    }


    //Animation event, dont touch this
    public void Foot()
    {
        walkAudioSource.Play();
    }

    public void PlayTaskCompletedSound()
    {
        walkAudioSource.PlayOneShot(taskCompletedSound);
    }

    [SerializeField] Slider soundSlider;
    [SerializeField] Slider sensibilitySlider;
    public AudioClip healthKitAudioClip;

    public void SoundValueChanged()
    {
        float value = soundSlider.value;
        audioMixer.SetFloat("volume", value);
        DataManager.Instance.currentSoundVolume = soundSlider.value;
    }

    public void SensibilityValueChanged()
    {
        DataManager.Instance.currentSensibilityValue = sensibilitySlider.value;
    }

    public void LoadAd()
    {
        Debug.Log("ad");
        GoogleAdMobController.Instance.ShowInterstitialAd();
    }

    public void OpenHeadShotIcon()
    {
        headshotIcon.SetActive(true);
        StartCoroutine(CloseHSIcon());
    }

    private IEnumerator CloseHSIcon()
    {
        yield return new WaitForSeconds(.4f);
        headshotIcon.SetActive(false);
    }
}


[Serializable]
public class PlayerMesh
{
    public Transform parent;
    public Mesh mesh;
    public int index;
    public CharacterDisplayData data;
    public SkinnedMeshRenderer meshRenderer;
}
