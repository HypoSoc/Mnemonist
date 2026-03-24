using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class EngramStrike() : MnemonistCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    private Decimal _extraDamageFromEngrams;
    
    protected override HashSet<CardTag> CanonicalTags =>
    [
        CardTag.Strike
    ];
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Retain,
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3m, ValueProp.Move), new DynamicVar("Increase", 2M)];
    
    private Decimal ExtraDamageFromEngrams
    {
        get => this._extraDamageFromEngrams;
        set
        {
            this.AssertMutable();
            this._extraDamageFromEngrams = value;
        }
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }
    
    public override Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (this.Pile?.Type != PileType.Hand)
            return Task.CompletedTask;
        if (card.Owner != Owner)
        {
            return Task.CompletedTask;
        }
        if (card is not Engram)
            return Task.CompletedTask;
        DamageVar damage = this.DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + this.DynamicVars["Increase"].BaseValue;
        this.ExtraDamageFromEngrams += this.DynamicVars["Increase"].BaseValue;
        return Task.CompletedTask;
    }
    
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + this.ExtraDamageFromEngrams;
    }

    protected override void OnUpgrade() => this.DynamicVars["Increase"].UpgradeValueBy(1M);
}