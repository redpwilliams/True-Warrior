using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters
{
    public static class JsonReader
    {

        public static List<Haiku> LoadHaikus()
        {
            string jsonString = Resources.Load<TextAsset>("haikus").ToString();
            return JsonUtility.FromJson<JsonData>(jsonString).haikus;
        }

        [Serializable]
        internal struct JsonData
        {
            public List<Haiku> haikus;
        }
    }
}