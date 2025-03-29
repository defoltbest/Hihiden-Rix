using UnityEngine;

public class ForceAspectRatio : MonoBehaviour
{
    [SerializeField] private float _targetAspect = 16f / 9f; // 1920x1080
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        FixAspect();
    }

    void FixAspect()
    {
        // Рассчитываем текущее соотношение сторон экрана
        float currentAspect = (float)Screen.width / Screen.height;
        float scaleHeight = currentAspect / _targetAspect;

        // Создаем "просмотровое окно" камеры, чтобы избежать растяжения
        if (scaleHeight < 1)
        {
            Rect rect = _camera.rect;
            rect.width = 1;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1 - scaleHeight) / 2;
            _camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1 / scaleHeight;
            Rect rect = _camera.rect;
            rect.width = scaleWidth;
            rect.height = 1;
            rect.x = (1 - scaleWidth) / 2;
            rect.y = 0;
            _camera.rect = rect;
        }
    }
}