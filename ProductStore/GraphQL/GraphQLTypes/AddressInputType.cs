using GraphQL.Types;

namespace ProductStore.GraphQL.GraphQLTypes
{
    public class AddressInputType: InputObjectGraphType
    {
        public AddressInputType() 
        {
            Name = "addressInput";
            Field<NonNullGraphType<IntGraphType>>("id");
            Field<NonNullGraphType<StringGraphType>>("street");
            Field<NonNullGraphType<StringGraphType>>("city");
            Field<NonNullGraphType<StringGraphType>>("state");
        }

    }
}
