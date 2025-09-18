using _Project.Logic.Meta.Mobile;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Meta.Service.DeviceIdentifier
{
    public class DeviceIdentifier:IInitializable
    {
        private readonly MobileTools _mobileTools;

        public DeviceIdentifier(MobileTools mobileTools)
        {
            _mobileTools = mobileTools;
        }
        public void Initialize()
        {
            
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Object.Instantiate(_mobileTools);
            }
        }
        
        // Пример класса если хотите прям гибко, гибко))
    }
}
