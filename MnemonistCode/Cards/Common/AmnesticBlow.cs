using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Common;

public class AmnesticBlow() : MnemonistCard(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12m, ValueProp.Move), new IntVar("Memory", 3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Memory>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_starry_impact")
            .Execute(choiceContext);
        await PowerCmd.Apply<Memory>(this.Owner.Creature, -1*DynamicVars["Memory"].IntValue, this.Owner.Creature, (CardModel) this, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Memory"].UpgradeValueBy(-1);
    }

    protected override bool IsPlayable
    {
        get
        {
            var memoryPower = Owner.Creature.GetPower<Memory>();
            if (memoryPower is null)
                return false;
            return memoryPower.Amount >= DynamicVars["Memory"].IntValue;
        }
    }
}