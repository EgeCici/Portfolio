using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject waypoints;
    public bool isFirstPhaseCompleted = false;
    public GameObject secondPhaseUI;
    public TextMeshProUGUI secondPhaseText;

    private bool isTimerStarted = false;
    private const float RAGE_TIME = 10f;
    private float currentRageTime = 0f;
    public int rageCount = 0;
    public int policeCount = 2;
    public GameObject policePrefab;
    public Transform policeSpawnPoint;

    private List<EnemyController> rageEnemys = new List<EnemyController>();

    public GameObject moneyPrefab;
    public Transform moneySpawnPoint;
    private float moneyTimer;
    private void Awake()
    {
        Instance = this;
    }

    public void FirstPhaseCompleted()
    {
        waypoints.SetActive(false);
        isFirstPhaseCompleted = true;
        secondPhaseUI.SetActive(true);
        isTimerStarted = true;
        currentRageTime = RAGE_TIME;
    }

    private void Update()
    {
        if (isFirstPhaseCompleted)
        {
            if (isTimerStarted)
            {
                secondPhaseText.text = $"The next police attack will be in {(int)currentRageTime} seconds!";
                currentRageTime -= Time.deltaTime;

                if (currentRageTime <= 0)
                {
                    isTimerStarted = false;
                    secondPhaseText.text = "Polices are attacking, defend yourself!";

                    for (int i = 0; i < policeCount; i++)
                    {
                        var police = Instantiate(policePrefab, policeSpawnPoint).GetComponent<EnemyController>();
                        police.isRageEnemy = true;
                        police.distance = 100;
                        rageEnemys.Add(police);
                    }
                }

                moneyTimer += Time.deltaTime;
                if (moneyTimer > 1)
                {
                    moneyTimer = 0;
                    GameObject money = Instantiate(moneyPrefab);
                    money.transform.position = moneySpawnPoint.position;
                    money.GetComponent<Rigidbody>().AddForce(Vector3.forward * Random.Range(25,100));
                }
            }
        }


    }

    public void RagePoliceDead(EnemyController police)
    {
        if (rageEnemys.Contains(police))
        {
            rageEnemys.Remove(police);

            if(rageEnemys.Count == 0)
            {
                isTimerStarted = true;
                currentRageTime = RAGE_TIME;
                
                rageCount++;

                //if (rageCount % 2 == 0)
                //    PlayerController.Instance.LoadAd();

                if (rageCount % 2 == 0)
                    policeCount++;
            }
        }
    }
}
