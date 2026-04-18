using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
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
    protected override object InitInternalData() => new Data();

    public override Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        var internalData = GetInternalData<Data>();
        internalData.ChoiceContext = choiceContext;
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
        var internalData = GetInternalData<Data>();
        internalData.CardsToExhaust = 0;
        decimal cardsInDiscard = PileType.Discard.GetPile(target.Player).Cards.Count;
        decimal cardsLeftToExhaust = cardsInDiscard - internalData.CardsToExhaust;
        if (cardsLeftToExhaust > amount)
        {
            internalData.CardsToExhaust += amount;
            return 0;
        }

        internalData.CardsToExhaust = cardsInDiscard;
        if (amount > cardsLeftToExhaust)
            internalData.DidDamage = true;
        return amount - cardsLeftToExhaust;
    }
    
    public override async Task AfterModifyingHpLostAfterOsty()
    {
        if (Owner.Player is null)
            return;
        var internalData = GetInternalData<Data>();
        if (internalData.CardsToExhaust == 0)
        {
            if (internalData.DidDamage)
            {
                if (Owner.Player.Character is Character.Mnemonist character0)
                    character0.PlayAnimation(Owner, "hit");
            }
            internalData.DidDamage = false;
            return;
        }
        if (internalData.ChoiceContext is null)
            return;
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return;
        }
        if (Owner.Player.Character is Character.Mnemonist character)
            character.PlayAnimation(Owner, "cast");
        var lockInCardsToExhaust = internalData.CardsToExhaust;
        var lockedChoiceContext = internalData.ChoiceContext;
        var lockedDidDamage = internalData.DidDamage;
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
                    PileType.Discard.GetPile(Owner.Player).InvokeCardRemoveFinished();
                }
            }
        }
        SaveManager.Instance.PrefsSave.FastMode = originalMode;
        if (Owner.Player.Character is Character.Mnemonist character2)
        {
            character2.PlayAnimation(Owner, lockedDidDamage ? "hit" : "idle_loop");
        }
        internalData.CardsToExhaust = 0;
        internalData.ChoiceContext = null;
        internalData.DidDamage = false;
    }
    
    private class Data
    {
        public decimal CardsToExhaust = 0;
        public PlayerChoiceContext? ChoiceContext = null;
        public bool DidDamage = false;
    }
}