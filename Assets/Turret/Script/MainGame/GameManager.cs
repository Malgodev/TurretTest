using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public static GameManager Instance { get; private set; }

    [field: SerializeField] public PlayerController PlayerController { get; private set; }
    [field: SerializeField] public UIController UIController { get; private set; }

    public GameObject enemyPrefab;

    [SerializeField] private float FPS;
    [SerializeField] private int score = 0;

    [SerializeField] private float SpawnTimer = 5f;
    private float lastSpawnTime = 5;

    private bool isDead = false;

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

    private void Update()
    {
        FPS = 1f / Time.deltaTime;

        lastSpawnTime += Time.deltaTime;

        if (lastSpawnTime > SpawnTimer && !isDead)
        {
            lastSpawnTime = 0;

            Instantiate(enemyPrefab, GetRandomPosition(14f), Quaternion.identity);

            SpawnTimer = Mathf.Max(SpawnTimer - 0.1f, 1);
        }
    }

    public static Vector3 GetRandomPosition(float minValue)
    {
        float randomX;
        float randomY;

        do
        {
            randomX = Random.Range(-15f, 15f);
            randomY = Random.Range(-15f, 15f);
        }
        while (Mathf.Abs(randomX) <= minValue || Mathf.Abs(randomY) <= minValue);

        return new Vector3(randomX, 1.5f, randomY);
    }

    public void IncreasePoint()
    {
        UIController.SetPlayerScore(++score);
    }

    public void Dead()
    {
        Time.timeScale = 0f;

        UIController.ShowDeadPanel();
    }
}
