using BaseLib.Abstracts;
using Mnemonist.MnemonistCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using Mnemonist.MnemonistCode.Cards.Basic;
using Mnemonist.MnemonistCode.Relics;

namespace Mnemonist.MnemonistCode.Character;

public class Mnemonist : PlaceholderCharacterModel
{
    public const string CharacterId = "Mnemonist";

    public static readonly Color Color = new("ffffff");
    
    public override CustomEnergyCounter? CustomEnergyCounter =>
        new CustomEnergyCounter((i) => "res://Mnemonist/images/charui/mnemonist_energy_icon.png", new Color(0.4f, 0.1f, 0.9f), new Color(0.7f, 0.1f, 0.9f));
    
    public override string CustomVisualPath => "res://Mnemonist/scenes/combat/creature_visuals/mnemonist.tscn";

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 10;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<MnemonistStrike>(),
        ModelDb.Card<MnemonistStrike>(),
        ModelDb.Card<MnemonistStrike>(),
        ModelDb.Card<MnemonistStrike>(),
        ModelDb.Card<MnemonistDefend>(),
        ModelDb.Card<MnemonistDefend>(),
        ModelDb.Card<MnemonistDefend>(),
        ModelDb.Card<MnemonistDefend>(),
        ModelDb.Card<NaggingThought>(),
        ModelDb.Card<ActiveRecall>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<IdyllicMemories>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<MnemonistCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<MnemonistRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<MnemonistPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
    
    public override CreatureAnimator? SetupCustomAnimationStates(MegaSprite controller)
    {
        return SetupAnimationState(controller, "idle_loop");
    }
}