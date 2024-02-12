using System.Reflection;
using System.Runtime.CompilerServices;

namespace GhostLyzer.Core.Utils
{
    /// <summary>
    /// Provides methods for retrieving types from assemblies.
    /// </summary>
    public static class TypeProvider
    {
        /// <summary>
        /// Determines whether the specified type is a record type.
        /// </summary>
        /// <param name="objectType">The type to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is a record type; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method checks for the presence of a compiler-generated method and an EqualityContract property,
        /// which are characteristics of record types.
        /// </remarks>
        private static bool IsRecord(this Type objectType)
        {
            // Check if the type has a compiler-generated method which is a characteristic of record types
            if (objectType.GetMethod("<Clone>$") != null)
            {
                return true;
            }

            // Check if the type has an EqualityContract property which is a characteristic of record types
            var equalityContractProperty = ((TypeInfo)objectType).GetDeclaredProperty("EqualityContract");

            if (equalityContractProperty != null)
            {
                var getMethod = equalityContractProperty.GetMethod;
                if (getMethod != null && getMethod.IsDefined(typeof(CompilerGeneratedAttribute), false))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the type from the referencing assembly.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>
        /// The type from the referencing assembly if found; otherwise, <c>null</c>.
        /// </returns>
        /// <remarks>
        /// This method searches the assemblies referenced by the entry assembly for a type with the specified name.
        /// The type name can be either the full name or the simple name of the type.
        /// </remarks>
        public static Type? GetTypeFromReferencingAssembly(string typeName)
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
                return null;

            var referencedAssemblies = new HashSet<string>(entryAssembly.GetReferencedAssemblies().Select(a => a.FullName));

            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => referencedAssemblies.Contains(a.FullName ?? string.Empty))
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == typeName || t.Name == typeName);
        }

        /// <summary>
        /// Gets the first type from the current domain's assemblies that matches the specified type name.
        /// </summary>
        /// <param name="typeName">The full name or the simple name of the type to find.</param>
        /// <returns>
        /// The first type that matches the specified name, or <c>null</c> if no such type is found.
        /// </returns>
        /// <remarks>
        /// This method searches all types in all assemblies in the current domain.
        /// It stops searching as soon as it finds the first type that matches the specified name.
        /// </remarks>
        public static Type? GetFirstMatchingTypeFromCurrentDomainAssembly(string typeName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == typeName || t.Name == typeName);
        }
    }
}
