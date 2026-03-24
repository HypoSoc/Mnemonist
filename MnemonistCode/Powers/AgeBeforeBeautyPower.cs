using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Mnemonist.MnemonistCode.Powers;

public class AgeBeforeBeautyPower : MnemonistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<DexterityPower>()];
    
    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        CombatState combatState)
    {
        if (player != Owner.Player)
            return;
        if (PileType.Draw.GetPile(player).Cards.Count < 20)
            return;
        Flash();
        await PowerCmd.Apply<AgeBeforeBeautyStrPower>(Owner, Amount, Owner, null, true);
        await PowerCmd.Apply<AgeBeforeBeautyDexPower>(Owner, Amount, Owner, null, true);
    }
}