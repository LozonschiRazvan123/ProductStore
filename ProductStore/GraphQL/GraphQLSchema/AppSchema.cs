using GraphQL.Types;
using ProductStore.GraphQL.GraphQLQueries;
using ProductStore.Interface;

namespace ProductStore.GraphQL.GraphQLSchema
{
    public class AppSchema: Schema
    {
        public AppSchema(IServiceProvider provider): base(provider)
        {
            Query = provider.GetRequiredService<AppQuerry>();
            Mutation = provider.GetRequiredService<AppMutation>();
        }
    }
}
