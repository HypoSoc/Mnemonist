using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Mnemonist.MnemonistCode.Character;
using Mnemonist.MnemonistCode.Powers;
using Mnemonist.MnemonistCode.Relics;

namespace Mnemonist.MnemonistCode.Relics;

[Pool(typeof(MnemonistRelicPool))]
public class TumultuousPresent() : MnemonistRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Memory>(5), new("Turns", 4M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Memory>()];

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side || combatState.RoundNumber > this.DynamicVars["Turns"].BaseValue)
            return;
        this.Flash();
        await PowerCmd.Apply<Memory>(Owner.Creature, DynamicVars["Memory"].IntValue, Owner.Creature, null);
    }
}