using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class Commitment() : MnemonistCard(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12m, ValueProp.Move), new CardsVar(3)];
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Innate,
        CardKeyword.Exhaust,
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
            return;
        await CardPileCmd.Draw(choiceContext, DynamicVars["Cards"].IntValue, this.Owner);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        var originalMode = SaveManager.Instance.PrefsSave.FastMode;
        if (originalMode != FastModeType.Fast && originalMode != FastModeType.Instant)
            SaveManager.Instance.PrefsSave.FastMode = FastModeType.Fast;
        foreach (var card in PileType.Draw.GetPile(Owner).Cards.ToList())
        {
            await CardCmd.Exhaust(choiceContext, card);
        }
        SaveManager.Instance.PrefsSave.FastMode = originalMode;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}