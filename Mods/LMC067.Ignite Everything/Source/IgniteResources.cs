using UnityEngine;
using Verse;

namespace IgniteEverything
{
    [StaticConstructorOnStartup]
    public static class IgniteResources
    {
        public static readonly Texture2D IgniteIcon = ContentFinder<Texture2D>.Get("Things/Building/Misc/TorchLamp_MenuIcon");

        static IgniteResources()
        {

        }
    }
}
