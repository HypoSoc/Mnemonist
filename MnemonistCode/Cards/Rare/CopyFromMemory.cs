using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class CopyFromMemory() : MnemonistCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Memory", 3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Memory>()];
    
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromSimpleGrid(choiceContext, 
            PileType.Exhaust.GetPile(Owner).Cards.Where(c => c is not Engram).ToList(), 
            Owner, prefs)).FirstOrDefault<CardModel>();
        if (card == null)
            return;
        CardModel clone = card.CreateClone();
        clone.DeckVersion = null;
        await CardPileCmd.AddGeneratedCardToCombat(clone, PileType.Hand, true);
        await PowerCmd.Apply<Memory>(Owner.Creature, -1*DynamicVars["Memory"].IntValue, Owner.Creature,  this);
        
    }

    protected override void OnUpgrade()
    {
        AddKeyword(MnemonistKeywords.Persistent);
    }

    protected override bool IsPlayable
    {
        get
        {
            var memoryPower = Owner.Creature.GetPower<Memory>();
            if (memoryPower is null)
                return false;
            return memoryPower.Amount >= DynamicVars["Memory"].IntValue;
        }
    }
}