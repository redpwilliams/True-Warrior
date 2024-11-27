namespace UI.Buttons.Menu
{
    public interface IButtonGroupManager<T> where T : BaseUIButton
    {
        public void ShowMenuButtonGroup(ButtonGroup<T> bg);
        public void OperateMenuButtonGroup(ButtonGroup<T> bg);
    }
}