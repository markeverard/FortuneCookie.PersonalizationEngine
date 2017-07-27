using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace FortuneCookie.PersonalizationEngine.Reflection
{
    internal class AttributedTypesUtility
    {
        internal static List<Type> GetTypesWithAttribute(Type attributeType)
        {
            var typesWithAttribute = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> typesWithAttributeInAssembly = GetTypesWithAttributeInAssembly(assembly, attributeType);
                typesWithAttribute.AddRange(typesWithAttributeInAssembly);
            }

            return typesWithAttribute;
        }

        private static IEnumerable<Type> GetTypesWithAttributeInAssembly(Assembly assembly, Type attributeType)
        {
            Type[] types = assembly.GetTypes();
            return types.Where(type => TypeHasAttribute(type, attributeType)).ToList();
        }

        private static bool TypeHasAttribute(Type type, Type attributeType)
        {
            bool typeHasAttribute = false;
            object[] attributes = GetAttributes(type);

            foreach (object attribute in attributes)
            {
                if (attributeType.IsAssignableFrom(attribute.GetType()))
                    typeHasAttribute = true;
            }

            return typeHasAttribute;
        }

        private static object[] GetAttributes(Type type)
        {
            return type.GetCustomAttributes(true); ;
        }

        internal static Attribute GetAttribute(Type type, Type attributeType)
        {
            Attribute attribute = null;

            object[] attributes = type.GetCustomAttributes(true);
            foreach (object attributeInType in attributes)
            {
                if (attributeType.IsAssignableFrom(attributeInType.GetType()))
                    attribute = (Attribute)attributeInType;
            }

            return attribute;
        }
    }
}