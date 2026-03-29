using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Mnemonist.MnemonistCode.Cards.Humors;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class Exuberance() : MnemonistCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ExuberancePower>(1m), new IntVar("Sanguine", 1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromCard<Sanguine>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
            return;
        await CardPileCmd.AddGeneratedCardsToCombat(Humor.Create<Sanguine>(Owner, DynamicVars["Sanguine"].IntValue, CombatState), PileType.Hand, true);
        await PowerCmd.Apply<ExuberancePower>(Owner.Creature, DynamicVars["ExuberancePower"].BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => this.DynamicVars["Sanguine"].UpgradeValueBy(1M);
}