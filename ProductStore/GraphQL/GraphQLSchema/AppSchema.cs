using GraphQL.Types;
using ProductStore.GraphQL.GraphQLQueries;

namespace ProductStore.GraphQL.GraphQLSchema
{
    public class AppSchema: Schema
    {
        public AppSchema(IServiceProvider provider): base(provider)
        {
            Query = provider.GetRequiredService<AppQuerry>();
        }
    }
}
