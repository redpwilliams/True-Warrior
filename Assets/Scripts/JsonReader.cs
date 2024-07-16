using System.Collections.Generic;
using UnityEngine;
// ReSharper disable InconsistentNaming

public class JsonReader : MonoBehaviour
{
    private void Start()
    {
        var jsonString = Resources.Load<TextAsset>("haikus").ToString();
        JsonData h = JsonUtility.FromJson<JsonData>(jsonString);
        Debug.Log(h.haikus[0].lines[0]);
    }

    [System.Serializable]
    public struct JsonData
    {
        public List<Interior> haikus;
    }

    [System.Serializable]
    public struct Interior
    {
        public string author;
        public List<string> lines;
    }
}