namespace Orión.Application.Interfaces;

public interface IMessageService
{
    void ShowInfo(string message, string title = "Información");
    void ShowWarning(string message, string title = "Advertencia");
    void ShowError(string message, string title = "Error");
    bool ShowConfirmation(string message, string title = "Confirmación");
}
