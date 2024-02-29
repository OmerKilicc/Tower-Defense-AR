
namespace Enemy
{
	public interface IEnemy
	{
		public void TakeDamage(int damage);
		public void Die();

		public void FinishedPath();
		public void Move();


	}
}

