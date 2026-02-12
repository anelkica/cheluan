using CommunityToolkit.Mvvm.ComponentModel;
using cheluan.Models;
using System.Threading.Tasks;
namespace cheluan.Services;

public partial class NotificationService : ObservableObject, INotificationService
{
    [ObservableProperty] private string? _notificationMessage;
    [ObservableProperty] private bool _isVisible = false;
    [ObservableProperty] private bool _isError = false;

    public async Task NotifyAsync(Result result, string successMessage = "Success.")
    {
        NotificationMessage = result.Success ? successMessage : result.Error;
        IsError = result.Failed;
        IsVisible = true;

        await Task.Delay(3000);
        IsVisible = false;
    }


}
