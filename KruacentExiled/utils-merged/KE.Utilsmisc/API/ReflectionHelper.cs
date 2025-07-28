using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.API
{
    public static class ReflectionHelper
    {
        public static IEnumerable<T> GetObjects<T>(Assembly assembly = null)
        {
            List<T> result = new();
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }
            foreach (Type t in assembly.GetTypes())
            {
                try
                {
                    if (typeof(T).IsAssignableFrom(t) && !t.IsAbstract)
                    {
                        T ffcc = (T)Activator.CreateInstance(t);
                        result.Add(ffcc);
                    }
                }
                catch (Exception)
                {

                }
            }
            return result;
        }

        public static IEnumerable<T> GetObjects<T>(IEnumerable<Assembly> assemblies)
        {
            List<T> result = new();
            foreach(Assembly assembly in assemblies)
            {
                result.AddRange(GetObjects<T>(assembly));
            }
            return result;
        }
    }
}
