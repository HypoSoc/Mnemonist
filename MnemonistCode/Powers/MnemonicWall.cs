using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;
using Mnemonist.MnemonistCode.Cards;

namespace Mnemonist.MnemonistCode.Powers;

public class MnemonicWall : MnemonistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    private decimal _cardsToExhaust = 0;
    private PlayerChoiceContext? _choiceContext = null;

    public override Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        this._choiceContext = choiceContext;
        return Task.CompletedTask;
    }

    public override Decimal ModifyHpLostAfterOstyLate(
        Creature target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target != Owner)
            return amount;
        if (target.Player is null)
            return amount;
        _cardsToExhaust = 0;
        decimal cardsInDiscard = PileType.Discard.GetPile(target.Player).Cards.Count;
        decimal cardsLeftToExhaust = cardsInDiscard - _cardsToExhaust;
        if (cardsLeftToExhaust > amount)
        {
            this._cardsToExhaust += amount;
            return 0;
        }

        this._cardsToExhaust = cardsInDiscard;
        return amount - cardsLeftToExhaust;
    }
    
    public override async Task AfterModifyingHpLostAfterOsty()
    {
        if (Owner.Player is null)
            return;
        if (this._cardsToExhaust == 0)
            return;
        if (this._choiceContext is null)
            return;
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return;
        }
        var lockInCardsToExhaust = this._cardsToExhaust;
        var lockedChoiceContext = this._choiceContext;
        var discardPile = PileType.Discard.GetPile(Owner.Player);
        var originalMode = SaveManager.Instance.PrefsSave.FastMode;
        if (originalMode != FastModeType.Fast && originalMode != FastModeType.Instant)
            SaveManager.Instance.PrefsSave.FastMode = FastModeType.Fast;
        var k = 0;
        var engramExhausted = false;
        for (int i = 0; i < lockInCardsToExhaust; ++i)
        {
            CardModel? card = discardPile.Cards.LastOrDefault<CardModel>();
            if (card != null)
            {
                var shouldSkip = k >= 5;
                if (card is Engram)
                {
                    if (!engramExhausted)
                    {
                        engramExhausted = true;
                    }
                    else
                    {
                        shouldSkip = true;
                    }
                }
                if (!shouldSkip)
                {
                    k += 1;
                }
                await CardCmd.Exhaust(lockedChoiceContext, card, skipVisuals: shouldSkip);
                if (shouldSkip)
                {
                    PileType.Exhaust.GetPile(Owner.Player).InvokeCardAddFinished();
                }
            }
        }
        SaveManager.Instance.PrefsSave.FastMode = originalMode;
        this._cardsToExhaust = 0;
        this._choiceContext = null;
    }
}