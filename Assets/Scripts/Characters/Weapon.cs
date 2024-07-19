using UnityEngine;

namespace Characters
{
    public class Weapon : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Player")) return;
            Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " +
             col.tag);
        }
    }
}