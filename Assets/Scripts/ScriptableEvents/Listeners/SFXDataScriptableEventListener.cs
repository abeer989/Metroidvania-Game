using UnityEngine;

namespace ScriptableEvents.Listeners
{
    [AddComponentMenu(
        ScriptableEventConstants.MenuNameCustom + "/SFX Data Scriptable Event Listener",
        ScriptableEventConstants.MenuOrderCustom + 0
    )]
    public class SFXDataScriptableEventListener : BaseScriptableEventListener<SFXData>
    {
    }
}
