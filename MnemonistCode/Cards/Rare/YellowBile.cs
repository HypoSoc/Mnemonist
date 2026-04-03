using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Mnemonist.MnemonistCode.Cards.Humors;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class YellowBile() : MnemonistCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<YellowBilePower>(4m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MnemonistKeywords.Createshumors), HoverTipFactory.FromCard<Choleric>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
            return;
        await PowerCmd.Apply<YellowBilePower>(Owner.Creature, DynamicVars["YellowBilePower"].BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => DynamicVars["YellowBilePower"].UpgradeValueBy(2M);
}