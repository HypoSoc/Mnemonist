using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Common;

public class AccumulatingMemories() : MnemonistCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
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
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("Memory", 3M),
        new IntVar("Increase", 1M)
    ];
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
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<Memory>(this.Owner.Creature, DynamicVars["Memory"].IntValue, this.Owner.Creature, (CardModel) this, false);
    }

    public override Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool causedByEthereal)
    {
        if (card != this)
            return Task.CompletedTask;
        int intValue = DynamicVars["Increase"].IntValue;
        BuffFromPlay(intValue);
        if (!(DeckVersion is AccumulatingMemories deckVersion))
            return Task.CompletedTask;
        deckVersion.BuffFromPlay(intValue);
        return Task.CompletedTask;
    }

    private void BuffFromPlay(int extraMemory)
    {
        this.IncreasedMemory += extraMemory;
        this.UpdateMemory();
    }
    
    private void UpdateMemory() => this.CurrentMemory = 3 + this.IncreasedMemory;
    protected override void OnUpgrade() => this.DynamicVars["Increase"].UpgradeValueBy(1M);
}