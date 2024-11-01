using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Abstractions
{
    // we need mediatr package, because we want to inherit from INotification
    // to perform domain events
    public interface IDomainEvent : INotification
    {
        Guid EventId => Guid.NewGuid();
        public DateTime OccurredOn => DateTime.Now;
        // which class threw this event
        public string EventType => GetType().AssemblyQualifiedName;
    }
}