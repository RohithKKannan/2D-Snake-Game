using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Controls game flow, canvas
    [SerializeField] PlayerMovement snake;
    [SerializeField] CanvasController canvas;
    int score;
    void Start()
    {
        score = 0;
        canvas.UpdateScore(score);
    }
    public void FoodConsumed()
    {
        score += 10;
        snake.AddBody();
        canvas.UpdateScore(score);
    }
    public void PauseGameTrigger()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
        canvas.PauseScreenUI();
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        canvas.GameOverUI();
    }
    public void MainMenu()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        LevelManager.Instance.LoadMainMenu();
    }
    public void RestartLevel()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        LevelManager.Instance.ReloadCurrentLevel();
    }
}