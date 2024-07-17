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
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 0.5f;

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
        _tmp.text = SetText(haiku.lines[stage]);
        yield return new WaitForSeconds(timeUntilStage1);
        EventManager.Events.StageX(stage++);
        yield return StartCoroutine(FadeText(fadeInDuration, true));
        yield return new WaitForSeconds(3f); // TODO - Time to hold text
        yield return StartCoroutine(FadeText(fadeOutDuration, false));
        
        // Line 2
        _tmp.text = SetText(haiku.lines[stage]);
        yield return new WaitForSeconds(timeUntilStage2);
        EventManager.Events.StageX(stage++);
        yield return StartCoroutine(FadeText(fadeInDuration, true));
        yield return new WaitForSeconds(3f); // TODO - Time to hold text
        yield return StartCoroutine(FadeText(fadeOutDuration, false));
        
        // Line 3
        _tmp.text = SetText(haiku.lines[stage]);
        EventManager.Events.StageX(stage++);
        yield return new WaitForSeconds(timeUntilStage3);
        yield return StartCoroutine(FadeText(fadeInDuration, true));
        yield return new WaitForSeconds(3f); // TODO - Time to hold text
        yield return StartCoroutine(FadeText(fadeOutDuration, false));
        
        // Battle Start
        _tmp.text = "Strike!\n攻撃！";
        yield return new WaitForSeconds(timeUntilBattleStart);
        EventManager.Events.StageX(stage);
        yield return StartCoroutine(FadeText(0.05f, true));
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

    private string SetText(JsonReader.LinePair lp)
    {
        return $"{lp.en}\n{lp.jp}";
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
            public List<LinePair> lines;
        }

        [System.Serializable]
        internal struct LinePair
        {
            public string en;
            public string jp;
        }
    }
}