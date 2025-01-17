﻿using System.Collections.Generic;
using Content.Server.Body.Components;
using Content.Shared.Atmos;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Server.Chemistry.ReagentEffects;

public class ModifyLungGas : ReagentEffect
{
    [DataField("ratios", required: true)]
    private Dictionary<Gas, float> _ratios = default!;

    public override void Effect(ReagentEffectArgs args)
    {
        if (args.EntityManager.TryGetComponent<LungComponent>(args.OrganEntity, out var lung))
        {
            foreach (var (gas, ratio) in _ratios)
            {
                lung.Air.Moles[(int) gas] += (ratio * args.Quantity.Float()) / Atmospherics.BreathMolesToReagentMultiplier;
            }
        }
    }
}
