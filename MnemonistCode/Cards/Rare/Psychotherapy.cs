using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class Psychotherapy() : MnemonistCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PsychotherapyPower>(30m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MnemonistKeywords.Createshumors)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
            return;
        await PowerCmd.Apply<PsychotherapyPower>(Owner.Creature, DynamicVars["PsychotherapyPower"].BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => this.DynamicVars["PsychotherapyPower"].UpgradeValueBy(10M);
}