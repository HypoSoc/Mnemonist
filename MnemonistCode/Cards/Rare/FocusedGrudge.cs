using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class FocusedGrudge() : MnemonistCard(0, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FocusedGrudgePower>(10), new IntVar("Size", 3)];
    
    private int _penaltyAmount = 0;

    private Task SetPenalty()
    {
        if (!IsMutable)
            return Task.CompletedTask;
        if (this.Pile is null || !Pile.IsCombatPile)
            return Task.CompletedTask;
        try
        {
            var penaltyAmount = (PileType.Draw.GetPile(Owner).Cards.Count + PileType.Discard.GetPile(Owner).Cards.Count) / DynamicVars["Size"].IntValue;
            if (penaltyAmount != this._penaltyAmount)
            {
                EnergyCost.AddThisCombat(-1*this._penaltyAmount);
                EnergyCost.AddThisCombat(penaltyAmount);
                this._penaltyAmount = penaltyAmount;
            }
        }
        catch (InvalidOperationException)
        {
            
        }
        return Task.CompletedTask;
    }
    
    public override async Task AfterCardChangedPiles(
        CardModel card,
        PileType oldPileType,
        AbstractModel? source)
    {
        await SetPenalty();
    }
    
    public override async Task AfterCardEnteredCombat(CardModel card)
    {
        if (card == this)
            await SetPenalty();
    }


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FocusedGrudgePower>(Owner.Creature, DynamicVars["FocusedGrudgePower"].IntValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => this.AddKeyword(MnemonistKeywords.Persistent);
}