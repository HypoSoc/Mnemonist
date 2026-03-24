using BaseLib.Abstracts;
using BaseLib.Utils;
using Mnemonist.MnemonistCode.Character;

namespace Mnemonist.MnemonistCode.Potions;

[Pool(typeof(MnemonistPotionPool))]
public abstract class MnemonistPotion : CustomPotionModel;