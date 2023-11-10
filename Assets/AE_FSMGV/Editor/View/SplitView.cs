using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace AE_FSMGV
{
    public class SplitView: TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
    }
}
