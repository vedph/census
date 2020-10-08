using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Census.Ef
{
    /// <summary>
    /// Validator for entities with <see cref="MaxLengthAttribute"/>.
    /// </summary>
    public static class MaxLengthValidator
    {
        public static Dictionary<string,Tuple<int,string>> GetErrors(
            object entity)
        {
            if (entity == null) return null;

            Dictionary<string, Tuple<int, string>> errors =
                new Dictionary<string, Tuple<int, string>>();

            Type t = entity.GetType();

            foreach (PropertyInfo pi in t.GetProperties(
                BindingFlags.Public | BindingFlags.Instance))
            {
                if (pi.PropertyType != typeof(string)) continue;

                MaxLengthAttribute attr =
                    pi.GetCustomAttribute<MaxLengthAttribute>();
                if (attr != null)
                {
                    string s = pi.GetValue(entity) as string;
                    if (s?.Length > attr.Length)
                    {
                        errors[t.Name + "." + pi.Name] =
                            Tuple.Create(attr.Length, s);
                    }
                }
            }

            return errors;
        }
    }
}
