namespace Util
{
    public interface IControllable
    {
        /// Enables the Scroll control for this entity.
        public void EnableScrollInput();
        
        /// Disables the Scroll control for this entity.
        public void DisableScrollInput();

        /// Enables the Select control for this entity.
        public void EnableSelectInput();
        
        /// Disables the Select control for this entity.
        public void DisableSelectInput();

        /// Enables the Attack control for this entity.
        public void EnableAttackInput();
        
        /// Disables the Attack control for this entity.
        public void DisableAttackInput();

    }
}