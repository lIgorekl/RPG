namespace Presentation.UI
{
    // Контроллер меню паузы
    // Обрабатывает действия пользователя в меню паузы
    public class PauseMenuController
    {
        // Выполняет действие продолжения игры
        public void Resume(System.Action onResume)
        {
            onResume?.Invoke();
        }

        // Выполняет действие перехода в главное меню
        public void GoToMenu(System.Action loadMenu)
        {
            loadMenu?.Invoke();
        }
    }
}