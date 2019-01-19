using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Realms;
using Shooter.Calendar.Core.Attributes;
using Shooter.Calendar.Core.Common;

namespace Shooter.Calendar.Core.RealmExtra.CopyManager
{
    public class CopyManager : ICopyManager
    {
        private readonly IEnumerable<ICopyManagerProvider> providers;

        public CopyManager()
        {
            providers = IoCExtensions.ResolveMany<ICopyManagerProvider>();
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
