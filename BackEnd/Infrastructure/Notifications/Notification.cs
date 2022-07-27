using System;

namespace Infrastructure.Notifications
{
    public class Notification
    {
        public Guid Id { get; set; }

        public string Event { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}