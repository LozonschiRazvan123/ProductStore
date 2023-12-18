using GraphQL;
using GraphQL.Types;
using ProductStore.DTO;
using ProductStore.GraphQL.GraphQLTypes;
using ProductStore.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ProductStore.GraphQL.GraphQLQueries
{
    public class AppMutation : ObjectGraphType
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AppMutation(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            Field<BooleanGraphType>(
                "createAddress",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<AddressInputType>> { Name = "address" }),
                resolve: context => CreateAddress(context)
            );

            Field<BooleanGraphType>(
                "updateAddress",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AddressInputType>> { Name = "address" }
                ),
                resolve: context => UpdateAddress(context)
            );

            Field<StringGraphType>(
                "deleteAddress",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "addressId" }),
                resolve: context => DeleteAddress(context)
            );
        }

        private bool CreateAddress(IResolveFieldContext<object> context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var addressRepository = scope.ServiceProvider.GetRequiredService<IAddressRepository>();
                var address = context.GetArgument<AddressDTO>("address");
                var result = addressRepository.CreateAddress(address);
                return result;
            }
        }

        private bool UpdateAddress(IResolveFieldContext<object> context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IAddressRepository>();
                var address = context.GetArgument<AddressDTO>("address");

                var result = repository.UpdateAddress(address);

                return result;
            }
        }

        private string DeleteAddress(IResolveFieldContext<object> context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IAddressRepository>();
                var addressId = context.GetArgument<int>("addressId");
                var address = repository.GetAddress(addressId);

                if (address == null)
                {
                    context.Errors.Add(new ExecutionError($"Couldn't find address with id {addressId} in the database."));
                    return "Address not found.";
                }

                repository.DeleteAddress(address);
                return $"Address with id {addressId} has been successfully deleted.";
            }
        }
    }
}
