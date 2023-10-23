using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    //[Header("Dedect")]
    //[SerializeField] LayerMask playerLayer;
    //[SerializeField] float lookRange;

    [Header("Vision")]
    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;
    public Color meshColor = Color.red;
    public int scanFrequency = 30;
    public LayerMask targetLayer;
    public LayerMask occlusionLayers;

    private Collider[] colliders = new Collider[1];
    private int count;
    [HideInInspector]public float scanInterval;
    private List<GameObject> objects = new List<GameObject>();

    private Mesh sensorMesh;

    private EnemyFSM stateMachine;

    [SerializeField] GameObject healthBarObj;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public NavMeshAgent agent;

    [Header("Shoot")]
    public Transform player;
    public float shootDelay;

    public bool CanShoot;

    [Header("VFX-SFX")]
    public ParticleSystem bloodParticle;
    public ParticleSystem muzzleParticle;
    public AudioSource gunShootAudioSource;

    public Image healthBar;

    private const float INITIAL_HEALTH = 100;

    public float currentHealth;
    public bool isDead;


    //public bool SHOOT_WHEN_PLAYER_INSIGHT;
    public string currentState;

    public int shootPower;

    public int taskIndex = -1;

    private float damageTimer;

    [SerializeField] Transform damageTextPos;

    private bool isGettingDamageForFirstTime = true;
    [SerializeField] float attackCallCastRadius;
    [SerializeField] LayerMask attackCallLayerMask;


    private List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>();
    [SerializeField] Collider bodyCollider;
    [SerializeField] Collider headCollider;

    private Camera mainCam;

    public bool isRageEnemy;
    private void Awake()
    {

        mainCam = Camera.main;

        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            ragdollRigidbodies.Add(item);
            item.isKinematic = true;
        }



        player = GameObject.FindGameObjectWithTag("Player").transform;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        scanInterval = 1.0f / scanFrequency;

        CanShoot = true;
        stateMachine = new EnemyFSM(this);

        currentHealth = INITIAL_HEALTH;

        healthBarObj.SetActive(false);

        isGettingDamageForFirstTime = true;
    }


    private void Update()
    {
        //float xVelocity = Mathf.Clamp(agent.velocity.x, -1, 1);
        //float zVelocity = Mathf.Clamp(agent.velocity.z, -1, 1);
        //animator.SetFloat("MoveX", xVelocity);
        //animator.SetFloat("MoveZ", zVelocity);
        stateMachine.RunUpdate();
        currentState = stateMachine.CurrentState.ToString();

        damageTimer += Time.deltaTime;
        if(damageTimer > 3 & healthBarObj.activeSelf)
        {
            healthBarObj.SetActive(false);
        }
    }

    public void Damage(int value, Vector3 contactPoint)
    {
        if (isDead) return;
        PlayerController.Instance.CrosshairScale();
        damageTimer = 0;
        healthBarObj.SetActive(true);
        animator.SetTrigger("damage");
        bloodParticle.Play();
        currentHealth -= value;
        var txt = Instantiate(PlayerController.Instance.damageAmountText).GetComponent<DamageAmounTextController>();
        txt.SetText(value.ToString());
        txt.transform.position = damageTextPos.position;

        if (isGettingDamageForFirstTime)
        {
            isGettingDamageForFirstTime = false;
            //stateMachine.ChangeState(stateMachine.moveState);

            Collider[] enemies = Physics.OverlapSphere(transform.position, attackCallCastRadius, attackCallLayerMask);

            foreach (var item in enemies)
            {
                item.transform.GetComponent<EnemyBodyPart>().enemyController.AttackCallResponse();
            }
        }


        if (currentHealth <= 0)
        {
            Dead(contactPoint);
        }

        healthBar.fillAmount = currentHealth / INITIAL_HEALTH;



    }

    public void AttackCallResponse()
    {
        isGettingDamageForFirstTime = false;
        stateMachine.ChangeState(stateMachine.moveState);
    }


    private void Dead(Vector3 contactPoint)
    {
        if (isRageEnemy)
        {
            GameManager.Instance.RagePoliceDead(this);
        }
        Destroy(GetComponent<Waypoint_Indicator>());
        isDead = true;

        animator.enabled = false;
        agent.enabled = false;
        bodyCollider.enabled = false;
        headCollider.enabled = false;

        foreach (var item in ragdollRigidbodies)
        {
            item.isKinematic = false;
        }

        Vector3 dir = transform.position - mainCam.transform.position;
        dir.y = 1;
        dir.Normalize();

        Rigidbody hitRb = ragdollRigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, contactPoint)).First();

        hitRb.AddForceAtPosition(dir * 150, contactPoint, ForceMode.Impulse);

        stateMachine.ChangeState(stateMachine.deadState);

        if (taskIndex != -1)
            TaskManager.Instance.TaskProgress(taskIndex);
        Destroy(gameObject, 5);
    }

    public Vector3 GetRandomPositionInPlayerView()
    {
        /*
        float playerX = player.position.x;
        float playerZ = player.position.z;

        float x = UnityEngine.Random.Range(playerX - 2, playerX + 2);
        float z = UnityEngine.Random.Range(playerZ + 5, playerZ + 8);

        return new Vector3(x, 0, z);*/

        float x = UnityEngine.Random.Range(-1.5f, 1.5f);
        float z = UnityEngine.Random.Range(-1.5f, 1.5f);
        Vector3 playerPos = new Vector3(player.position.x + x, player.position.y, player.position.z + z);
        Vector3 pos = playerPos + player.transform.forward * 3;

        return pos;

    }
    public void Shoot()
    {
        if (CanShoot == false) //protection
            return;

        animator.SetTrigger("shoot");
        CanShoot = false;
        muzzleParticle.Play();
        gunShootAudioSource.Play();
        PlayerController.Instance.Damage(shootPower);

        StopAllCoroutines();
        StartCoroutine(ShootDelay());
    }

    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shootDelay);
        CanShoot = true;
    }

    public bool Scan()
    {
        Collider[] player = Physics.OverlapSphere(transform.position, distance, targetLayer);
        if (player.Length > 0)
            return true;

        return false;
        //count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, targetLayer, QueryTriggerInteraction.Collide);

        ////objects.Clear();

        //for (int i = 0; i < count; i++)
        //{
        //    GameObject obj = colliders[i].gameObject;
        //    if (IsInSight(obj))
        //    {
        //        //objects.Add(obj);
        //        return obj.transform;
        //    }
        //}

       // return null;
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;

        if(direction.y < - height || direction.y > height)
        {
            return false;
        }

        direction.y = 0;

        float deltaAngle = Vector3.Angle(direction, transform.forward);

        if(deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        dest.y = origin.y;

        if (Physics.Linecast(origin, dest, occlusionLayers))
            return false;
        



        return true;
    }


    private Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;

        //left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;

        for (int i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }




        for (int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;

        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();


        return mesh;
    }

    private void OnValidate()
    {
        scanInterval = 1.0f / scanFrequency;
        sensorMesh = CreateWedgeMesh();
    }
    private void OnDrawGizmos()
    {
        if (sensorMesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(sensorMesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 1);
        }
    }


}

