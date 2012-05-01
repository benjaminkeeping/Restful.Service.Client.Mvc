using System;
using System.Linq;
using System.Reflection;

namespace Restful.Service.Client.Mvc
{
    public static class Bootstrap
    {
        public static void RegisterClients(Assembly[] assemblyToFindTypes, Action<Type, Type> registerTypeForContainer)
        {
            var allTypes = assemblyToFindTypes.SelectMany(a => a.GetTypes());
            var allServiceClientInterfaces =
                allTypes.Where(x => typeof(IClient).IsAssignableFrom(x) && x.IsInterface && x != typeof(IClient)).ToList();
            foreach (var @interface in allServiceClientInterfaces)
            {
                registerTypeForContainer(@interface, allTypes.Where(x => @interface.IsAssignableFrom(x) && x.IsClass).FirstOrDefault());
            }
        }
    }
}