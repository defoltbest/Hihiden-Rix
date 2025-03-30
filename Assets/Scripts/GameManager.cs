using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct LevelGroup
{
    public string mainLevel;
    public List<string> childLevels;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Tooltip("Список основных уровней и их дочерних уровней")]
    public List<LevelGroup> levelGroups;

    [Tooltip("Сцена заставки")]
    public string splashScreenScene;

    [Tooltip("Стартовая сцена")]
    public string startScene;

    [Tooltip("Окно с концом игры")]
    public string gameOverScene;

    private int currentMainLevelIndex = 0;
    private int currentChildLevelIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadStartScene();
    }

    public void LoadMainLevel(int mainLevelIndex)
    {
        if (mainLevelIndex >= 0 && mainLevelIndex < levelGroups.Count)
        {
            currentMainLevelIndex = mainLevelIndex;
            currentChildLevelIndex = 0;
            SceneManager.LoadScene(levelGroups[mainLevelIndex].mainLevel);
        }
        else
        {
            Debug.LogError("Неверный индекс основного уровня");
        }
    }

    public void LoadChildLevel(int childLevelIndex)
    {
        if (childLevelIndex >= 0 && childLevelIndex < levelGroups[currentMainLevelIndex].childLevels.Count)
        {
            currentChildLevelIndex = childLevelIndex;
            SceneManager.LoadScene(levelGroups[currentMainLevelIndex].childLevels[childLevelIndex]);
        }
        else
        {
            Debug.LogError("Неверный индекс дочернего уровня");
        }
    }

    public void LoadNextLevel()
    {
        int nextChildLevelIndex = currentChildLevelIndex + 1;
        if (nextChildLevelIndex < levelGroups[currentMainLevelIndex].childLevels.Count)
        {
            LoadChildLevel(nextChildLevelIndex);
        }
        else
        {
            int nextMainLevelIndex = currentMainLevelIndex + 1;
            if (nextMainLevelIndex < levelGroups.Count)
            {
                LoadMainLevel(nextMainLevelIndex);
            }
            else
            {
                Debug.Log("Все уровни пройдены!");
                LoadStartScene();
            }
        }
    }

    public void LoadStartScene()
    {
        if (!string.IsNullOrEmpty(startScene))
        {
            SceneManager.LoadScene(startScene);
        }
        else
        {
            Debug.LogError("Стартовая сцена не задана");
        }
    }

    public void LoadSplashScreen()
    {
        if (!string.IsNullOrEmpty(splashScreenScene))
        {
            SceneManager.LoadScene(splashScreenScene);
        }
        else
        {
            Debug.LogError("Сцена заставки не задана");
        }
    }

    public void LoadGameOverScene()
    {
        if (!string.IsNullOrEmpty(gameOverScene))
        {
            SceneManager.LoadScene(gameOverScene);
        }
        else
        {
            Debug.LogError("Сцена конца игры не задана");
        }
    }

    public void RestartMainLevel()
    {
        LoadMainLevel(currentMainLevelIndex);
    }
}