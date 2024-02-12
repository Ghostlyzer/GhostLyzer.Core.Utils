using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace GhostLyzer.Core.Utils
{
    /// <summary>
    /// Transforms outbound parameters by converting PascalCase or camelCase to kebab-case.
    /// </summary>
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        /// <summary>
        /// Transforms the specified value into kebab-case.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>
        /// The transformed value in kebab-case, or <c>null</c> if the value is <c>null</c>.
        /// </returns>
        public string? TransformOutbound(object? value)
        {
            return value == null ? null : Regex.Replace(value.ToString() ?? string.Empty, "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
