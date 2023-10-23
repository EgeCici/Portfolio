using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CinematicManager : MonoBehaviour
{
    public static CinematicManager Instance;

    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject car;
    public GameObject player;
    public GameObject UI;

    public CharacterController playerCharacterController;
    public PlayerController playerControllerScript;

    public CinemachineVirtualCamera cinematicCam;
    public CinemachineVirtualCamera playerCam;

    public Animator playerCinematicAnim;
    public Animator carLeftDoorAnim;
    public Animator carRightDoorAnim;

    public float doorOpeningTime;


    private GameObject end;

    private void Awake()
    {
        Instance = this;

       

        playerCharacterController = player.GetComponent<CharacterController>();
        playerControllerScript = player.GetComponent<PlayerController>();

        playerCharacterController.enabled = false;
        playerControllerScript.enabled = false;

        player.transform.SetParent(car.transform);
        
        
        playerControllerScript.CloseAllGunRigs();

        UI.SetActive(false);
        
    }

    private void Start()
    {
        playerCam.Priority = 0;
        cinematicCam.Priority = 10;
    }

    private void OnEnable()
    {
        CameraSwitcher.Register(cinematicCam);
        CameraSwitcher.Register(playerCam);
    }

    private void OnDisable()
    {
        CameraSwitcher.Unregister(cinematicCam);
        CameraSwitcher.Unregister(playerCam);
    }



    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("End"))
        {

            //Car end destination trigger
            carLeftDoorAnim.SetTrigger("open");
            carRightDoorAnim.SetTrigger("open");
           
            end = other.gameObject;
            Destroy(end.gameObject);
            StartCoroutine(WaitForDoorOpening());
        }
    }

    private IEnumerator WaitForDoorOpening()
    {
        yield return new WaitForSeconds(doorOpeningTime);

        player.transform.localPosition = new Vector3(0.04f, 0.7f, -2.0f);
        playerCinematicAnim.SetTrigger("cinematic1");

        yield return WaitForPlayerAnimationEnd();
    }

    public IEnumerator WaitForPlayerAnimationEnd()
    {
        yield return new WaitForSeconds(2);

        Debug.Log("Mission Started");
        CameraSwitcher.SwitchCamera(playerCam);
        playerCam.Priority = 10;
        cinematicCam.Priority = 0;

        playerCharacterController.enabled = true;
        player.transform.SetParent(null);
        playerControllerScript.enabled = true;
        

        PlayerController.Instance.GunSetActive();
        UI.SetActive(true);


    }
}
