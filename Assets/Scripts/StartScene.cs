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
            Debug.LogError("������ ������ �� ��������� � ����������");
        }
    }

    private void OnStartButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadSplashScreen(); // ��������� ����� ��������
        }
        else
        {
            Debug.LogError("GameManager.Instance �� ������");
        }
    }
}