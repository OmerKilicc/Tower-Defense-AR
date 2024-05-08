using System;
using UnityEngine.SceneManagement;
using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : ScriptableObject
{
    [SerializeField]
    GameTileContent _destinationPrefab;

    [SerializeField]
    GameTileContent _emptyPrefab;

	[SerializeField]
	GameTileContent _wallPrefab;

	Scene _contentScene;
    public void Reclaim(GameTileContent content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(content.gameObject);
    }


    // kendisine verilen prefab türünde contenti spawn eder kendini origin olarak verir
    // factory sahnesine taşır ve return eder
    GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = Instantiate(prefab);
        instance.OriginFactory = this;
		MoveToFactoryScene(instance.gameObject);
        return instance;
    }

    public GameTileContent Get(GameTileContentType type)
    {
        switch (type)
        {
            case GameTileContentType.Destination: return Get(_destinationPrefab);
            case GameTileContentType.Empty: return Get(_emptyPrefab);
            case GameTileContentType.Wall: return Get(_wallPrefab);
        }

        Debug.Assert(false, "Unsupported type: " + type);
        return null;
    }

	private void MoveToFactoryScene(GameObject o)
	{
        //Content sahnesi yuklenmediyse
        if(!_contentScene.isLoaded)
        {
            //editör modundaysak
            if(Application.isEditor)
            {
                // Objenin ismi neyse o isimde sahne getir ata
                _contentScene = SceneManager.GetSceneByName(name);
                if(!_contentScene.isLoaded )
                {
                    //editörde de yüklü değilse bu sefer yarat
                    _contentScene = SceneManager.CreateScene(name);
                }
            }
            else
            {
                // Normal moddaysak yarat
				_contentScene = SceneManager.CreateScene(name);
			}
        }

		//sahen yüklüysse bu objeyyi de content sahnesine taşı
		SceneManager.MoveGameObjectToScene(o, _contentScene);
		
	}
}
