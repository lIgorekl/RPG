namespace Presentation.UI
{
    public class PauseMenuController
    {
        public void Resume(System.Action onResume)
        {
            onResume?.Invoke();
        }

        public void GoToMenu(System.Action loadMenu)
        {
            loadMenu?.Invoke();
        }
    }
}