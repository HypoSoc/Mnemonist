using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace Mnemonist.MnemonistCode.Powers;

public class MindlessMasteryPower : MnemonistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override bool TryModifyEnergyCostInCombat(
        CardModel card,
        Decimal originalCost,
        out Decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (!this.ShouldModifyCost(card))
            return false;
        modifiedCost = 0M;
        return true;
    }

    public override bool TryModifyStarCost(
        CardModel card,
        Decimal originalCost,
        out Decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (!this.ShouldModifyCost(card))
            return false;
        modifiedCost = 0M;
        return true;
    }

    private bool ShouldModifyCost(CardModel card)
    {
        if (!CombatManager.Instance.IsInProgress || card.Owner.Creature != this.Owner || Owner.Player is null)
            return false;
        return !PileType.Draw.GetPile(Owner.Player).Cards.Any();
    }
}