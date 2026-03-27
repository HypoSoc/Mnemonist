using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mnemonist.MnemonistCode.Powers;

public class StrivePower: MnemonistPower
{
    private bool _initialCard = true;
    private bool _preventPlay = false;
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType
    {
        get
        {
            if (_preventPlay)
                return PowerStackType.Single;
            return PowerStackType.Counter;
        }
    }

    public override bool ShouldPlay(CardModel card, AutoPlayType _)
    {
        return card.Owner.Creature != Owner || !_preventPlay;
    }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || _preventPlay)
            return;
        if (_initialCard)
        {
            _initialCard = false;
            return;
        }
        if (Amount == 1)
        {
            _preventPlay = true;
            this.Amount = 0;
            return;
        }
        await PowerCmd.Decrement(this);
    }

    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side)
            return;
        await PowerCmd.Remove(this);
    }
}