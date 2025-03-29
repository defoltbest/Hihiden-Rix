using System;
using UnityEngine;
using UnityEngine.UI;

public class MainBehavior : MonoBehaviour
{   
    // Паблики 
    [Serializable] 
    public struct TimerSettings
    {
        public Text gameTimerText;
        public float gameTimeF;
    }

    [Serializable]
    public struct GameObjectsSetting
    {
        public GameObject[] FindObjectPrefabs;
    }

    [SerializeField, Tooltip("Сосал?")] private TimerSettings Timer;
    [SerializeField] private GameObjectsSetting FindObject;

    private float currentTime;

    void Start()
    {
        Debug.Log("Стартуем");
        InitializeGame();
    }

    void Update() //Инициализация игры
    {
        CheckForObjectClick();
        UpdateTimer();
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
            currentTime = Timer.gameTimeF;
        }
    }

    private void UpdateTimerText()
    {
        Timer.gameTimerText.text = Mathf.Ceil(currentTime).ToString();
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
    }
}
