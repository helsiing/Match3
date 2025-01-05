#region
using BrunoMikoski.ScriptableObjectCollections;
using UnityEngine;
#endregion

namespace VoodooMatch3.Models
{
    [CreateAssetMenu(menuName = "ScriptableObject Collection/Collections/Create LevelTemplateCollection",
        fileName = "LevelTemplateCollection", order = 0)]
    public class LevelTemplateCollection : ScriptableObjectCollection<LevelTemplate>
    {
    }
}