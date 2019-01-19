using Realms;
using System.Collections.Generic;
using System;

namespace Shooter.Calendar.Core.POCO.Entities
{
    public class Exercise : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<Shot> Shots { get; }

        public DateTimeOffset TimeStamp { get; set; }
    }
}
