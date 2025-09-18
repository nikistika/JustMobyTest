using DG.Tweening;
using UnityEngine;

namespace _Project.Logic.Gameplay.Enemy
{
    public class DeathView
    {
        private readonly float _deathTimer = 0.5f;

        public void InvokeDeathVisual(EnemyAbstract enemy)
        {
            enemy.gameObject.GetComponent<Collider>().enabled = false;
            enemy.transform.DOScale(Vector3.zero, _deathTimer).SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    enemy.gameObject.SetActive(false);
                });
        }
    }
}