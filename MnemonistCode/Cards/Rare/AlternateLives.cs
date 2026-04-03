using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class AlternateLives() : MnemonistCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(30)];
    
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = CardFactory.GetForCombat(Owner,
            Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
                .Where((c => c.Rarity is CardRarity.Common or CardRarity.Uncommon)), DynamicVars.Cards.IntValue,
            Owner.RunState.Rng.CombatCardGeneration);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Discard, true));
    }
    
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(10);
}