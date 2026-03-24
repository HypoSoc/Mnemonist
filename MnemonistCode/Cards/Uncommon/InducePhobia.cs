using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Uncommon;

public class InducePhobia() : MnemonistCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Memory", 3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<Memory>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        var memoryAmount = Owner.Creature.GetPower<Memory>()?.Amount;
        if (memoryAmount is null || memoryAmount.Value < DynamicVars["Memory"].IntValue)
            return;
        var shackleAmount = memoryAmount.Value / DynamicVars["Memory"].IntValue;
        await PowerCmd.Apply<InducePhobiaPower>(cardPlay.Target, shackleAmount, Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["Memory"].UpgradeValueBy(-1);
    }
}