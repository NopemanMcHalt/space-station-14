﻿using System;
using Content.Shared.Alert;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.Timing;

namespace Content.Server.Chemistry.ReagentEffects;

public class AdjustAlert : ReagentEffect
{
    [DataField("alertType", required: true)]
    public AlertType Type;

    [DataField("clear")]
    public bool Clear;

    [DataField("cooldown")]
    public bool Cooldown;

    [DataField("time")]
    public float Time;

    public override void Effect(ReagentEffectArgs args)
    {
        var alertSys = EntitySystem.Get<AlertsSystem>();
        if (args.EntityManager.HasComponent<AlertsComponent>(args.SolutionEntity))
        {
            if (Clear)
            {
                alertSys.ClearAlert(args.SolutionEntity, Type);
            }
            else
            {
                (TimeSpan, TimeSpan)? cooldown = null;
                if (Cooldown)
                {
                    var timing = IoCManager.Resolve<IGameTiming>();
                    cooldown = (timing.CurTime, timing.CurTime + TimeSpan.FromSeconds(Time));
                }
                alertSys.ShowAlert(args.SolutionEntity, Type, cooldown: cooldown);
            }
        }
    }
}
