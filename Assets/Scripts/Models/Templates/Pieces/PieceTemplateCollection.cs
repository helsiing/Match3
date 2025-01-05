#region
using BrunoMikoski.ScriptableObjectCollections;
using UnityEngine;
#endregion

namespace VoodooMatch3.Models
{
    [CreateAssetMenu(menuName = "ScriptableObject Collection/Collections/Create PieceTemplateCollection",
        fileName = "PieceTemplateCollection", order = 0)]
    public class PieceTemplateCollection : ScriptableObjectCollection<PieceTemplate>
    {
    }
}