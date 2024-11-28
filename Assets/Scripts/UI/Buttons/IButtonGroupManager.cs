namespace UI.Buttons
{
    public interface IButtonGroupManager
    {
        
        public void ShowButtonGroup<T>(ButtonGroup<T> bg)
            where T : BaseUIButton;
    }
}