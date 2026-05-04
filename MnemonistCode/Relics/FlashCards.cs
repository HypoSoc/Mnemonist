using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Mnemonist.MnemonistCode.Character;

namespace Mnemonist.MnemonistCode.Relics;

[Pool(typeof(MnemonistRelicPool))]
public class FlashCards() : MnemonistRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;
    
    private int _cardsDrawnThisTurn = 0;
        
    private int CardsDrawnThisTurn
    {
        get => this._cardsDrawnThisTurn;
        set
        {
            this.AssertMutable();
            this._cardsDrawnThisTurn = value;
            this.UpdateDisplay();
        }
    }
    
    public override bool ShowCounter => CombatManager.Instance.IsInProgress;

    public override int DisplayAmount => CardsDrawnThisTurn;
    
    private void UpdateDisplay()
    {
        int intValue = this.DynamicVars["Draw"].IntValue;
        this.InvokeDisplayAmountChanged();
    }


    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("Draw", 10),
        new PowerVar<StrengthPower>(1),
        new PowerVar<DexterityPower>(1)
    ];
    
    public override Task BeforeCombatStart()
    {
        this.CardsDrawnThisTurn = 0;
        this.Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }

    public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        this.CardsDrawnThisTurn = 0;
        this.Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }

    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (card.Owner != Owner || Owner.Creature.CombatState == null)
            return;
        if (CardsDrawnThisTurn >= DynamicVars["Draw"].IntValue)
            return;
        CardsDrawnThisTurn++;
        if (CardsDrawnThisTurn != DynamicVars["Draw"].IntValue)
            return;
        Flash();
        await PowerCmd.Apply<StrengthPower>(choiceContext,Owner.Creature, DynamicVars["StrengthPower"].IntValue, Owner.Creature, null);
        await PowerCmd.Apply<DexterityPower>(choiceContext,Owner.Creature, DynamicVars["DexterityPower"].IntValue, Owner.Creature, null);
    }
}