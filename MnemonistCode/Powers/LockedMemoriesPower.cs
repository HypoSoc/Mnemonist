using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mnemonist.MnemonistCode.Powers;

public class LockedMemoriesPower : MnemonistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    private readonly List<CardModel> _unplayableCards = [];
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner.Player is null)
            return;
        if (Owner.Player != player)
            return;

        foreach (var card in PileType.Draw.GetPile(Owner.Player).Cards.Take(Amount))
        {
            if (card.Keywords.Contains(CardKeyword.Unplayable)) continue;
            _unplayableCards.Add(card);
            card.AddKeyword(CardKeyword.Unplayable);
        }
        await CardPileCmd.Draw(choiceContext, Amount, Owner.Player);
    }
    
    public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side)
            return Task.CompletedTask;
        foreach (var card in _unplayableCards)
        {
            card.RemoveKeyword(CardKeyword.Unplayable);
        }
        _unplayableCards.Clear();
        return Task.CompletedTask;
    }
}