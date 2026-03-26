using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mnemonist.MnemonistCode.Cards.Uncommon;

public class Lethologica() : MnemonistCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    private Decimal _extraDamageFromExhaust;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10m, ValueProp.Move), new DynamicVar("Increase", 3M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Engram>()];

    private Decimal ExtraDamageFromExhaust
    {
        get => this._extraDamageFromExhaust;
        set
        {
            this.AssertMutable();
            this._extraDamageFromExhaust = value;
        }
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_big_slash", tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext);
    }
    
    public override Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool causedByEthereal)
    {
        if (!IsMutable)
            return Task.CompletedTask;
        if (card is Engram)
            return Task.CompletedTask;
        if (card.Owner != Owner)
        {
            return Task.CompletedTask;
        }
        
        DamageVar damage = this.DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + this.DynamicVars["Increase"].BaseValue;
        this.ExtraDamageFromExhaust += this.DynamicVars["Increase"].BaseValue;
        return Task.CompletedTask;
    }
    
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + this.ExtraDamageFromExhaust;
    }

    protected override void OnUpgrade() => this.DynamicVars["Increase"].UpgradeValueBy(1M);
}