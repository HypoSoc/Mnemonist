using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mnemonist.MnemonistCode.Cards;

namespace Mnemonist.MnemonistCode.Powers;

public class CognitiveIgnitionPower : MnemonistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Engram>()];
    
    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool causedByEthereal)
    {
        if (card is not Engram)
            return;
        
        IReadOnlyList<Creature> hittableEnemies = CombatState.HittableEnemies;
        if (hittableEnemies.Count == 0 || Owner.Player is null)
            return;
        Creature? target = Owner.Player.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
        if (target is null)
            return;
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, Amount, ValueProp.Unpowered, Owner, null);
    }
}