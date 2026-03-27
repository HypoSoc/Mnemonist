using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mnemonist.MnemonistCode.Cards.Uncommon;

public class PrizedMemories() : MnemonistCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move), new IntVar("Size", 3m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await CardPileCmd.Draw(choiceContext, 1, Owner);
        if (PileType.Draw.GetPile(Owner).Cards.Count > DynamicVars["Size"].IntValue)
            return;
        float attackAnimDelay = Owner.Character.AttackAnimDelay;
        if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Normal)
            attackAnimDelay += 0.2f;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithAttackerAnim("Attack", attackAnimDelay).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Size"].UpgradeValueBy(2);
    }
}