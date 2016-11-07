namespace Ergate.DataAccess
{
    using System;
    using System.Collections.Generic;

    public interface IInterceptorsResolver
	{
		IEnumerable<IEntityInterceptor> GetGlobalInterceptors();
		IEnumerable<IEntityInterceptor> GetEntityInterceptors(Type entityType);
	}
}