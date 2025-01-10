using Managers;
using UnityEngine;
using VoodooMatch3.Services;

namespace Services
{
    public class GameBootstrapper : MonoBehaviour
    {
        protected void Awake()
        {
            ServiceLocator.Global.Register<IUiService>(new UiService());
            ServiceLocator.Global.Register<IScoreService>(new ScoreService());
        }
    }
}