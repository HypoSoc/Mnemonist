using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class PersistentMemory() : MnemonistCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        MnemonistKeywords.Persistent,
        CardKeyword.Exhaust,
    ];

    private Decimal _extraDamageFromShuffle;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move), new DynamicVar("Increase", 5M)];

    private Decimal ExtraDamageFromShuffle
    {
        get => this._extraDamageFromShuffle;
        set
        {
            this.AssertMutable();
            this._extraDamageFromShuffle = value;
        }
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }
    
    public override Task AfterCardChangedPiles(
        CardModel card,
        PileType oldPileType,
        AbstractModel? source)
    {
        if (card != this || oldPileType == PileType.Draw || card.Pile is null || card.Pile.Type != PileType.Draw)
        {
            return Task.CompletedTask;
        }
        DamageVar damage = this.DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + this.DynamicVars["Increase"].BaseValue;
        this.ExtraDamageFromShuffle += this.DynamicVars["Increase"].BaseValue;
        return Task.CompletedTask;
    }
    
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + this.ExtraDamageFromShuffle;
    }

    protected override void OnUpgrade() => this.DynamicVars["Increase"].UpgradeValueBy(3M);
}