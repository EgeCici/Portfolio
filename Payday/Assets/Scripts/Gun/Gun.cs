using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gunData;


    public Transform rightHandGrip;
    public Transform leftHandGrip;
    //public Rig armRig;

    private float timeSinceLastShoot;

    public int currentAmmo;

    

    public ParticleSystem muzzleFlash;
    public ParticleSystem ejectCasing;

    //public Animator akAnimator;
    //public Animator pistolAnimator;
    //public Animator mp5Animator;

    private Camera cam;

    public Animator animator;

    private void Awake()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        currentAmmo = gunData.magSize;
    }

    private bool CanShoot()
    {
        return timeSinceLastShoot > gunData.shootDelay;
    }

    public void StartReload()
    {
        if (!gunData.reloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        currentAmmo = gunData.magSize;

        gunData.reloading = false;
    }

    public void Shoot()
    {
        //if (currentAmmo > 0)
        {
            if (CanShoot())
            {
                Debug.Log("Shoot");
                //PlayerController.Instance.FindEnemy();


                muzzleFlash.Play();
                ejectCasing.Play();
                animator.SetTrigger("shoot");
                GunManager.Instance.GunSound(gunData.audioClip);
                
                //pistolAnimator.SetTrigger("shoot");
                //PlayerController.Instance.CameraRebound();
                //StartCoroutine(Recoil());

                //shooting raycasting
                RaycastHit hit;

                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity,GunManager.Instance.shootLayers))
                {
                    Debug.Log(hit.transform.name);
                    if (hit.collider.gameObject.TryGetComponent(out EnemyBodyPart enemy))
                    {
                        enemy.Damage(gunData.damage, hit.point);

                    }
                    if (hit.collider.gameObject.TryGetComponent(out NPCController npc))
                    {
                        npc.Damage(gunData.damage, hit.point);
                    }
                }
                Debug.DrawRay(cam.transform.position, cam.transform.forward * 1000, Color.red, 10);

                currentAmmo--;
                timeSinceLastShoot = 0;

            }
        }

        //if (currentAmmo <= 0)
        //{
        //    StartReload();
        //}
    }

    private void Update()
    {
        timeSinceLastShoot += Time.deltaTime;

        if (GunManager.Instance.isPressed)
        {
            Shoot();
        }
    }

}


