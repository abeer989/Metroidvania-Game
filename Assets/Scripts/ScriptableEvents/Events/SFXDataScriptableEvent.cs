using UnityEngine;

namespace ScriptableEvents.Events
{
    [CreateAssetMenu(
        fileName = "SFXDataScriptableEvent",
        menuName = ScriptableEventConstants.MenuNameCustom + "/SFX Data Scriptable Event",
        order = ScriptableEventConstants.MenuOrderCustom + 0
    )]
    public class SFXDataScriptableEvent : BaseScriptableEvent<SFXData>
    {
    }
}
