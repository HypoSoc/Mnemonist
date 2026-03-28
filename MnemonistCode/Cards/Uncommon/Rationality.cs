using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Mnemonist.MnemonistCode.Cards.Humors;

namespace Mnemonist.MnemonistCode.Cards.Uncommon;

public class Rationality() : MnemonistCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [ new IntVar("Phlegmatics", 2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MnemonistKeywords.Createshumors), HoverTipFactory.FromCard<Phlegmatic>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel? card = PileType.Discard.GetPile(Owner).Cards.LastOrDefault();
        if (card != null)
        {
            await CardCmd.Exhaust(choiceContext, card);
        }
        if (CombatState is null)
            return;
        await CardPileCmd.AddGeneratedCardsToCombat(Humor.Create<Phlegmatic>(Owner, DynamicVars["Phlegmatics"].IntValue, CombatState), PileType.Hand, true);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Phlegmatics"].UpgradeValueBy(1);
    }
}