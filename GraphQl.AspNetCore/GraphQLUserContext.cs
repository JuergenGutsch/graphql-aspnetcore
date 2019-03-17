using System.Security.Claims;
using GraphQL.Authorization;

namespace GraphQl.AspNetCore
{
    public class GraphQLUserContext : IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User { get; set; }
    }
}
