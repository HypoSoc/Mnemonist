using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Mnemonist.MnemonistCode.Cards.Humors;

namespace Mnemonist.MnemonistCode.Cards.Common;

public class GoodHumors() : MnemonistCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Humors", 2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MnemonistKeywords.Createshumors)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
            return;
        List<Humor> cards = [];
        cards.AddRange(Humor.Create<Choleric>(Owner, DynamicVars["Humors"].IntValue, CombatState, IsUpgraded));
        cards.AddRange(Humor.Create<Melancholic>(Owner, DynamicVars["Humors"].IntValue, CombatState, IsUpgraded));
        cards.AddRange(Humor.Create<Phlegmatic>(Owner, DynamicVars["Humors"].IntValue, CombatState, IsUpgraded));
        cards.AddRange(Humor.Create<Sanguine>(Owner, DynamicVars["Humors"].IntValue, CombatState, IsUpgraded));
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(cards.UnstableShuffle(Owner.RunState.Rng.CombatCardGeneration), PileType.Discard, true), 0.2f);
    }
}