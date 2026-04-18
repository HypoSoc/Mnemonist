using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Mnemonist.MnemonistCode.Nodes;

public partial class MnemonistSelectionReticle : NSelectionReticle
{
    public override void _Ready()
    {
        Log.Info("Initializing rest site reticle");
        base._Ready();
    }
}