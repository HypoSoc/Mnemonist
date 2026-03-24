using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mnemonist.MnemonistCode.Cards.Common;

public class Ruminate() : MnemonistCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2), new IntVar("Mill", 10m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [EnergyHoverTip];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
            return;
        var cardsToMill = Math.Min(PileType.Draw.GetPile(Owner).Cards.Count(), DynamicVars["Mill"].IntValue);
        if (cardsToMill > 0)
            await CardPileCmd.Add(PileType.Draw.GetPile(Owner).Cards.Take(cardsToMill), PileType.Discard);
        if (PileType.Draw.GetPile(Owner).Cards.Any())
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }

    protected override void OnUpgrade() => DynamicVars.Energy.UpgradeValueBy(1M);
}