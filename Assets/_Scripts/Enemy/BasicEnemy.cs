using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
	public class BasicEnemy : MonoBehaviour , IEnemy
	{
		[SerializeField]
		EnemyDataSO _enemyData;

		[SerializeField]
		PlayerDataSO _playerData;

		public static event Action<int> OnMoneyChanged;

		[SerializeField] List<Waypoint> path = new List<Waypoint>();
		[SerializeField][Range(0f, 5f)] float speed = 1f;



		void OnEnable()
		{
			Bullet.OnEnemyDamaged += TakeDamage;
		}

		private void OnDisable()
		{
			Bullet.OnEnemyDamaged -= TakeDamage;
		}
		private void Start()
		{
			Move();
		}
		private void FindPathBruteForce()
		{
			path.Clear();

			GameObject parent = GameObject.FindGameObjectWithTag("Path");

			foreach (Transform child in parent.transform)
			{
				path.Add(child.GetComponent<Waypoint>());
			}
		}

		private void ReturnToStart()
		{
			transform.position = path[0].transform.position;
		}

		IEnumerator FollowPath()
		{
			//iterating on all the waypoint
			foreach (var waypoint in path)
			{

				Vector3 startPosition = transform.position;
				Vector3 endPosition = waypoint.transform.position;
				float distanceCovered = 0f;

				//making it look at the direction which its moving
				transform.LookAt(endPosition);

				while (distanceCovered < 1f)
				{
					distanceCovered += Time.deltaTime * speed;
					transform.position = Vector3.Lerp(startPosition, endPosition, distanceCovered);
					yield return new WaitForEndOfFrameUnit();
				}
			}

			//When path is finished
			FinishedPath();
		}

		public void TakeDamage(int damage)
		{
			_enemyData.CurrentHealth -= damage;

			if(_enemyData.CurrentHealth <= 0)
			{
				Die();
			}
		}


		public void Move()
		{
			FindPathBruteForce();
			ReturnToStart();
			StartCoroutine(FollowPath());
		}

		public void FinishedPath()
		{
			//decreese health, money from the player. Invoke the money for UI
			_playerData.CurrentHealth -= _enemyData.Damage;
			_playerData.CurrentMoney -= _enemyData.Penalty;
			OnMoneyChanged.Invoke(_playerData.CurrentMoney);
			Die();

		}

		public void Die()
		{
			//TODO: invoke an event to alert the pooler to take this object to unactive state
			_enemyData.CurrentHealth = 0;
			gameObject.SetActive(false);
			ParticleManager.Instance.SpawnParticleAtLocation(transform.position, ParticleManager.Particles.Explosion);
			SoundManager.Instance.PlayOneShot(SoundManager.Sounds.EnemyDeath);
		}
	}

}
