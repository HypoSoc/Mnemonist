using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Common;

public class ComfortingThought() : MnemonistCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier( (card, _) => (card.CombatState != null ? card.Owner.Creature.GetPowerAmount<Memory>() + (card.IsUpgraded ? card.DynamicVars["Memory"].IntValue : 0) : 0)),
        new IntVar("Memory", 0m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Memory>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), DynamicVars.CalculatedBlock.Props, cardPlay);
        if (IsUpgraded)
            await PowerCmd.Apply<Memory>(Owner.Creature, DynamicVars["Memory"].IntValue, Owner.Creature, (CardModel) this, false);

    }

    protected override void OnUpgrade()
    {
        DynamicVars["Memory"].UpgradeValueBy(2);
    }
}