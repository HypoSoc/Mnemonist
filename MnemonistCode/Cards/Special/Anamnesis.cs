using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Mnemonist.MnemonistCode.Relics;

namespace Mnemonist.MnemonistCode.Cards.Special;

public class Anamnesis() : MnemonistCard(1,
    CardType.Skill, CardRarity.Curse,
    TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cranialTrauma = Owner.GetRelic<CranialTrauma>();
        if (cranialTrauma == null)
            return;
        await cranialTrauma.UseUnexhaust();
        CardCmd.ApplyKeyword(this, CardKeyword.Unplayable);
    }
    public override int MaxUpgradeLevel => 0;
}