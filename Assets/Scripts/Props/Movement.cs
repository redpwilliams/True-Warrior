using Characters;
using UnityEngine;

namespace Props
{
    [RequireComponent(typeof(Character))]
    public class Movement : MonoBehaviour
    {
        private Character _character;

        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void Start()
        {
            Debug.Log("Got character: " + _character);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
