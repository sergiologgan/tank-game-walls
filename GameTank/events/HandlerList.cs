using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameTank.events
{
    public class HandlerList
    {
        private MethodInfo[] methods;

        public HandlerList()
        {
            List<Type> classes = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(x => x.GetTypes())
        .Where(x => x.IsClass).ToList();

            var methods = AppDomain.CurrentDomain.GetAssemblies()
       .SelectMany(x => x.GetTypes())
       .Where(x => x.IsClass && x.IsSubclassOf(typeof(TankEvent)))
       .SelectMany(x => x.GetMethods())
       .Where(x => x.GetCustomAttributes(typeof(EventHanddler), false).FirstOrDefault() != null).ToArray();

            this.methods = methods;
        }

        public void InvokeMethod(BindingFlags bindingFlags, Type[] parameterTypes, object[] args)
        {
            var method = this.methods
                .Where(x => x.GetParameters().Any(y => parameterTypes.Any(j => j.Equals(y.ParameterType))))
                .ToArray();

            if (method != null)
            {
                for (int i = 0; i < method.Length; i++)
                {
                    var obj = Activator.CreateInstance(method[i].DeclaringType);
                    method[i].Invoke(obj, args);
                }
            }
        }
    }
}