using LMSWebAppClean.API.Interface;
using System.Reflection;

namespace LMSWebAppClean.API.Extension
{
    public static class EndpointGroupRegistrationExtensions
    {
        public static void RegisterAllEndpointGroups(this IEndpointRouteBuilder app)
        {
            var endpointGroupTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IEndpointGroup).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in endpointGroupTypes)
            {
                try
                {
                    if (Activator.CreateInstance(type) is IEndpointGroup group)
                    {
                        group.MapEndpoints(app);
                    }
                }
                catch (Exception)
                {
                    // Silently continue if endpoint registration fails
                    // In production, you might want to use proper logging instead
                }
            }
        }
    }
}
