using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class Overshare() : MnemonistCard(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("PerDeck", 10m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
    
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        var shackleAmount = 1 + PileType.Draw.GetPile(Owner).Cards.Count / DynamicVars["PerDeck"].IntValue;
        await PowerCmd.Apply<StrengthPower>(cardPlay.Target, -1*shackleAmount, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);
}