using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Uncommon;

public class StriveToRemember() : MnemonistCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrivePower>(2m)];
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.PlayerCombatState is null) 
            return;
        await CardPileCmd.Draw(choiceContext, 10 - Owner.PlayerCombatState.Hand.Cards.Count, Owner);
        if (Owner.Creature.HasPower<StrivePower>())
            return;
        await PowerCmd.Apply<StrivePower>(Owner.Creature, DynamicVars["StrivePower"].IntValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => DynamicVars["StrivePower"].UpgradeValueBy(1m);
}