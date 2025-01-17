﻿using Content.Server.Botany.Components;
using Content.Shared.FixedPoint;
using Robust.Server.GameObjects;
using Robust.Shared.GameObjects;
using Robust.Shared.Maths;

namespace Content.Server.Botany.Systems;

public partial class BotanySystem
{
    public void ProduceGrown(EntityUid uid, ProduceComponent produce)
    {
        if (!_prototypeManager.TryIndex<SeedPrototype>(produce.SeedName, out var seed))
            return;

        if (TryComp(uid, out SpriteComponent? sprite))
        {
            sprite.LayerSetRSI(0, seed.PlantRsi);
            sprite.LayerSetState(0, seed.PlantIconState);
        }

        var solutionContainer = _solutionContainerSystem.EnsureSolution(uid, produce.SolutionName);

        solutionContainer.RemoveAllSolution();
        foreach (var (chem, quantity) in seed.Chemicals)
        {
            var amount = FixedPoint2.New(quantity.Min);
            if (quantity.PotencyDivisor > 0 && seed.Potency > 0)
                amount += FixedPoint2.New(seed.Potency / quantity.PotencyDivisor);
            amount = FixedPoint2.New((int) MathHelper.Clamp(amount.Float(), quantity.Min, quantity.Max));
            solutionContainer.MaxVolume += amount;
            solutionContainer.AddReagent(chem, amount);
        }
    }
}
