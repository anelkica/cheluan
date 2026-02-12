using cheluan.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace cheluan.Services;

public interface INotificationService
{
    string? NotificationMessage { get; }
    bool IsVisible { get; }
    bool IsError { get; }

    Task NotifyAsync(Result result, string successMessage = "Success!");
}
