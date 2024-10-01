using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public static class JsonReader
    {

        public static List<Haiku> LoadHaikus()
        {
            string jsonString = Resources.Load<TextAsset>("haikus").ToString();
            return JsonUtility.FromJson<JsonData>(jsonString).haikus;
        }

        [Serializable]
        public struct JsonData
        {
            public List<Haiku> haikus;
        }
    }
}