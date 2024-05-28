using UnityEngine;
using UnityEngine.SceneManagement;

/* Base abstract class for all factory types */
// Factories used to spawn multiple objects for its own scene
public abstract class GameObjectFactory : ScriptableObject
{
	Scene scene;

	// Generic method to create gameobject with given type of prefab
	protected T CreateGameObjectInstance<T>(T prefab) where T : MonoBehaviour
	{
		// if scene is not loaded Create one, if in editor get its name
		if (!scene.isLoaded)
		{
			if (Application.isEditor)
			{
				scene = SceneManager.GetSceneByName(name);

				if (!scene.isLoaded)
				{
					scene = SceneManager.CreateScene(name);
				}
			}

			else
			{
				scene = SceneManager.CreateScene(name);
			}
		}

		// Create an instance of prefab and move it to the scene that created
		// return the created object

		T instance = Instantiate(prefab);
		//SceneManager.MoveGameObjectToScene(instance.gameObject, scene);
		return instance;
	}
}
