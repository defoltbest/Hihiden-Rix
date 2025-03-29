using UnityEngine;
using System.Collections;
using TMPro; // Для TextMeshPro
// Вызывайте из другого скрипта или через Event Trigger GetComponent<TextAnimator>().SkipAnimation();

public class TextAnimator : MonoBehaviour
{
    [SerializeField] private float delayBetweenChars = 0.1f;
    [SerializeField] private bool useRichText = true;
    [SerializeField] private AudioClip typeSound;
    private TMP_Text textComponent; // Для TextMeshPro
    // private Text textComponent; // Для стандартного UI Text
     private AudioSource audioSource; // Добавляем AudioSource
    
    private string fullText;
    private int currentCharIndex;

    void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        fullText = textComponent.text;
        textComponent.text = "";
    }

    void Start()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        while(currentCharIndex < fullText.Length)
        {
            // Воспроизводим звук через AudioSource
            //if(typeSound != null)
           // {
           //     audioSource.Play();
           // }
            // Пропуск HTML-тегов при использовании Rich Text
            if(useRichText && fullText[currentCharIndex] == '<')
            {
                int tagEnd = fullText.IndexOf('>', currentCharIndex);
                currentCharIndex = tagEnd + 1;
                continue;
            }

            textComponent.text = fullText.Substring(0, currentCharIndex + 1);
            currentCharIndex++;
            yield return new WaitForSeconds(delayBetweenChars);
        }
    }

    // Метод для принудительного завершения анимации
    public void SkipAnimation()
    {
        StopAllCoroutines();
        textComponent.text = fullText;
    }
}