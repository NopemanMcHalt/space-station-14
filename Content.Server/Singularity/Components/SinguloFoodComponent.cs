using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;

namespace Content.Server.Singularity.Components
{
    /// <summary>
    /// Overrides exactly how much energy this object gives to a singularity.
    /// </summary>
    [RegisterComponent]
    public class SinguloFoodComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("energy")]
        public int Energy { get; set; } = 1;
    }
}
