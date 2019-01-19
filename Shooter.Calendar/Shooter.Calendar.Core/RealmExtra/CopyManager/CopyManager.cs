using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Realms;
using Shooter.Calendar.Core.Attributes;

namespace Shooter.Calendar.Core.RealmExtra.CopyManager
{
    public class CopyManager
    {
        private readonly IEnumerable<ICopyManagerProvider> providers;

        public CopyManager()
        {
            providers = container.ResolveMany<ICopyManagerProvider>().ToList();
        }

        public RealmObject MakeACopy([NotNull] RealmObject realmObject)
        {
            var provider = providers.LastOrDefault(p => p.CanHandle(realmObject));

            return provider == null
                ? FallbackCopyimplementation(realmObject)
                : provider.MakeACopy(realmObject);
        }

        private RealmObject FallbackCopyimplementation(RealmObject realmObject)
            => JsonConvert.DeserializeObject(JsonConvert.SerializeObject(realmObject), realmObject.GetType()) as RealmObject;
    }
}
