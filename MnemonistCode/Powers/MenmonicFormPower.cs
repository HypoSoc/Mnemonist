using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using Mnemonist.MnemonistCode.Cards;

namespace Mnemonist.MnemonistCode.Powers;

public class MnemonicFormPower : MnemonistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Memory>(), HoverTipFactory.FromCard<Engram>()];
    
    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side)
            return;
        var damage = Owner.GetPowerAmount<Memory>() * Amount;
        if (damage <= 0)
            return;
        Flash();
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, damage, ValueProp.Unpowered, Owner, null);
    }
}