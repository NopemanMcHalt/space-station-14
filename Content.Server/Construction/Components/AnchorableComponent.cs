using Content.Shared.Tools;
using Robust.Shared.Analyzers;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.ViewVariables;

namespace Content.Server.Construction.Components
{
    [RegisterComponent, Friend(typeof(AnchorableSystem))]
    public class AnchorableComponent : Component
    {
        [DataField("tool", customTypeSerializer:typeof(PrototypeIdSerializer<ToolQualityPrototype>))]
        public string Tool { get; private set; } = "Anchoring";

        [DataField("snap")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool Snap { get; private set; } = true;

        /// <summary>
        /// Base delay to use for anchoring.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("delay")]
        public float Delay = 0.5f;
    }

    public abstract class BaseAnchoredAttemptEvent : CancellableEntityEventArgs
    {
        public EntityUid User { get; }
        public EntityUid Tool { get; }

        /// <summary>
        ///     Extra delay to add to the do_after.
        ///     Add to this, don't replace it.
        ///     Output parameter.
        /// </summary>
        public float Delay { get; set; } = 0f;

        protected BaseAnchoredAttemptEvent(EntityUid user, EntityUid tool)
        {
            User = user;
            Tool = tool;
        }
    }

    public class AnchorAttemptEvent : BaseAnchoredAttemptEvent
    {
        public AnchorAttemptEvent(EntityUid user, EntityUid tool) : base(user, tool) { }
    }

    public class UnanchorAttemptEvent : BaseAnchoredAttemptEvent
    {
        public UnanchorAttemptEvent(EntityUid user, EntityUid tool) : base(user, tool) { }
    }

    public abstract class BaseAnchoredEvent : EntityEventArgs
    {
        public EntityUid User { get; }
        public EntityUid Tool { get; }

        protected BaseAnchoredEvent(EntityUid user, EntityUid tool)
        {
            User = user;
            Tool = tool;
        }
    }

    /// <summary>
    ///     Raised just before the entity's body type is changed.
    /// </summary>
    public class BeforeAnchoredEvent : BaseAnchoredEvent
    {
        public BeforeAnchoredEvent(EntityUid user, EntityUid tool) : base(user, tool) { }
    }

    /// <summary>
    ///     Raised when an entity with an anchorable component is anchored. Note that you may instead want the more
    ///     general <see cref="AnchorStateChangedEvent"/>. This event has the benefit of having user & tool information,
    ///     as a result of interactions mediated by the <see cref="AnchorableSystem"/>.
    /// </summary>
    public class UserAnchoredEvent : BaseAnchoredEvent
    {
        public UserAnchoredEvent(EntityUid user, EntityUid tool) : base(user, tool) { }
    }

    /// <summary>
    ///     Raised just before the entity's body type is changed.
    /// </summary>
    public class BeforeUnanchoredEvent : BaseAnchoredEvent
    {
        public BeforeUnanchoredEvent(EntityUid user, EntityUid tool) : base(user, tool) { }
    }

    /// <summary>
    ///     Raised when an entity with an anchorable component is unanchored. Note that you will probably also need to
    ///     subscribe to the more general <see cref="AnchorStateChangedEvent"/>, which gets raised BEFORE this one. This
    ///     event has the benefit of having user & tool information, whereas the more general event may be due to
    ///     explosions or grid-destruction or other interactions not mediated by the <see cref="AnchorableSystem"/>.
    /// </summary>
    public class UserUnanchoredEvent : BaseAnchoredEvent
    {
        public UserUnanchoredEvent(EntityUid user, EntityUid tool) : base(user, tool) { }
    }
}
