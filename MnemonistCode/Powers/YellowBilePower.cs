using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mnemonist.MnemonistCode.Cards;
using Mnemonist.MnemonistCode.Cards.Humors;

namespace Mnemonist.MnemonistCode.Powers;

public class YellowBilePower : MnemonistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MnemonistKeywords.Createshumors)];
    
    private static bool IsPoweredAttack(ValueProp props)
    {
        return props.HasFlag(ValueProp.Move) && !props.HasFlag(ValueProp.Unpowered);
    }

    public void FlashPublic()
    {
        Flash();
    }

    public override Decimal ModifyDamageAdditive(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? card)
    {
        return Owner != dealer || !IsPoweredAttack(props) || card == null || card is not Humor ? 0M : Amount;
    }
}