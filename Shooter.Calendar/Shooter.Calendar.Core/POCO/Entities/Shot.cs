using Realms;

namespace Shooter.Calendar.Core.POCO.Entities
{
    public class Shot : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public int BulletsCount { get; set; }

        public Weapon Weapon { get; set; }

        public ShotZone ShotZone { get; set; }
    }
}
