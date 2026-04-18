using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;

namespace Mnemonist.MnemonistCode.Nodes;

[GlobalClass]
public partial class SNMerchantChracter : NMerchantCharacter
{
    public override void _Ready()
    {
        Log.Info("Initializing creature visuals");
        base._Ready();
    }

}