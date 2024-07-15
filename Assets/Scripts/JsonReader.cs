using System.Collections.Generic;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    private void Start()
    {
        var file = Resources.Load<TextAsset>("haikus").ToString();
        Haikus h = LoadFromJSON(file);
        Debug.Log(h.haikus[0].lines[0]);
    }
    private Haikus LoadFromJSON(string ta)
    {
        return JsonUtility.FromJson<Haikus>(ta);
    }
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