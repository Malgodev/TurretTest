using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public static GameManager Instance { get; private set; }

    [SerializeField] private float FPS;

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
    }
}
