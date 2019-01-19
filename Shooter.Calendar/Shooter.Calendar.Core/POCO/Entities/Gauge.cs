using Realms;

namespace Shooter.Calendar.Core.POCO.Entities
{
    public class Gauge : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
