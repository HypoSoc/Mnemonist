using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using Mnemonist.MnemonistCode.Character;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Relics;

[Pool(typeof(MnemonistRelicPool))]
public class SpireDiary() : MnemonistRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;
    
    private int _currentMemory;
    private int _increasedMemory;
    
    [SavedProperty]
    public int CurrentMemory
    {
        get => this._currentMemory;
        set
        {
            this.AssertMutable();
            this._currentMemory = value;
            this.DynamicVars["Memory"].BaseValue = (Decimal) this._currentMemory;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Memory>(5), new IntVar("Increase", 5)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Memory>()];

    [SavedProperty]
    public int IncreasedMemory
    {
        get => this._increasedMemory;
        set
        {
            this.AssertMutable();
            this._increasedMemory = value;
        }
    }
    
    private void BuffFromRest()
    {
        this.IncreasedMemory += DynamicVars["Increase"].IntValue;
        this.UpdateMemory();
        InvokeDisplayAmountChanged();
    }
    
    private void UpdateMemory() => this.CurrentMemory = DynamicVars["Increase"].IntValue + this.IncreasedMemory;

    public override bool ShowCounter => true;
    public override int DisplayAmount => DynamicVars["Memory"].IntValue;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side || combatState.RoundNumber != 2)
            return;
        await PowerCmd.Apply<Memory>(Owner.Creature, DynamicVars["Memory"].IntValue, Owner.Creature, null);
    }
    
    public override Task AfterRestSiteHeal(Player player, bool isMimicked)
    {
        if (player != Owner)
            return Task.CompletedTask;
        Flash();
        BuffFromRest();
        return Task.CompletedTask;
    }
    
    public override IReadOnlyList<LocString> ModifyExtraRestSiteHealText(
        Player player,
        IReadOnlyList<LocString> currentExtraText)
    {
        if (!LocalContext.IsMe(this.Owner))
            return currentExtraText;
        IReadOnlyList<LocString> locStringList = currentExtraText;
        int index = 0;
        LocString[] items = new LocString[1 + locStringList.Count];
        foreach (LocString locString in locStringList)
        {
            items[index] = locString;
            ++index;
        }
        if (AdditionalRestSiteHealText is not null)
            items[index] = this.AdditionalRestSiteHealText;
        return items;
    }
}