using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mnemonist.MnemonistCode.Cards.Basic;

public class ActiveRecall() : MnemonistCard(0,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Retain,
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromSimpleGrid(choiceContext, 
            PileType.Discard.GetPile(Owner).Cards, 
            Owner, prefs)).FirstOrDefault<CardModel>();
        if (card == null)
            return;
        await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top);
    }

    protected override void OnUpgrade()
    {
        this.AddKeyword(CardKeyword.Innate);
    }
}