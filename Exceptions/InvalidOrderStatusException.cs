using OnlineShoppingSystem.Exceptions;
using System;

namespace OnlineShoppingSystem.Exceptions
{
    /// <summary>
    /// Thrown when an invalid status transition is attempted on an order.
    /// </summary>
    public class InvalidOrderStatusException : Exception
    {
        public string CurrentStatus { get; }
        public string RequestedStatus { get; }

        public InvalidOrderStatusException(string currentStatus, string requestedStatus)
            : base($"Cannot change order status from '{currentStatus}' to '{requestedStatus}'.")
        {
            CurrentStatus = currentStatus;
            RequestedStatus = requestedStatus;
        }
    }
}