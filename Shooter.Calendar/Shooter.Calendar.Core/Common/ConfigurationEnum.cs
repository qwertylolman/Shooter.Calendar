using System;
using System.Linq;
using Shooter.Calendar.Core.Common.Extensions;
using Shooter.Calendar.Core.Attributes;

namespace Shooter.Calendar.Core.Common
{
    public abstract class ConfigurationEnum
    {
        protected ConfigurationEnum(string value)
        {
            value.ThrowIfNullOrEmpty(nameof(value));

            Value = value;
        }

        public string Value { get; }

        public override string ToString()
            => Value;

        public virtual bool Equals(string otherValue)
            => EqualsValue(Value, otherValue);

        public override bool Equals(object obj)
            => Equals((ConfigurationEnum)obj);

        protected bool Equals([NotNull] ConfigurationEnum other)
            => EqualsValue(Value, other.Value);

        public override int GetHashCode()
            => Value?.GetHashCode() ?? 0;

        /// <summary>
        /// Produce an instance of entity inherited from ConfigurationEnum or returns one frome the list of predefined
        /// </summary>
        /// <typeparam name="T">Type of configuration enum</typeparam>
        /// <param name="value">Value of enum</param>
        /// <param name="factoryMethod">Factory method to produce new instance of configuration enum implementation</param>
        /// <param name="predefined">Possible predefined lists of configuration enums</param>
        /// <returns>Instance</returns>
        protected static T Produce<T>(string value, Func<string, T> factoryMethod, params T[] predefined)
            where T : ConfigurationEnum
        {
            var configurationEnum = GetFromPredefined(value, predefined);
            if (configurationEnum != null)
            {
                return configurationEnum;
            }

            configurationEnum = factoryMethod(value);

            return configurationEnum;
        }

        /// <summary>
        /// Return an instance of entity inherited from ConfigurationEnum frome the list of predefined
        /// </summary>
        /// <typeparam name="T">Type of configuration enum</typeparam>
        /// <param name="value">Value of enum</param>
        /// <param name="predefined">Possible predefined lists of configuration enums</param>
        /// <returns>Instance</returns>
        protected static T GetFromPredefined<T>(string value, params T[] predefined)
            where T : ConfigurationEnum
            => predefined.FirstOrDefault(ce => EqualsValue(ce.Value, value));

        /// <summary>
        /// Compare string values
        /// </summary>
        /// <param name="value1">value from first object</param>
        /// <param name="value2">value from second object</param>
        /// <returns>Comparison result</returns>
        protected static bool EqualsValue(string value1, string value2)
            => string.Equals(value1, value2, StringComparison.CurrentCultureIgnoreCase);
    }
}
