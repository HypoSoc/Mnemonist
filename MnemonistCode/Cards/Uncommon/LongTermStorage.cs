using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Uncommon;

public class LongTermStorage() : MnemonistCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Memory", 3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Memory>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 0, 999);
        var memoryAmount = 0;
        foreach (CardModel card in (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).ToList<CardModel>())
        {
            await CardCmd.Exhaust(choiceContext, card);
            memoryAmount += DynamicVars["Memory"].IntValue;
        }
        await PowerCmd.Apply<Memory>(Owner.Creature, memoryAmount, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.AddKeyword(MnemonistKeywords.Persistent);
    }
}