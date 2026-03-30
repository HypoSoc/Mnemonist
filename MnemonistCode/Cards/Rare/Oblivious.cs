using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class Oblivious() : MnemonistCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0M),
        new CalculationExtraVar(2M),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier( (card, _) => card.CombatState != null ? card.Owner.Creature.GetPowerAmount<Memory>() : 0),
        new IntVar("PerEnergy", 3m)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Memory>()];
    
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), DynamicVars.CalculatedBlock.Props, cardPlay);
        var energyGain = Owner.Creature.GetPowerAmount<Memory>() / DynamicVars["PerEnergy"].IntValue;
        await PowerCmd.Remove<Memory>(Owner.Creature);
        if (energyGain > 0)
        {
            await PlayerCmd.GainEnergy(energyGain, Owner);
            await CardPileCmd.Draw(choiceContext, energyGain, this.Owner); 
        }
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}