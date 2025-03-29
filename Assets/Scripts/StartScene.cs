using UnityEngine;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    [SerializeField] private Button startButton;

    private void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        else
        {
            Debug.LogError("Кнопка старта не назначена в инспекторе");
        }
    }

    private void OnStartButtonClicked()
    {
        GameManager.Instance.LoadSplashScreen(); // Загружаем сцену заставки
    }
}