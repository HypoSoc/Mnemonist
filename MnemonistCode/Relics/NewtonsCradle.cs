using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Mnemonist.MnemonistCode.Cards;
using Mnemonist.MnemonistCode.Cards.Humors;
using Mnemonist.MnemonistCode.Character;

namespace Mnemonist.MnemonistCode.Relics;

[Pool(typeof(MnemonistRelicPool))]
public class NewtonsCradle() : MnemonistRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Humors", 2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MnemonistKeywords.Psych)];

    public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
    {
        if (shuffler != Owner || Owner.Creature.CombatState == null)
            return;
        Flash();
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(Humor.CreateRandom(Owner, DynamicVars["Humors"].IntValue, Owner.Creature.CombatState), PileType.Draw, true, CardPilePosition.Random), 0.2f);
        await CardPileCmd.Draw(choiceContext, 1, Owner);
    }
}