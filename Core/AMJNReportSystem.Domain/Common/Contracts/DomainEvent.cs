﻿using AMJNReportSystem.Domain.Event;

namespace AMJNReportSystem.Domain.Common.Contracts
{
    public abstract class DomainEvent : IEvent
    {
        public DateTime TriggeredOn { get; protected set; } = DateTime.UtcNow;
    }
}
