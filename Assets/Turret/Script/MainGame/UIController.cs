using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Player info")]
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private TMP_Text playerScoreText;

    [Header("Panel")]
    [SerializeField] private GameObject deadPanel;

    [Header("Button")]
    [SerializeField] private Button restartButton;

    private void Start()
    {
        restartButton.onClick.AddListener(Restart);
    }

    public void SetHealthBar(int newValue)
    {
        playerHealthBar.value = newValue;
        playerHealthText.text = newValue.ToString();
    }

    public void SetPlayerScore(int score)
    {
        playerScoreText.text = "Score: " + score.ToString();
    }

    public void ShowDeadPanel()
    {
        deadPanel.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
