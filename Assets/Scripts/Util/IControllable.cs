namespace Util
{
    public interface IControllable
    {
        /// Enables the Scroll control for this entity.
        public void EnableScrollInput();
        
        /// Disables the Scroll control for this entity.
        public void DisableScrollInput();

        /// Enables the Select control for this entity.
        public void EnableIntentInput();
        
        /// Disables the Select control for this entity.
        public void DisableIntentInput();
    }
}