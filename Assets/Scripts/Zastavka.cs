using UnityEngine;
using UnityEngine.Video;

public class Zastavka : MonoBehaviour
{
    public float delayBeforeStartVideo = 1f;
    public VideoPlayer videoPlayer;

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
            Invoke("StartVideo", delayBeforeStartVideo);
        }
        else
        {
            Debug.LogError("¬идео плеер не назначен в инспекторе");
        }
    }

    private void StartVideo()
    {
        videoPlayer.Play();
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        LoadFirstLevel();
    }

    private void LoadFirstLevel()
    {
        GameManager.Instance.LoadLevel(0); // «агружаем первый уровень из массива
    }
}