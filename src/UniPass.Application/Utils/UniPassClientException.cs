using System;
using Radzen;

namespace UniPass.Application.Utils;

public class UniPassClientException : Exception
{
    public UniPassClientException(string resultMessage) : base(resultMessage)
    {
    }
    
    public UniPassClientException(NotificationMessage notificationMessage)
    {
        NotificationMessage = notificationMessage;
    }

    public NotificationMessage NotificationMessage { get; }
}