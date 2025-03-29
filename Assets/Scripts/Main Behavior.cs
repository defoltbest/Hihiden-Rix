using System;
using UnityEngine;
using TMPro; // Подключаем TextMeshPro

public class MainBehavior : MonoBehaviour
{
    // Паблики 
    [Serializable]
    public struct TimerSettings
    {
        public TextMeshProUGUI gameTimerText; // Используем TextMeshProUGUI
        public float gameTimeF;
    }

    [Serializable]
    public struct FindObjectPrefabsSetting
    {
        public GameObject FindObjectPrefabs;
        public GameObject UIFindObjectPrefabs;
    }

    [Serializable]
    public struct ObjectPrefabsSetting
    {
        public FindObjectPrefabsSetting[] findObjectPrefabs;
    }

    [SerializeField, Tooltip("Сосал?")] private TimerSettings Timer;
    [SerializeField] private ObjectPrefabsSetting FindObject;
    [SerializeField] private TextMeshProUGUI dialogueText; // Используем TextMeshProUGUI
    [SerializeField] private string[] dialogues;

    private float currentTime;
    private int currentDialogueIndex = 0;
    private bool isDialogueActive = true;

    void Start()
    {
        Debug.Log("Стартуем");
        InitializeGame();
        ShowNextDialogue();
    }

    void Update() //Инициализация игры
    {
        if (isDialogueActive)
        {
            CheckForDialogueClick();
        }
        else
        {
            CheckForObjectClick();
            UpdateTimer();
        }
    }

    private void InitializeGame() //Инициализация игры
    {
        Debug.Log("Инициализация игры");
        currentTime = Timer.gameTimeF;
        UpdateTimerText();
    }

    private void UpdateTimer()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            GameOver();
        }
    }

    private void UpdateTimerText()
    {
        Timer.gameTimerText.text = Mathf.Ceil(currentTime).ToString();
    }

    private void CheckForDialogueClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowNextDialogue();
        }
    }

    private void ShowNextDialogue()
    {
        if (currentDialogueIndex < dialogues.Length)
        {
            Debug.Log($"Показ диалога: {dialogues[currentDialogueIndex]}"); // Лог для отладки
            dialogueText.text = dialogues[currentDialogueIndex];
            currentDialogueIndex++;
        }
        else
        {
            isDialogueActive = false;
            dialogueText.gameObject.SetActive(false);
        }
    }

    private void CheckForObjectClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                HandleObjectClicked(hit.collider.gameObject);
            }
        }
    }

    private void HandleObjectClicked(GameObject clickedObject)
    {
        Debug.Log($"Объект {clickedObject.name} кликнут и будет удален.");
        Destroy(clickedObject);

        // Найти соответствующий UIFindObjectPrefabs и скрыть его
        for (int i = 0; i < FindObject.findObjectPrefabs.Length; i++)
        {
            if (FindObject.findObjectPrefabs[i].FindObjectPrefabs == clickedObject)
            {
                if (FindObject.findObjectPrefabs[i].UIFindObjectPrefabs != null)
                {
                    FindObject.findObjectPrefabs[i].UIFindObjectPrefabs.SetActive(false);
                }
                FindObject.findObjectPrefabs[i].FindObjectPrefabs = null; // Устанавливаем в null, чтобы отметить как уничтоженный
                break;
            }
        }

        // Сбросить таймер
        currentTime = Timer.gameTimeF;

        // Проверить, остались ли объекты
        bool allObjectsDestroyed = true;
        for (int i = 0; i < FindObject.findObjectPrefabs.Length; i++)
        {
            if (FindObject.findObjectPrefabs[i].FindObjectPrefabs != null)
            {
                allObjectsDestroyed = false;
                break;
            }
        }

        if (allObjectsDestroyed)
        {
            LevelComplete();
        }
    }

    private void GameOver()
    {
        Debug.Log("Игра окончена");
        GameManager.Instance.LoadGameOverScene();
    }

    private void LevelComplete()
    {
        Debug.Log("Уровень пройден");
        GameManager.Instance.LoadNextLevel();
    }
}