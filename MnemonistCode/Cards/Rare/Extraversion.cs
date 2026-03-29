using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Mnemonist.MnemonistCode.Cards.Humors;

namespace Mnemonist.MnemonistCode.Cards.Rare;

public class Extraversion() : MnemonistCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0M),
        new ExtraDamageVar(3M),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier( (card, _) => PileType.Draw.GetPile(card.Owner).Cards.Count(c => c is Humor) + card.DynamicVars["Humors"].IntValue),
        new IntVar("Humors", 4)
    ];
    
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        MnemonistKeywords.Psych,
        MnemonistKeywords.Createshumors,
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        if (CombatState is null)
            return;
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt").Execute(choiceContext);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(Humor.CreateRandom(Owner, DynamicVars["Humors"].IntValue, CombatState, IsUpgraded), PileType.Draw, true, CardPilePosition.Random), 0.2f);
    }
    
    protected override void OnUpgrade() {
        this.AddKeyword(MnemonistKeywords.Psychplus);
        this.RemoveKeyword(MnemonistKeywords.Psych);
    }
}