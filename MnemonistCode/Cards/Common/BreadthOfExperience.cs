using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mnemonist.MnemonistCode.Cards.Common;

public class BreadthOfExperience() : MnemonistCard(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12m, ValueProp.Move), new IntVar("Size", 20m)];

    private bool _isDiscounted = false;

    private Task SetDiscount()
    {
        if (!IsMutable)
            return Task.CompletedTask;
        try
        {
            if (_isDiscounted)
            {
                if (PileType.Draw.GetPile(Owner).Cards.Count < DynamicVars["Size"].IntValue)
                {
                    _isDiscounted = false;
                    EnergyCost.AddThisCombat(1);
                }
            }
            else
            {
                if (PileType.Draw.GetPile(Owner).Cards.Count >= DynamicVars["Size"].IntValue)
                {
                    _isDiscounted = true;
                    EnergyCost.AddThisCombat(-1, reduceOnly: true);
                }
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

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_big_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}