﻿using Content.Server.Power.NodeGroups;
using Robust.Shared.GameObjects;

namespace Content.Server.Power.Components
{
    /// <summary>
    ///     Connects the loading side of a <see cref="BatteryComponent"/> to a non-APC power network.
    /// </summary>
    [RegisterComponent]
    public class BatteryChargerComponent : BasePowerNetComponent
    {
        protected override void AddSelfToNet(IPowerNet net)
        {
            net.AddCharger(this);
        }

        protected override void RemoveSelfFromNet(IPowerNet net)
        {
            net.RemoveCharger(this);
        }
    }
}
