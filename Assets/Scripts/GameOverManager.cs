using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

    public GameObject gameOverScreen;
    public Text secondsSurvivedUI;
    public bool gameOver;

    float secondsSurvived;


    void Start() {
        FindObjectOfType<GridManager>().IsFull += OnGameOver;
    }

    void Update() {
        if (gameOver) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                // Reload game
                SceneManager.LoadScene(0);
            }
        }
    }

    void OnGameOver() {
        gameOverScreen.SetActive(true);
        secondsSurvived = Mathf.Round(Time.timeSinceLevelLoad * 100.0f) / 100.0f;
        secondsSurvivedUI.text = secondsSurvived.ToString();
        gameOver = true;
    }
}
