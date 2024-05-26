using System.Collections.Generic;

[System.Serializable]
public class GameBehaviorCollection
{
    List<GameBehavior> behaviors = new List<GameBehavior>();

    public void Add(GameBehavior behavior)
    {
        behaviors.Add(behavior);
    }

    public void GameUpdate()
    {
        for (int i = 0; i < behaviors.Count; i++)
        {
            // If the choosen enemy is dead
            if (!behaviors[i].GameUpdate())
            {
                // make it the last index of the enemy on the list 
                // then remove it
                int lastIndex = behaviors.Count - 1;
                behaviors[i] = behaviors[lastIndex];
                behaviors.RemoveAt(lastIndex);
                i -= 1;
            }
        }
    }
}