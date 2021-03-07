using System;
using System.Collections.Generic;

namespace Instabrand.Shared.Infrastructure.CQRS
{
    public interface IQueryHandlerRegistry
    {
        IEnumerable<Type> Queries { get; }

        Type GetQueryHandler(Type type);
    }
}
