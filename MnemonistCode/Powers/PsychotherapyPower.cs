using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mnemonist.MnemonistCode.Cards;
using Mnemonist.MnemonistCode.Cards.Humors;

namespace Mnemonist.MnemonistCode.Powers;

public class PsychotherapyPower : MnemonistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private bool _choleric = false;
    private bool _melancholic = false;
    private bool _phlegmatic = false;
    private bool _sanguine = false;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MnemonistKeywords.Createshumors)];
    
    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool causedByEthereal)
    {
        if (card.Owner.Creature != Owner || card is not Humor)
            return;
        if (card is Choleric)
            _choleric = true;
        if (card is Melancholic)
            _melancholic = true;
        if (card is Phlegmatic)
            _phlegmatic = true;
        if (card is Sanguine)
            _sanguine = true;
        if (_choleric && _melancholic && _phlegmatic && _sanguine)
        {
            _choleric = false;
            _melancholic = false;
            _phlegmatic = false;
            _sanguine = false;
            Flash();
            await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner, null);
        }
    }
}