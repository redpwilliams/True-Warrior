using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class JsonReader
    {

        public static List<Haiku> LoadHaikus()
        {
            string jsonString = Resources.Load<TextAsset>("haikus").ToString();
            return JsonUtility.FromJson<JsonData>(jsonString).Haikus;
        }

        private struct JsonData
        {
            public List<Haiku> Haikus;
        }
    }
}