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
public class IdyllicMemories() : MnemonistRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Memory>(10)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Memory>()];

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        IdyllicMemories idyllicMemories = this;
        if (side != Owner.Creature.Side || combatState.RoundNumber > 1)
            return;
        idyllicMemories.Flash();
        await PowerCmd.Apply<Memory>(Owner.Creature, DynamicVars["Memory"].IntValue, Owner.Creature, null);
    }
    
    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<TumultuousPresent>();
    }
}