using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Mnemonist.MnemonistCode.Relics;

namespace Mnemonist.MnemonistCode.Cards.Special;

public class Amnesia() : MnemonistCard(0,
    CardType.Skill, CardRarity.Curse,
    TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Anamnesis>()];
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Retain,
    ];

    private bool _didPlay;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cranialTrauma = Owner.GetRelic<CranialTrauma>();
        if (cranialTrauma is null || CombatState is null)
            return;
        await cranialTrauma.UseExhaust(choiceContext);
        _didPlay = true;
    }
    public override async Task AfterCardChangedPiles(
        CardModel card,
        PileType oldPileType,
        AbstractModel? source)
    {
        if (card != this || CombatState is null || !_didPlay)
        {
            return;
        }
        var anamnesis = CombatState.CreateCard<Anamnesis>(Owner);
        await CardCmd.Transform(this, anamnesis);
    }

    public override int MaxUpgradeLevel => 0;
}