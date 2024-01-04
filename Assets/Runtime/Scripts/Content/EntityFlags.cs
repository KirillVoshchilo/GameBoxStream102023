using System.Linq;

namespace App.Content
{
    public sealed class EntityFlags
    {
        private readonly string[] _values;

        public EntityFlags(string[] values)
            => _values = values;

        public bool HasFlag(string flag)
            => _values.Contains(flag);
    }
}