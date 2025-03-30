using UnityEngine;
using UnityEngine.Video;

public class GameOverVideo : MonoBehaviour
{
    public float delayBeforeStartVideo = 1f;
    public VideoPlayer videoPlayer;
    public string videoUrl;

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false; // Отключаем Play on Awake
            videoPlayer.url = videoUrl; // Устанавливаем URL видео
            videoPlayer.loopPointReached += OnVideoEnd;
            Invoke("StartVideo", delayBeforeStartVideo);
        }
        else
        {
            Debug.LogError("Видео плеер не назначен в инспекторе");
        }
    }

    private void StartVideo()
    {
        videoPlayer.Play();
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        RestartLevel();
    }

    private void RestartLevel()
    {
        GameManager.Instance.RestartMainLevel(); // Перезапуск основного уровня
    }
}