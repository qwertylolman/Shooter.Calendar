using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MvvmCross;
using Shooter.Calendar.Core.Attributes;

namespace Shooter.Calendar.Core.Common
{
    public static class IoCExtensions
    {
        private static readonly ConcurrentDictionary<Type, HashSet<Type>> singletonRegistrations;
        private static readonly ConcurrentDictionary<Type, HashSet<Func<object>>> singletonConstructorsRegistrations;
        private static readonly ConcurrentDictionary<Type, HashSet<object>> singletonInstanceRegistrations;
        private static readonly ConcurrentDictionary<Type, HashSet<Type>> prototypeRegistrations;

        static IoCExtensions()
        {
            singletonRegistrations = new ConcurrentDictionary<Type, HashSet<Type>>();
            singletonConstructorsRegistrations = new ConcurrentDictionary<Type, HashSet<Func<object>>>();
            singletonInstanceRegistrations = new ConcurrentDictionary<Type, HashSet<object>>();
            prototypeRegistrations = new ConcurrentDictionary<Type, HashSet<Type>>();
        }

        public static void RegisterSingleton<TInterface, TService>()
            where TInterface : class
            where TService : class, TInterface
        {
            Mvx.IoCProvider.RegisterSingleton<TInterface>(() => IocConstruct<TService>());
        }

        public static void RegisterManyAsSingleton<TInterface, TService>()
            where TInterface : class
            where TService : class, TInterface
        {
            singletonRegistrations.GetOrAdd(typeof(TInterface), type => new HashSet<Type>())
                .Add(typeof(TService));
        }

        public static void RegisterManyAsSingleton<TInterface>([NotNull] Func<TInterface> serviceConstructor)
            where TInterface : class
        {
            singletonConstructorsRegistrations.GetOrAdd(typeof(TInterface), type => new HashSet<Func<object>>())
                .Add(serviceConstructor);
        }

        public static void RegisterManyAsSingleton<TInterface>([NotNull] TInterface service)
            where TInterface : class
        {
            singletonInstanceRegistrations.GetOrAdd(typeof(TInterface), type => new HashSet<object>())
                .Add(service);
        }

        public static void RegisterManyAsType<TInterface, TService>()
            where TInterface : class
            where TService : class, TInterface
        {
            prototypeRegistrations.GetOrAdd(typeof(TInterface), type => new HashSet<Type>())
                .Add(typeof(TService));
        }

        public static TInterface[] ResolveMany<TInterface>()
            where TInterface : class
            => DoResolveMany<TInterface>().ToArray();

        private static IEnumerable<TInterface> DoResolveMany<TInterface>()
            where TInterface : class
        {
            var targetType = typeof(TInterface);

            var isAnySingletonInstanceRegistration = singletonInstanceRegistrations.TryGetValue(targetType, out var singletonInstances);
            if (!isAnySingletonInstanceRegistration)
            {
                var isAnySingletonRegistration = singletonRegistrations.TryGetValue(targetType, out var singletonTypes);
                if (isAnySingletonRegistration)
                {
                    singletonInstances = new HashSet<object>();
                    foreach (var type in singletonTypes)
                    {
                        var instance = IocConstruct(type);
                        singletonInstances.Add(instance);
                        yield return instance as TInterface;
                    }

                    singletonInstanceRegistrations.AddOrUpdate(targetType, type => singletonInstances, (type, set) => singletonInstances);
                }

                var isAnySingletonConstructor = singletonConstructorsRegistrations.TryGetValue(targetType, out var constructors);
                if (isAnySingletonConstructor)
                {
                    singletonInstances = singletonInstances ?? new HashSet<object>();
                    foreach (var ctor in constructors)
                    {
                        var instance = ctor();
                        singletonInstances.Add(instance);
                        yield return instance as TInterface;
                    }

                    singletonInstanceRegistrations.AddOrUpdate(targetType, type => singletonInstances, (type, set) => singletonInstances);
                }
            }
            else
            {
                foreach (var instance in singletonInstances)
                {
                    yield return instance as TInterface;
                }
            }

            var isAnyPrototypeRegistration = prototypeRegistrations.TryGetValue(targetType, out var prototypeTypes);
            if (isAnyPrototypeRegistration)
            {
                foreach (var type in prototypeTypes)
                {
                    var instance = IocConstruct(type);
                    yield return instance as TInterface;
                }
            }
        }

        public static TService IocConstruct<TService>() 
            where TService : class
            => Mvx.IoCProvider.IoCConstruct<TService>();

        public static object IocConstruct([NotNull] Type type)
            => Mvx.IoCProvider.IoCConstruct(type);
    }
}
