using System.Collections;
using UnityEngine;
using TMPro;

namespace Characters
{
    public class CharacterText : MonoBehaviour
    {
        private TextMeshPro _tmp;
        private void Awake()
        {
            _tmp = GetComponent<TextMeshPro>();
            _tmp.enabled = false;
        }

        public void SetText(string text)
        {
            _tmp.enabled = true;
            _tmp.text = text;
        }
    }
}