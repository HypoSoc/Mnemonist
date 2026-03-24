using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Mnemonist.MnemonistCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace Mnemonist.MnemonistCode.Powers;

public class RecursiveMemoryPower : MnemonistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Engram>(), HoverTipFactory.FromPower<Memory>()];
    
    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (card.Owner.Creature != Owner)
            return;
        if (card is not Engram)
            return;
        Flash();
        await PowerCmd.Apply<Memory>(this.Owner, this.Amount, this.Owner, null, true);
    }
}