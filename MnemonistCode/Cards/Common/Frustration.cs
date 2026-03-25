using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mnemonist.MnemonistCode.Cards.Common;

public class Frustration() : MnemonistCard(2, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move), new IntVar("Size", 10m)];

    private int _discountAmount = 0;

    private Task SetDiscount()
    {
        if (!IsMutable)
            return Task.CompletedTask;
        if (this.Pile is null || !Pile.IsCombatPile)
            return Task.CompletedTask;
        try
        {
            var discountAmount = PileType.Exhaust.GetPile(Owner).Cards.Count / DynamicVars["Size"].IntValue;
            discountAmount = Math.Min(2, discountAmount);
            if (discountAmount != this._discountAmount)
            {
                EnergyCost.AddThisCombat(this._discountAmount);
                EnergyCost.AddThisCombat(-1*discountAmount);
                this._discountAmount = discountAmount;
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
        await SetDiscount();
    }
    
    public override async Task AfterCardEnteredCombat(CardModel card)
    {
        if (card == this)
            await SetDiscount();
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState)
            .WithHitFx("vfx/vfx_big_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}