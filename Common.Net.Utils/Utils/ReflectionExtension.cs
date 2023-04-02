using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Net.Utils
{
    public static class ReflectionExtension
    {
        public static void MapFromReflectedProperties(this object target, object source)
        {
            foreach (var targetProperty in target.GetType().GetProperties().Where(p => p.CanWrite))
            {
                try
                {
                    var sourceProperty = source.GetType().GetProperty(targetProperty.Name);

                    if (sourceProperty != null && sourceProperty.PropertyType == targetProperty.PropertyType)
                        targetProperty.SetValue(target, sourceProperty.GetValue(source));
                }
                catch
                {
                }
            }
        }

        public static IList<FieldInfo> GetAllPublicConstFields(this Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                       .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                       .ToList();
        }

        public static IList<string> GetPublicConstValues(this Type type)
        {
            return type.GetAllPublicConstFields().Select(p => p.GetValue(p).ToString().ToLower()).ToList();
        }

        public static IDictionary<string, string> GetPublicConstKeyValues(this Type type)
        {
            return type.GetAllPublicConstFields().ToDictionary(f => f.Name, f => f.GetValue(f).ToString());
        }

        /// <summary>
        /// Returns the Attributes of a specific property.
        /// Example: 
        /// <code>
        /// var attribute1 = GetPropertyAttribute<myClass, JsonPropertyAttribute>(t => t.SomeProperty);
        /// var attribute2 = GetPropertyAttribute<myClass, MyCustomAttribute>(t => t.SomeProperty)
        /// var result = attribute1.PropertyName;
        /// </code>
        /// </summary>
        /// <typeparam name="TType">The class that contains the member as a type.</typeparam>
        /// <typeparam name="TAttribute">The attribute class,</typeparam>
        /// <param name="property">The property</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Expression must be a property</exception>
        /// <exception cref="Exception">"No such attribute is found.</exception>
        public static TAttribute GetPropertyAttribute<TType, TAttribute>(Expression<Func<TType, object>> property) where TAttribute : Attribute
        {
            var body = property.Body;

            // control especial para tipos int, bool and double
            // there is a boxing operation happening under the covers: the int is boxed in order to pass as an object.
            if (body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            if (body is not MemberExpression memberExpression)
                throw new ArgumentException("Expression must be a property");

            return memberExpression.Member.GetCustomAttribute<TAttribute>() ?? throw new Exception("No such attribute is found.");
        }

        /// <summary>
        /// Devuelve el attributo DisplayName asociado a un Clase dada a través de su Tipo.
        /// </summary>
        /// <typeparam name="T">La clase que contiene el DisplayName como Type.</typeparam>
        public static string GetClassDisplayName<T>()
        {
            var displayName = typeof(T).GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute;

            return displayName?.DisplayName ?? string.Empty;
        }

        /// <summary>
        /// Devuelve una lista con los DisplayName asociados a las propiedades de una Clase a través de tu Tipo.
        /// </summary>
        /// <typeparam name="T">La clase que contiene las propiedades con DisplayName como Type.</typeparam>
        public static List<string> GetPropertiesDisplayName<T>()
        {
            var propertiesDisplayNames = new List<string>();
            var properties = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor property in properties) propertiesDisplayNames.Add(property.DisplayName);

            return propertiesDisplayNames;
        }
    }
}