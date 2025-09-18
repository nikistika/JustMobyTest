using System;
using _Project.Logic.Gameplay.LoseControlling;
using _Project.Logic.Gameplay.PlayerLogic;
using UniRx;
using Zenject;

namespace _Project.Logic.Meta.UI.Lose
{
    public class LoseViewModel : IInitializable, IDisposable
    {
        private readonly ReactiveCommand _loseCommand = new ReactiveCommand();
        private readonly LoseView _view;
        private readonly SceneRestarter _sceneRestarter;
        private readonly Player _player;
        private readonly GameTimeController _gameTimeController;

        public LoseViewModel(LoseView view, SceneRestarter sceneRestarter, Player player,
            GameTimeController gameTimeController)
        {
            _view = view;
            _sceneRestarter = sceneRestarter;
            _player = player;
            _gameTimeController = gameTimeController;
        }

        public void Initialize()
        {
            _player.OnDead += Show;
            BindButtons();
        }

        public void Dispose()
        {
            _player.OnDead -= Show;
            _loseCommand.Dispose();
        }


        private void BindButtons()
        {
            _loseCommand.Subscribe(_ => RestartGame());
            _view.RestartButton.OnClickAsObservable()
                .Subscribe(_=> _loseCommand.Execute());
        }

        private void Show()
        {
            _view.Show();
            _gameTimeController.StopGame();
        }

        private void RestartGame()
        {
            _sceneRestarter.RestartScene();
        }
    }
}