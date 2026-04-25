using System;

namespace Outplay.Core
{
    /// <summary>
    /// Anything that has a current/max value and notifies on change —
    /// HP, mana, future cooldowns, boss bars, etc. Domain-agnostic.
    /// </summary>
    public interface IResourceProvider
    {
        float Current { get; }
        float Max { get; }
        event Action<float, float> OnChanged;
    }
}
