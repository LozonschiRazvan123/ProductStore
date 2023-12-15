using GraphQL.Types;
using ProductStore.DTO;
using ProductStore.GraphQL.GraphQLTypes;
using ProductStore.Interface;
using Quartz.Impl.AdoJobStore.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProductStore.GraphQL.GraphQLQueries
{
    public class AppQuerry : ObjectGraphType
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AppQuerry(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

            Field<ListGraphType<AddressType>>(
                "address",
                resolve: context =>
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var addressRepository = scope.ServiceProvider.GetRequiredService<IAddressRepository>();
                        return addressRepository.GetAddresses();
                    }
                });
            /*Field<AppMutation>("mutation", resolve: context =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var addressRepository = scope.ServiceProvider.GetRequiredService<IAddressRepository>();
                    return scope.ServiceProvider.GetRequiredService<AppMutation>();
                }
            });*/
        }
    }
}
