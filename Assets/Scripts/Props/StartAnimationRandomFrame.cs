using UnityEngine;

namespace Props
{
   [RequireComponent(typeof(Animator))]
   public class StartAnimationRandomFrame : MonoBehaviour
   {
      private void Start()
      {
         var anim = GetComponent<Animator>();
         var state = anim.GetCurrentAnimatorStateInfo(0);
         anim.Play(state.fullPathHash, 0, Random.Range(0f, 1f));
      }
   }
}
