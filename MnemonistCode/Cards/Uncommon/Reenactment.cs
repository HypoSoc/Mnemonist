using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Mnemonist.MnemonistCode.Cards.Humors;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Uncommon;

public class Reenactment() : MnemonistCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Humors", 2m), new IntVar("Memory", 3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MnemonistKeywords.Createshumors), HoverTipFactory.FromPower<Memory>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
            return;
        await PowerCmd.Apply<Memory>(Owner.Creature, -1*DynamicVars["Memory"].IntValue, Owner.Creature, this, false);
        await CardPileCmd.AddGeneratedCardsToCombat(Humor.CreateRandom(Owner, DynamicVars["Humors"].IntValue, CombatState), PileType.Hand, true, CardPilePosition.Random);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["Humors"].UpgradeValueBy(1);
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