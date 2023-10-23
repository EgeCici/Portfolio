using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
  

    public float enemyHealth;
    [SerializeField] Image image;
    private const float defaultHealth = 100;
    public Animator enemyAnimator;
    public Collider enemyCollider;
    public NavMeshAgent enemyNavMesh;
    public ParticleSystem blood;
    public ParticleSystem muzlleFlash;
    public ParticleSystem ejectCasing;
    public AudioSource enemyAudio;

    public bool isDead;

    public Transform lookAtTransform;

    public Animator animator;

    public bool isGettingDamage;

    public int taskIndex = -1;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        enemyHealth = defaultHealth;
        image.fillAmount = enemyHealth / 100;
        enemyAudio.GetComponentInChildren<AudioSource>();

       

    }

    


    public void Damage(int damage)
    {
        if (isDead) return;
        if (isGettingDamage) return;
        //PlayerController.Instance.CrosshairScale();
        isGettingDamage = true;
        animator.SetTrigger("damage");
        enemyHealth -= damage;
        image.fillAmount = enemyHealth / 100;
        blood.Play();
        

        if (enemyHealth <= 0)
        {
            isDead = true;
            //PlayerController.Instance.EnemyKilled();
            StartCoroutine(Dead());
            return;
        }

        StartCoroutine(DamageWaiter());

    }

    private IEnumerator DamageWaiter()
    {
        
        yield return new WaitForSeconds(0.1f);
        isGettingDamage = false;
        //PlayerController.Instance.crosshair.color = new Color(0, 255, 0);
        //PlayerController.Instance.hitCrosshair.SetActive(false);
        //PlayerController.Instance.ToggleCrosshairScale(false);
    }

   public IEnumerator Dead()
   {
        if (EnemyManager.Instance.enemies.Contains(this))
        {
            EnemyManager.Instance.enemies.Remove(this);
        }

        enemyAnimator.SetBool("dead", true);
        enemyCollider.enabled = false;
        enemyNavMesh.enabled = false;
        image.enabled = false;
        enemyAudio.Stop();
        muzlleFlash.Stop();
        ejectCasing.Stop();

        //PlayerController.Instance.ToggleCrosshairScale(false);
        yield return new WaitForSeconds(3f);
        //PlayerController.Instance.crosshair.color = new Color(0, 255, 0);

        //PlayerController.Instance.hitCrosshair.SetActive(false);

        if(taskIndex != -1)
            TaskManager.Instance.TaskProgress(taskIndex);

        Destroy(gameObject);
   }
}
