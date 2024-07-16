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
    }

    private void Start()
    {
        var haikus = new JsonReader().Haikus;
        StartCoroutine(HaikuCountdown(haikus, 3));
    }

    private IEnumerator HaikuCountdown(List<JsonReader.Haiku> haikus, int stage)
    {
        JsonReader.Haiku haiku = haikus[Random.Range(0, haikus.Count)];
        for (int i = 0; i < haikus.Count; i++)
        {
            foreach (var line in haikus[i].lines)
            { 
                Debug.Log("Stage: " + stage);
                _tmp.text = line; 
                yield return new WaitForSeconds(5);
            }
        }

    }
    
    
    private class JsonReader
    {
        public List<Haiku> Haikus { get; } 
        
        public JsonReader()
        {
            string jsonString = Resources.Load<TextAsset>("haikus").ToString();
            Haikus = JsonUtility.FromJson<JsonData>(jsonString).haikus;
            Debug.Log(Haikus[0].lines[0]);
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
