using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Mnemonist.MnemonistCode.Cards;
using Mnemonist.MnemonistCode.Cards.Humors;

namespace Mnemonist.MnemonistCode.Potions;

public class SleevedJoker: MnemonistPotion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(MnemonistKeywords.Createshumors),
    ];

    protected override async Task OnUse(PlayerChoiceContext ctx, Creature? target)
    {
        if (target?.Player == null || target?.CombatState is null) return;
        await CardPileCmd.AddGeneratedCardsToCombat(Humor.Create<Choleric>(Owner, 1, target.CombatState, true), PileType.Hand, true);
        await CardPileCmd.AddGeneratedCardsToCombat(Humor.Create<Melancholic>(Owner, 1, target.CombatState, true), PileType.Hand, true);
        await CardPileCmd.AddGeneratedCardsToCombat(Humor.Create<Phlegmatic>(Owner, 1, target.CombatState, true), PileType.Hand, true);
        await CardPileCmd.AddGeneratedCardsToCombat(Humor.Create<Sanguine>(Owner, 1, target.CombatState, true), PileType.Hand, true);
    }
}