using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mnemonist.MnemonistCode.Cards.Common;

public class Contemplate() : MnemonistCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier( (card, _) => (card.CombatState != null ? PileType.Draw.GetPile(card.Owner).Cards.Count(c => c is Engram) + card.DynamicVars["Engrams"].IntValue : 0)),
        new IntVar("Engrams", 2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Engram>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
            return;
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), DynamicVars.CalculatedBlock.Props, cardPlay);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(Engram.Create(Owner, DynamicVars["Engrams"].IntValue, CombatState).ToList(), PileType.Draw, true, CardPilePosition.Random), 0.2f);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Engrams"].UpgradeValueBy(2);
    }
}