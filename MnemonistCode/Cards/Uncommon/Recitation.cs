using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards.Uncommon;

public class Recitation() : MnemonistCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override bool HasEnergyCostX => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move), new PowerVar<Memory>(2m), new IntVar("Engrams", 2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Engram>(), HoverTipFactory.FromPower<Memory>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        var xValue = ResolveEnergyXValue();
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(xValue).FromCard(this).Targeting(cardPlay.Target).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NStabVfx.Create(t, true, VfxColor.Gold))).Execute(choiceContext);
        if (xValue > 0)
        {
            await PowerCmd.Apply<Memory>(Owner.Creature, DynamicVars["Memory"].IntValue * xValue, Owner.Creature, this);
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(Engram.Create(Owner, DynamicVars["Engrams"].IntValue*xValue, CombatState).ToList(), PileType.Draw, true, CardPilePosition.Random), 0.2f);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["Memory"].UpgradeValueBy(1M);
        DynamicVars["Engrams"].UpgradeValueBy(1M);
    }
}