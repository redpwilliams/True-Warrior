using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// ReSharper disable InconsistentNaming

[RequireComponent(typeof(TextMeshProUGUI))]
public class HaikuText : MonoBehaviour
{
    private TextMeshProUGUI _tmp;

    [SerializeField] private float timeUntilStage1= 2.5f;
    [SerializeField] private float timeUntilStage2 = 5f;
    [SerializeField] private float timeUntilStage3 = 5f;
    // TODO - Make range?
    [SerializeField] private float timeUntilBattleStart = 5f;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        _tmp.text = "";
        _tmp.alpha = 0;
    }

    private void Start()
    {
        var haikus = new JsonReader().Haikus;
        StartCoroutine(HaikuCountdown(haikus));
    }

    private IEnumerator HaikuCountdown(List<JsonReader.Haiku> haikus)
    {
        // Initial startup buffer
        yield return new WaitForSeconds(2.5f);
        
        // Choose haiku
        JsonReader.Haiku haiku = haikus[Random.Range(0, haikus.Count)];
        
        int stage = 0;

        // Line 1
        yield return new WaitForSeconds(timeUntilStage1);
        _tmp.text = haiku.lines[stage++];
        yield return StartCoroutine(FadeText(1f, true));
        
        // Line 2
        yield return new WaitForSeconds(timeUntilStage2);
        _tmp.text = haiku.lines[stage++];
        
        // Line 3
        yield return new WaitForSeconds(timeUntilStage3);
        _tmp.text = haiku.lines[stage];
        
        // Battle Start
        yield return new WaitForSeconds(timeUntilBattleStart);
        // TODO - Change text to different formatted text object?
    }
    
    private IEnumerator FadeText(float fadeDuration, bool fadeIn)
    {
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            _tmp.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / 
            fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _tmp.alpha = endAlpha;
    }

    private class JsonReader
    {
        public List<Haiku> Haikus { get; }

        public JsonReader()
        {
            string jsonString = Resources.Load<TextAsset>("haikus").ToString();
            Haikus = JsonUtility.FromJson<JsonData>(jsonString).haikus;
        }

        [System.Serializable]
        internal struct JsonData
        {
            public List<Haiku> haikus;
        }

        [System.Serializable]
        internal struct Haiku
        {
            public string author;
            public List<string> lines;
        }
    }
}