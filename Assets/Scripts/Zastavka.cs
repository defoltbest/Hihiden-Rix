using UnityEngine;
using UnityEngine.Video;

public class Zastavka : MonoBehaviour
{
    public float delayBeforeStartVideo = 1f;
    public VideoPlayer videoPlayer;
    public string videoUrl;

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false; // ��������� Play on Awake
            videoPlayer.url = videoUrl; // ������������� URL �����
            videoPlayer.loopPointReached += OnVideoEnd;
            Invoke("StartVideo", delayBeforeStartVideo);
        }
        else
        {
            Debug.LogError("����� ����� �� �������� � ����������");
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
        GameManager.Instance.LoadLevel(0); // ��������� ������ ������� �� �������
    }
}

