using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager Instance;

    public Gun[] activeGun;

    public Gun selectedGun;

    public bool isPressed;

    //Audio
    public AudioSource fireSource;

    public AudioClip audioClip;
   
    public LayerMask shootLayers;


    public bool clickingFireButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {


        for (int i = 0; i < activeGun.Length; i++)
        {
            if (activeGun[i].isActiveAndEnabled)
            {

                selectedGun = activeGun[i];

            }
        }

        
        
    }

    private void Update()
    {
        if(selectedGun != null && clickingFireButton)
        {
            selectedGun.Shoot();
        }
    }

    public void GunSound(AudioClip audioClip)
    {
        fireSource.PlayOneShot(audioClip);
    }

    public void Reload()
    {
        selectedGun.StartReload();
    }


    
}
