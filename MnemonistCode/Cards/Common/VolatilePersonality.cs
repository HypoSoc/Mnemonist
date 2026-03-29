using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Mnemonist.MnemonistCode.Cards.Humors;

namespace Mnemonist.MnemonistCode.Cards.Common;

public class VolatilePersonality() : MnemonistCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Humors", 2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MnemonistKeywords.Createshumors)];
    
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        MnemonistKeywords.Psych,
        MnemonistKeywords.Createshumors,
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
            return;
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(Humor.CreateRandom(Owner, DynamicVars["Humors"].IntValue, CombatState, IsUpgraded), PileType.Draw, true, CardPilePosition.Random), 0.2f);
        await CardPileCmd.Draw(choiceContext, 1, Owner);
    }
    
    protected override void OnUpgrade() {
        this.AddKeyword(MnemonistKeywords.Psychplus);
        this.RemoveKeyword(MnemonistKeywords.Psych);
    }
}