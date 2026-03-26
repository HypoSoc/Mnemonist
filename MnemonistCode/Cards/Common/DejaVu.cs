using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mnemonist.MnemonistCode.Cards.Common;

public class DejaVu() : MnemonistCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    private Decimal _extraDamageFromPlays;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(6M, ValueProp.Move), new DynamicVar("Increase", 4M)];

    private Decimal ExtraDamageFromPlays
    {
        get => this._extraDamageFromPlays;
        set
        {
            this.AssertMutable();
            this._extraDamageFromPlays = value;
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue += DynamicVars["Increase"].BaseValue;
        ExtraDamageFromPlays += DynamicVars["Increase"].BaseValue;
        await CardPileCmd.Add(this, PileType.Draw);
    }

    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue += ExtraDamageFromPlays;
    }

    protected override void OnUpgrade() => DynamicVars["Increase"].UpgradeValueBy(2M);
}