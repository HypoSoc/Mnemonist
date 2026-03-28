using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Mnemonist.MnemonistCode.Cards.Humors;

namespace Mnemonist.MnemonistCode.Cards.Uncommon;

public class Depression() : MnemonistCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10m, ValueProp.Move), new PowerVar<WeakPower>(2), new IntVar("Melancholics", 3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(),  HoverTipFactory.FromKeyword(MnemonistKeywords.Createshumors), HoverTipFactory.FromCard<Melancholic>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
        await PowerCmd.Apply<WeakPower>(cardPlay.Target, DynamicVars["WeakPower"].IntValue, Owner.Creature, this);
        if (CombatState is null)
            return;
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(Humor.Create<Melancholic>(Owner, DynamicVars["Melancholics"].IntValue, CombatState), PileType.Discard, true), 0.2f);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
        DynamicVars["WeakPower"].UpgradeValueBy(1);
        DynamicVars["Melancholics"].UpgradeValueBy(1);
    }
}