using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// ReSharper disable InconsistentNaming

[RequireComponent(typeof(TextMeshProUGUI))]
public class HaikuText : MonoBehaviour
{
    private TextMeshProUGUI _tmp;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        _tmp.text = null;
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
        
        // Display lines
        JsonReader.Haiku haiku = haikus[Random.Range(0, haikus.Count)];
        for (int i = 0; i < 3; i++)
        {
            _tmp.text = haiku.lines[i];
            yield return new WaitForSeconds(5);
        }
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