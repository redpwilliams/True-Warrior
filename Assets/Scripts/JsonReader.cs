using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    private void Start()
    {
        // var file = Resources.Load<TextAsset>("haikus").ToString().Replace("\n", "").Replace("\t", "").Replace("\\", "");
        var file = Resources.Load<TextAsset>("haikus").ToString();
        Haikus h = Haikus.LoadFromJSON(file);
        
    }
}

[System.Serializable]
public class Haikus
{
    public List<Interior> haikus;

    public static Haikus LoadFromJSON(string ta)
    {
        return JsonUtility.FromJson<Haikus>(ta);
    }
}

[System.Serializable]
public class Interior
{
    public string author;
    public List<string> lines;
}