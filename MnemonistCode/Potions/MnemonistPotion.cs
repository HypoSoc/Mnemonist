using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Mnemonist.MnemonistCode.Character;
using Mnemonist.MnemonistCode.Extensions;

namespace Mnemonist.MnemonistCode.Potions;

[Pool(typeof(MnemonistPotionPool))]
public abstract class MnemonistPotion : CustomPotionModel
{
    public override string CustomPackedImagePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
    public override string CustomPackedOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".PotionImagePath();
}