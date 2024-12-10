namespace Util
{
    public interface IControllable
    {
        /// Enables controls for this entity.
        /// This method applies this/these controls.
        public void EnableInput(PlayerInputFlags input);

        /// Enables controls for this entity.
        /// This method applies this/these controls.
        public void EnableInput(params PlayerInputFlags[] inputs);
        
        /// Disables controls for this entity.
        /// This method applies this/these controls.
        public void DisableInput(PlayerInputFlags input);
        
        /// Disables controls for this entity.
        /// This method applies this/these controls.
        public void DisableInput(params PlayerInputFlags[] inputs);
    }
}