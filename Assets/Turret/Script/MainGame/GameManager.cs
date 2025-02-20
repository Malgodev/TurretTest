using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public static GameManager Instance { get; private set; }
    [SerializeField] private float FPS;

    [field: Header("Controller")]
    [field: SerializeField] public PlayerController PlayerController { get; private set; }
    [field: SerializeField] public TurretUpgradeController TurretUpgradeController { get; private set; }
    [field: SerializeField] public UIController UIController { get; private set; }


    [Header("Prefab")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Game Stat")]
    [SerializeField] private int score = 0;
    [SerializeField] private int enemyCount = 0;

    [SerializeField] private float SpawnTimer = 5f;
    private float lastSpawnTime = 5;

    public Action OnEnemyKilled;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        PlayerController.OnPlayerDead += PlayerController_OnTurretDead;
        PlayerController.OnPlayerHealthChange += PlayerController_OnGettingDamage;

        OnEnemyKilled += IncreasePoint;
    }

    private void PlayerController_OnGettingDamage(int health)
    {
        UIController.SetHealthBar(health);
    }

    private void PlayerController_OnTurretDead()
    {
        Time.timeScale = 0f;

        UIController.ShowDeadPanel();
    }

    private void Update()
    {
        FPS = 1f / Time.deltaTime;

        lastSpawnTime += Time.deltaTime;

        if (lastSpawnTime > SpawnTimer && enemyCount <= 10)
        {
            lastSpawnTime = 0;
            SpawnTimer = Mathf.Max(SpawnTimer - 0.1f, 1);

            Instantiate(enemyPrefab, GetRandomPosition(14f), Quaternion.identity);

            enemyCount++;
        }
    }

    public static Vector3 GetRandomPosition(float minValue)
    {
        float randomX;
        float randomY;

        do
        {
            randomX = UnityEngine.Random.Range(-15f, 15f);
            randomY = UnityEngine.Random.Range(-15f, 15f);
        }
        while (Mathf.Abs(randomX) <= minValue || Mathf.Abs(randomY) <= minValue);

        return new Vector3(randomX, 1.5f, randomY);
    }

    public void IncreasePoint()
    {
        enemyCount--;

        UIController.SetPlayerScore(++score);

        if (score % 10 == 0)
        {
            TurretUpgradeController.SelectUpgrade();
        }
    }
}

