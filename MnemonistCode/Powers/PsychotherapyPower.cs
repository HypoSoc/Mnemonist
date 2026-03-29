using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mnemonist.MnemonistCode.Cards;
using Mnemonist.MnemonistCode.Cards.Humors;

namespace Mnemonist.MnemonistCode.Powers;

public class PsychotherapyPower : MnemonistPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MnemonistKeywords.Createshumors)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BoolVar("Choleric", false),
        new BoolVar("Melancholic", false),
        new BoolVar("Phlegmatic", false),
        new BoolVar("Sanguine", false),
    ];
    
    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool causedByEthereal)
    {
        if (card.Owner.Creature != Owner || card is not Humor)
            return;
        if (card is Choleric && DynamicVars["Choleric"].IntValue == 0)
            DynamicVars["Choleric"].BaseValue = 1;
        if (card is Melancholic && DynamicVars["Melancholic"].IntValue == 0)
            DynamicVars["Melancholic"].BaseValue = 1;
        if (card is Phlegmatic && DynamicVars["Phlegmatic"].IntValue == 0)
            DynamicVars["Phlegmatic"].BaseValue = 1;
        if (card is Sanguine && DynamicVars["Sanguine"].IntValue == 0)
            DynamicVars["Sanguine"].BaseValue = 1;
        if (DynamicVars["Choleric"].IntValue == 1 &&
            DynamicVars["Melancholic"].IntValue == 1 &&
            DynamicVars["Phlegmatic"].IntValue == 1 &&
            DynamicVars["Sanguine"].IntValue == 1)
        {
            DynamicVars["Choleric"].BaseValue = 0;
            DynamicVars["Melancholic"].BaseValue = 0;
            DynamicVars["Phlegmatic"].BaseValue = 0;
            DynamicVars["Sanguine"].BaseValue = 0;
            Flash();
            await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner, null);
        }
    }
}