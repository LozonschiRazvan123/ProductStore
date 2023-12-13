using GraphQL.Types;
using ProductStore.DTO;
using ProductStore.GraphQL.GraphQLTypes;
using ProductStore.Interface;

namespace ProductStore.GraphQL.GraphQLQueries
{
    public class AppQuerry: ObjectGraphType
    {
        public AppQuerry(IAddressRepository repository)
        {
            Field<ListGraphType<AddressType>>(
            "addresses",
            resolve: context =>
            {
                var serviceProvider = context.RequestServices;
                var addressRepository = serviceProvider.GetRequiredService<IAddressRepository>();
                return addressRepository.GetAddresses();
            }
        );
        }
    }
}
