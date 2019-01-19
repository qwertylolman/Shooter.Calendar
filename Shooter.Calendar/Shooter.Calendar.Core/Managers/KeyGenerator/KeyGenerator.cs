using System;

namespace Shooter.Calendar.Core.Managers.KeyGenerator
{
    public class KeyGenerator : IKeyGenerator
    {
        public string GenerateKey()
            => Guid.NewGuid().ToString();
    }
}
