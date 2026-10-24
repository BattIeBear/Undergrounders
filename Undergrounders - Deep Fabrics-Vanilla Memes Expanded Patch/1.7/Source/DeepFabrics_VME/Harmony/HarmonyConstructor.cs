using HarmonyLib;
using System.Reflection;
using Verse;

namespace Undergrounders_DeepFabrics_VanillaMemesExpanded
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.Undergrounders_DeepFabrics_VanillaMemesExpanded");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
