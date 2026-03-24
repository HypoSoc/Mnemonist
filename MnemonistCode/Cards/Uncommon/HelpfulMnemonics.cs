using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Uncommon;

public class HelpfulMnemonics() : MnemonistCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<HelpfulMnemonicsPower>(1m), new IntVar("Engrams", 3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Engram>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
            return;
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(Engram.Create(Owner, DynamicVars["Engrams"].IntValue, CombatState).ToList(), PileType.Draw, true, CardPilePosition.Random), 0.2f);
        await PowerCmd.Apply<HelpfulMnemonicsPower>(Owner.Creature, DynamicVars["HelpfulMnemonicsPower"].BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => DynamicVars["Engrams"].UpgradeValueBy(3M);
}