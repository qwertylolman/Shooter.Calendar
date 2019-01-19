using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Realms;

namespace Shooter.Calendar.Core.RealmExtra.CopyManager.PredefinedProviders
{
    public class ReflectionCopyProvider : ICopyManagerProvider
    {
        public bool CanHandle(RealmObject realmObject)
            => true;

        public RealmObject MakeACopy(RealmObject realmObject)
        {
            if (realmObject == null)
            {
                return null;
            }

            var copy = Activator.CreateInstance(realmObject.GetType()) as RealmObject;

            var declaredProperties = realmObject
                .GetType()
                .GetTypeInfo()
                .DeclaredProperties;

            // IList<> will not be here, because they are readonly properties
            var realmObjectType = typeof(RealmObject);
            var grouppedProperties = declaredProperties
                .Where(p => p.CanRead == true
                    && p.CanWrite == true)
                .GroupBy(p => realmObjectType.IsAssignableFrom(p.PropertyType))
                .ToList();

            // set RealmObject typed properties
            foreach (var p in grouppedProperties.Where(g => g.Key == true).SelectMany(g => g))
            {
                p.SetValue(copy, MakeACopy(p.GetValue(realmObject) as RealmObject));
            }

            // set other typed properties
            foreach (var p in grouppedProperties.Where(g => g.Key == false).SelectMany(g => g))
            {
                p.SetValue(copy, p.GetValue(realmObject));
            }

            var genericListDefinition = typeof(IList<>);
            var listPropertiesGroups = declaredProperties
                .Where(p =>
                {
                    var typeInfo = p.PropertyType.GetTypeInfo();
                    return p.CanRead == true
                        && p.CanWrite == false
                        && typeInfo.IsGenericType == true
                        && typeInfo.GetGenericTypeDefinition() == genericListDefinition;
                })
                .GroupBy(p =>
                {
                    var typeInfo = p.PropertyType.GetTypeInfo();
                    return realmObjectType.IsAssignableFrom(typeInfo.GenericTypeArguments.First());
                })
                .ToList();

            // set list items for RealmObjects
            foreach (var p in listPropertiesGroups.Where(g => g.Key == true).SelectMany(g => g))
            {
                var copyPropertyList = p.GetValue(copy) as IList;
                var originalList = p.GetValue(realmObject) as IEnumerable;

                foreach (var item in originalList)
                {
                    copyPropertyList.Add(MakeACopy(item as RealmObject));
                }
            }

            // set list items for other objects
            foreach (var p in listPropertiesGroups.Where(g => g.Key == false).SelectMany(g => g))
            {
                var copyPropertyList = p.GetValue(copy) as IList;
                var originalList = p.GetValue(realmObject) as IEnumerable;

                foreach (var item in originalList)
                {
                    copyPropertyList.Add(item);
                }
            }

            return copy;
        }
    }
}
