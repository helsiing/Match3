using System;
using VoodooMatch3.Models;
using VoodooMatch3.Utils;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public Action<LevelTemplate> LoadLevel;
    }
}