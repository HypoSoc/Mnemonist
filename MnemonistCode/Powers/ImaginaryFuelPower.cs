using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Mnemonist.MnemonistCode.Powers;

public class ImaginaryFuelPower : MnemonistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Memory>()];
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side)
            return;
        for (int i = 0; i < Amount; ++i)
        {
            if (Owner.GetPowerAmount<Memory>() >= 2)
            {
                await PowerCmd.Apply<Memory>(Owner, -2, this.Owner, null);
                await PowerCmd.Apply<EnergyNextTurnPower>(Owner, 1, Owner, null);
            }
        }
    }
}