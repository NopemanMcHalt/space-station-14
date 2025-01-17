﻿using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Server.Chemistry.ReagentEffects;

public class Oxygenate : ReagentEffect
{
    [DataField("factor")]
    public float Factor = 1f;

    public override void Effect(ReagentEffectArgs args)
    {
        if (args.EntityManager.TryGetComponent<RespiratorComponent>(args.SolutionEntity, out var resp))
        {
            var respSys = EntitySystem.Get<RespiratorSystem>();
            respSys.UpdateSaturation(resp.Owner, args.Quantity.Float() * Factor, resp);
        }
    }
}
