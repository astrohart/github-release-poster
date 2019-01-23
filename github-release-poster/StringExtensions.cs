using github_release_poster.Properties;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace github_release_poster
{
    /// <summary>
    /// Provides methods for working with strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets an instance of <see cref="T:System.Text.RegularExpressions.Regex"/> that can be used
        /// to match against urls of the form <pre>http://localhost/dir{?param1.param2,...,paramN}</pre>
        /// </summary>
        public static Regex OptionalQueryStringRegex { get; } = new Regex(
            Resources.HypermediaRelationUriTemplateRegex
        );

        /// <summary>
        /// Expands a URI template of the form <pre>http://localhost/dir{?param1.param2,...,paramN}</pre>.  You
        /// need to give this method a reference to an object (either anonymous or strongly-typed) whose properties
        /// are strings.  The properties' names should match the param1, param2, ..., paramN names exactly, and the
        /// values of the properties tell this method what to substitute for them.
        /// </summary>
        /// <param name="template">URI template to substitute.</param>
        /// <param name="values">Reference to an instance of an object whose properties' values should be substituted
        /// for the parameters.</param>
        /// <returns>URI in a conventional query string format, e.g.,
        /// <pre>http://localhost/dir?param1=val1&param2=val2</pre>.
        /// </returns>
        public static Uri ExpandUriTemplate(this string template, object values)
        {
            // Do not know why this would ever happen with an extension method, but you can't be
            // too careful...let's ensure we are working with valid input.
            if (string.IsNullOrWhiteSpace(template))
                throw new ArgumentNullException(nameof(template));

            // Try to match up against the OptionalQueryStringRegex.  If we can't, this means
            // that the {?param1,param2,etc} is not present, so we just return the input, encased
            // in a new URI object.
            var optionalQueryStringMatch = OptionalQueryStringRegex.Match(template);
            if (!optionalQueryStringMatch.Success)
                return new Uri(template);

            // Turn the inputted template into a query string-formatted URL.
            var expansion = string.Empty;
            var parameters = optionalQueryStringMatch.Groups[1].Value.Split(',');
            if (!parameters.Any())
                return new Uri(template);

            foreach (var parameter in parameters)
            {
                var parameterProperty = values.GetType().GetProperty(parameter);
                if (parameterProperty == null)
                    continue;

                expansion +=
                    string.IsNullOrWhiteSpace(expansion) ? "?" : "&";
                expansion +=
                    $"{parameter}={Uri.EscapeDataString($"{parameterProperty.GetValue(values, new object[0])}")}";
            }

            template = OptionalQueryStringRegex.Replace(template, expansion);
            return new Uri(template);
        }
    }
}