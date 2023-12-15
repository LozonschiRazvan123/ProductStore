using GraphQL.Types;
using ProductStore.DTO;

namespace ProductStore.GraphQL.GraphQLTypes
{
    public class AddressType: ObjectGraphType<AddressDTO>
    {
        public AddressType() 
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Id property from the address object.");
            Field(x => x.Street).Description("Street property from the address object");
            Field(x => x.City).Description("City property from the address object");
            Field(x => x.State).Description("State property from the address object");

            //Field<BooleanGraphType>("success", resolve: context => true, description: "Indicates whether the operation was successful.");
            //Field<StringGraphType>("message", resolve: context => "Operation successful", description: "A message providing details about the operation.");
        }
    }
}
