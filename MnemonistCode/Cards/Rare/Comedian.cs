using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Mnemonist.MnemonistCode.Cards.Humors;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class Comedian() : MnemonistCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MnemonistKeywords.Createshumors)];
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null)
            return;
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 0, 999);
        var cardAmount = 0;
        foreach (CardModel card in (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).ToList<CardModel>())
        {
            await CardCmd.Exhaust(choiceContext, card);
            cardAmount += 1;
        }
        if (cardAmount == 0)
            return;
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(Humor.CreateRandom(Owner, cardAmount, CombatState, IsUpgraded), PileType.Draw, true, CardPilePosition.Random), 0.2f);
        await CardPileCmd.AddGeneratedCardsToCombat(Humor.CreateRandom(Owner, cardAmount, CombatState, IsUpgraded), PileType.Hand, true, CardPilePosition.Random);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(Humor.CreateRandom(Owner, cardAmount, CombatState, IsUpgraded), PileType.Discard, true, CardPilePosition.Random), 0.2f);
    }
}