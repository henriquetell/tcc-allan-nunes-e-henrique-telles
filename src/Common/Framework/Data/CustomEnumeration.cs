using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Framework.Data
{
    //REF: https://github.com/HeadspringLabs/Enumeration/blob/master/Enumeration.cs
    [Serializable]
    [DebuggerDisplay("{DisplayName} - {Value}")]
    public abstract class CustomEnumeration<TEnumeration, TValue> : IComparable<TEnumeration>, IEquatable<TEnumeration>, ICustomEnumeration
        where TEnumeration : CustomEnumeration<TEnumeration, TValue>
        where TValue : IComparable
    {
        private static readonly Lazy<TEnumeration[]> Enumerations = new Lazy<TEnumeration[]>(GetEnumerations);

        [DataMember(Order = 1)]
        readonly string _displayName;

        [DataMember(Order = 0)]
        readonly TValue _value;

        protected CustomEnumeration(TValue value, string displayName)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            _value = value;
            _displayName = displayName;
        }

        public TValue Value => _value;

        public string DisplayName => _displayName;

        string ICustomEnumeration.Value => Value.ToString();

        string ICustomEnumeration.DisplayName => DisplayName;

        public int CompareTo(TEnumeration other)
        {
            return Value.CompareTo(other == default(TEnumeration) ? default(TValue) : other.Value);
        }

        public sealed override string ToString() => DisplayName;

        public static TEnumeration[] GetAll() => Enumerations.Value;

        private static TEnumeration[] GetEnumerations()
        {
            Type enumerationType = typeof(TEnumeration);
            return enumerationType
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(info => enumerationType.IsAssignableFrom(info.FieldType))
                .Select(info => info.GetValue(null))
                .Cast<TEnumeration>()
                .ToArray();
        }

        public override bool Equals(object obj) => Equals(obj as TEnumeration);

        public bool Equals(TEnumeration other) => other != null && ValueEquals(other.Value);

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(CustomEnumeration<TEnumeration, TValue> left, CustomEnumeration<TEnumeration, TValue> right) => Equals(left, right);

        public static bool operator !=(CustomEnumeration<TEnumeration, TValue> left, CustomEnumeration<TEnumeration, TValue> right) => !Equals(left, right);

        public static TEnumeration FromValue(TValue value) => Parse(value, "value", item => item.Value.Equals(value));

        public static TEnumeration Parse(string displayName) => Parse(displayName, "display name", item => item.DisplayName == displayName);

        static bool TryParse(Func<TEnumeration, bool> predicate, out TEnumeration result)
        {
            result = GetAll().FirstOrDefault(predicate);
            return result != null;
        }

        private static TEnumeration Parse(object value, string description, Func<TEnumeration, bool> predicate)
        {
            TEnumeration result;

            if (!TryParse(predicate, out result))
            {
                string message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(TEnumeration));
                throw new ArgumentException(message, "value");
            }

            return result;
        }

        public static bool TryParse(TValue value, out TEnumeration result) => TryParse(e => e.ValueEquals(value), out result);

        public static bool TryParse(string displayName, out TEnumeration result) => TryParse(e => e.DisplayName == displayName, out result);

        protected virtual bool ValueEquals(TValue value) => Value.Equals(value);
    }

    public interface ICustomEnumeration
    {
        string Value { get; }

        string DisplayName { get; }
    }
}
