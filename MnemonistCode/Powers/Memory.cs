using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using Mnemonist.MnemonistCode.Cards;

namespace Mnemonist.MnemonistCode.Powers;

public class Memory : MnemonistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Engram>()];

    
    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != CombatSide.Enemy)
            return;
        if (Owner.Player is null)
            return;
        
        PileType pileType = Owner.HasPower<MnemonicFormPower>() ? PileType.Draw : PileType.Discard;
        CardPilePosition pilePosition = Owner.HasPower<MnemonicFormPower>() ? CardPilePosition.Random : CardPilePosition.Bottom;

        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(Engram.Create(Owner.Player, Amount, combatState).ToList(), pileType, true, pilePosition), 0.2f);
        await PowerCmd.Remove(this);
    }
}