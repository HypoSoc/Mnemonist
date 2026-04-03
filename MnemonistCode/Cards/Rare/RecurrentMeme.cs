using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class RecurrentMeme() : MnemonistCard(-1, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12m, ValueProp.Move)];
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Unplayable,
    ];

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        return Task.CompletedTask;
    }
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner != player)
            return;
        if (Pile?.Type != PileType.Exhaust)
            return;
        if (CombatState is null)
            return;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingRandomOpponents(CombatState).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        await CardPileCmd.Add(this, PileType.Hand);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}