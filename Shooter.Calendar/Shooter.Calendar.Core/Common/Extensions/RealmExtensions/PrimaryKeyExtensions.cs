using System;
using System.Linq;
using System.Reflection;
using Realms;
using Shooter.Calendar.Core.Attributes;

namespace Shooter.Calendar.Core.Common.RealmExtensions.Extensions
{
    public static class PrimaryKeyExtensions
    {
        public static void ThrowIfNoPrimaryKey<TItem>()
            where TItem : RealmObject
        {
            if (HasPrimaryKey<TItem>() == true)
            {
                return;
            }

            throw new ArgumentException($"{typeof(TItem).Name} is expected to have read/write {typeof(string).Name} typed property marked with {nameof(PrimaryKeyAttribute)} attribute");
        }

        public static void ThrowIfNoPrimaryKey([NotNull] this RealmObject realmObject)
        {
            if (HasPrimaryKey(realmObject) == true)
            {
                return;
            }

            throw new ArgumentException($"{realmObject.GetType().Name} is expected to have read/write {typeof(string).Name} typed property marked with {nameof(PrimaryKeyAttribute)} attribute");
        }

        public static bool HasPrimaryKey<TItem>([NotNull] this TItem realmObject)
            where TItem : RealmObject
            => HasPrimaryKey(realmObject.GetType());

        public static bool HasPrimaryKey<TType>()
            where TType : RealmObject
            => HasPrimaryKey(typeof(TType));

        private static bool HasPrimaryKey(Type type)
            => type.GetTypeInfo().DeclaredProperties
                .FirstOrDefault(p =>
                    p.CanRead == true
                    && p.CanWrite == true
                    && p.GetCustomAttribute<PrimaryKeyAttribute>() != null) != null;

        public static string GetPrimaryKey([NotNull] this RealmObject realmObject)
            => realmObject.GetType().GetTypeInfo().DeclaredProperties
                .FirstOrDefault(p =>
                    p.CanRead == true
                    && p.GetCustomAttribute<PrimaryKeyAttribute>() != null)?
                .GetValue(realmObject)?
                .ToString();

        public static bool SetPrimaryKey([NotNull] this RealmObject realmObject, string key)
        {
            var typeInfo = realmObject.GetType().GetTypeInfo();

            var propertyInfo = realmObject.GetType().GetTypeInfo().DeclaredProperties
                .FirstOrDefault(p =>
                    p.CanWrite == true
                    && p.GetCustomAttribute<PrimaryKeyAttribute>() != null);

            if (propertyInfo == null)
            {
                return false;
            }

            propertyInfo.SetValue(realmObject, key);
            return true;
        }
    }
}
