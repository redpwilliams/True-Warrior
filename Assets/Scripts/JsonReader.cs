using System.Collections.Generic;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    private void Start()
    {
        var jsonString = Resources.Load<TextAsset>("haikus").ToString();
        Haikus h = JsonUtility.FromJson<Haikus>(jsonString);
        Debug.Log(h.haikus[0].lines[0]);
    }

    [System.Serializable]
    public struct Haikus
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