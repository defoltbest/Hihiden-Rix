using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Tooltip("������ ���� �������")]
    public string[] levelScenes;

    [Tooltip("����� ��������")]
    public string splashScreenScene;

    [Tooltip("��������� �����")]
    public string startScene;

    private int currentLevelIndex = 0;

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

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelScenes.Length)
        {
            currentLevelIndex = levelIndex;
            SceneManager.LoadScene(levelScenes[levelIndex]);
        }
        else
        {
            Debug.LogError("�������� ������ ������");
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
            Debug.LogError("����� �������� �� ������");
        }
    }

    public void LoadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;
        if (nextLevelIndex < levelScenes.Length)
        {
            LoadLevel(nextLevelIndex);
        }
        else
        {
            Debug.Log("��� ������ ��������!");
            LoadStartScene();
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
            Debug.LogError("��������� ����� �� ������");
        }
    }
}