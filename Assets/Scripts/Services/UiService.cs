using System;
using VoodooMatch3.Models;

namespace Managers
{
    public interface IUiService
    {
        public Action<LevelTemplate> LoadLevel { get; set; } 
        public Action LoadLevelList { get; set; }
    }
    
    public class UiService : IUiService
    {
        public Action<LevelTemplate> LoadLevel{ get; set; } 
        public Action LoadLevelList { get; set; }
    }
}