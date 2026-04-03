using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Uncommon;

public class NoteToSelf() : MnemonistCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1m),
        new CalculatedVar("Memory").WithMultiplier( (card, _) => (PileType.Draw.GetPile(card.Owner).Cards.Count(c => c.Type == CardType.Status) + PileType.Hand.GetPile(card.Owner).Cards.Count(c => c.Type == CardType.Status) + PileType.Discard.GetPile(card.Owner).Cards.Count(c => c.Type == CardType.Status))/2)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Memory>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var drawStatus = PileType.Draw.GetPile(Owner).Cards.Where(c => c.Type == CardType.Status).ToList();
        int memoryAmount = drawStatus.Count;
        foreach (var card in drawStatus)
        {
            await CardCmd.Exhaust(choiceContext, card, skipVisuals: true);
            PileType.Exhaust.GetPile(Owner).InvokeCardAddFinished();
        }

        if (IsUpgraded)
        {
            var handStatus = PileType.Hand.GetPile(Owner).Cards.Where(c => c.Type == CardType.Status).ToList();
            memoryAmount += handStatus.Count;
            foreach (var card in handStatus)
            {
                await CardCmd.Exhaust(choiceContext, card);
            }
            
            var discardStatus = PileType.Discard.GetPile(Owner).Cards.Where(c => c.Type == CardType.Status).ToList();
            memoryAmount += discardStatus.Count;
            foreach (var card in discardStatus)
            {
                await CardCmd.Exhaust(choiceContext, card, skipVisuals: true);
                PileType.Exhaust.GetPile(Owner).InvokeCardAddFinished();
            }
        }

        memoryAmount /= 2;
        if (memoryAmount > 0)
            await PowerCmd.Apply<SimpleMnemonicsPower>(Owner.Creature, memoryAmount, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => AddKeyword(CardKeyword.Retain);
}