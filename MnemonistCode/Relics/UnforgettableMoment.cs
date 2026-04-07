using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Mnemonist.MnemonistCode.Cards;
using Mnemonist.MnemonistCode.Character;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Relics;

[Pool(typeof(MnemonistRelicPool))]
public class UnforgettableMoment() : MnemonistRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Common;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Memory>(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MnemonistKeywords.Persistent), HoverTipFactory.FromPower<Memory>()];

    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool causedByEthereal)
    {
        if (!card.IsPersistent())
            return;
        Flash();
        await PowerCmd.Apply<Memory>(Owner.Creature, DynamicVars["Memory"].IntValue, Owner.Creature, null);
    }
}