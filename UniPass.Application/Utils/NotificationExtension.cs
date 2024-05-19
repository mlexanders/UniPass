using Radzen;

namespace UniPass.Application.Utils;

public static class NotificationExtension
{
    
    public static void ShowExceptionNotification(this NotificationService service, Exception e)
    {
        service.Notify(GetNotification("Ошибка", e.Message, NotificationSeverity.Error));
    }

    public static void ShowNotification(this NotificationService service, string title, string description,
        NotificationSeverity type)
    {
        service.Notify(GetNotification(title, description, type));
    }

    public static UniPassClientException GetNotificationException(string title, string description,
        NotificationSeverity type)
    {
        return new UniPassClientException(GetNotification(title, description, type));
    }

    public static UniPassClientException GetNotificationException(string title, Exception exception,
        NotificationSeverity type)
    {
        return new UniPassClientException(GetNotification(title, exception.Message, type));
    }

    private static NotificationMessage GetNotification(string title, string description, NotificationSeverity type)
    {
        return new NotificationMessage
        {
            Severity = type,
            Duration = type is NotificationSeverity.Error ? 15000 : 8000,
            Summary = title,
            Detail = description
        };
    }
}