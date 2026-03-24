using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class MnemonicForm() : MnemonistCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Memory>(0)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Memory>(), HoverTipFactory.FromCard<Engram>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<Memory>(Owner.Creature, DynamicVars["Memory"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<MnemonicFormPower>(Owner.Creature, 1, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => DynamicVars["Memory"].UpgradeValueBy(10M);
}