using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Подключаем TextMeshPro

public class MainBehavior : MonoBehaviour
{
    // Паблики 
    [Serializable]
    public struct TimerSettings
    {
        public TextMeshProUGUI gameTimerText; // Используем TextMeshProUGUI
        public float gameTimeF;
        public AudioSource timerAudioSource; // Аудио источник для таймера
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

    [Serializable]
    public struct DialogueSetting
    {
        public string dialogueText;
        public GameObject dialoguePrefab;
    }

    [SerializeField, Tooltip("Сосал?")] private TimerSettings Timer;
    [SerializeField] private ObjectPrefabsSetting FindObject;
    [SerializeField] private TextMeshProUGUI dialogueTextMeshPro; // Используем TextMeshProUGUI
    [SerializeField] private Transform dialogueSocket; // Сокет для инстанцирования префабов диалогов
    [SerializeField] private DialogueSetting[] dialogues; // Массив структур для диалогов
    [SerializeField] private float typingDelay = 0.1f; // Задержка между печатью букв
    [SerializeField] private AudioSource typingAudioSource; // Общий аудио источник для текста
    [SerializeField] private AudioSource objectPickupAudioSource; // Аудио источник для поднятия объекта
    [SerializeField] private AudioSource nonObjectClickAudioSource; // Аудио источник для нажатия не на игровой объект
    [SerializeField] private Button hintButton1; // Первая кнопка для подсказки
    [SerializeField] private Button hintButton2; // Вторая кнопка для подсказки
    [SerializeField] private Button hintButton3; // Третья кнопка для подсказки
    [SerializeField] private GameObject hintObject; // Объект подсказки

    private float currentTime;
    private int currentDialogueIndex = 0;
    private bool isDialogueActive = true;
    private Coroutine typingCoroutine;
    private GameObject currentDialogueInstance;

    void Start()
    {
        Debug.Log("Стартуем");
        InitializeGame();
        SetUIFindObjectPrefabsActive(false); // Скрыть UIFindObjectPrefabs при запуске игры
        if (dialogues.Length > 0)
        {
            ShowNextDialogue();
        }
        else
        {
            isDialogueActive = false;
            SetUIFindObjectPrefabsActive(true); // Показать UIFindObjectPrefabs, если диалогов нет
        }

        // Назначить обработчики событий для кнопок
        if (hintButton1 != null)
        {
            hintButton1.onClick.AddListener(ShowHint);
            Debug.Log("Обработчик для hintButton1 назначен");
        }
        if (hintButton2 != null)
        {
            hintButton2.onClick.AddListener(ShowHint);
            Debug.Log("Обработчик для hintButton2 назначен");
        }
        if (hintButton3 != null)
        {
            hintButton3.onClick.AddListener(ShowHint);
            Debug.Log("Обработчик для hintButton3 назначен");
        }
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
            // Проигрывать звук таймера
            if (Timer.timerAudioSource != null && !Timer.timerAudioSource.isPlaying)
            {
                Timer.timerAudioSource.Play();
            }
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
            Debug.Log($"Показ диалога: {dialogues[currentDialogueIndex].dialogueText}"); // Лог для отладки
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // Останавливаем текущую корутину печати текста
            }
            // Удалить предыдущий диалоговый инстанс
            if (currentDialogueInstance != null)
            {
                Destroy(currentDialogueInstance);
            }
            // Инстанцировать текущий диалоговый префаб
            if (dialogues[currentDialogueIndex].dialoguePrefab != null)
            {
                currentDialogueInstance = Instantiate(dialogues[currentDialogueIndex].dialoguePrefab, dialogueSocket);
            }
            // Включить аудио источник
            if (typingAudioSource != null)
            {
                typingAudioSource.Play();
            }
            typingCoroutine = StartCoroutine(TypeText(dialogues[currentDialogueIndex].dialogueText));
            currentDialogueIndex++;
        }
        else
        {
            isDialogueActive = false;
            dialogueTextMeshPro.gameObject.SetActive(false);
            if (currentDialogueInstance != null)
            {
                Destroy(currentDialogueInstance); // Удалить последний диалоговый инстанс
            }
            SetUIFindObjectPrefabsActive(true); // Показать UIFindObjectPrefabs
        }
    }

    private IEnumerator TypeText(string text)
    {
        dialogueTextMeshPro.text = "";
        foreach (char c in text)
        {
            dialogueTextMeshPro.text += c;
            yield return new WaitForSeconds(typingDelay);
        }
        // Остановить аудио источник после завершения печати текста
        if (typingAudioSource != null)
        {
            typingAudioSource.Stop();
        }
    }

    private void SetUIFindObjectPrefabsActive(bool isActive)
    {
        for (int i = 0; i < FindObject.findObjectPrefabs.Length; i++)
        {
            if (FindObject.findObjectPrefabs[i].UIFindObjectPrefabs != null)
            {
                FindObject.findObjectPrefabs[i].UIFindObjectPrefabs.SetActive(isActive);
            }
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
            else
            {
                // Проигрывать звук нажатия не на игровой объект
                if (nonObjectClickAudioSource != null)
                {
                    nonObjectClickAudioSource.Play();
                }
            }
        }
    }

    private void HandleObjectClicked(GameObject clickedObject)
    {
        Debug.Log($"Объект {clickedObject.name} кликнут и будет удален.");
        Destroy(clickedObject);

        // Проигрывать звук поднятия объекта
        if (objectPickupAudioSource != null)
        {
            objectPickupAudioSource.Play();
        }

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

    private void ShowHint()
    {
        Debug.Log("ShowHint вызван"); // Лог для отладки
        if (hintObject != null)
        {
            Debug.Log("Показ подсказки"); // Лог для отладки
            hintObject.SetActive(true);
            Animator animator = hintObject.GetComponent<Animator>();
            if (animator != null)
            {
                Debug.Log("Анимация найдена, запуск корутины"); // Лог для отладки
                StartCoroutine(DisableHintAfterAnimation(animator, hintObject));
            }
            else
            {
                Debug.LogError("Анимация не найдена на объекте подсказки");
            }
        }
        else
        {
            Debug.LogError("Объект подсказки не назначен");
        }
    }

    private IEnumerator DisableHintAfterAnimation(Animator animator, GameObject hintObject)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        hintObject.SetActive(false);
        Debug.Log("Подсказка скрыта"); // Лог для отладки
    }

    private void GameOver()
    {
        Debug.Log("Игра окончена");
        GameManager.Instance.LoadGameOverVideoScene(); // Загрузка сцены проигрыша с видео
    }

    private void LevelComplete()
    {
        Debug.Log("Уровень пройден");
        GameManager.Instance.LoadNextLevel();
    }
}