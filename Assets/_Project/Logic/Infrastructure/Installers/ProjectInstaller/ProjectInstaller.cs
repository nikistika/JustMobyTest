using _Project.Logic.Gameplay.Service.TimeForInteract;
using _Project.Logic.Meta.Service.RandomServiceWrap;
using Zenject;

namespace _Project.Logic.Infrastructure.Installers.ProjectInstaller
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindingMetaService();
        }

        private void BindingMetaService()
        {
            Container.BindInterfacesAndSelfTo<TimeService>().AsCached().NonLazy();
            Container.BindInterfacesAndSelfTo<RandomService>().AsCached().NonLazy();
        }
    }
}