using System;
using Managers;
using ScriptableObjects;
using UI;
using UnityEngine;

namespace Characters
{
    public class SpawnSystem : MonoBehaviour
    {
        [Header("Character Prefabs")]
        [SerializeField] private GameObject _roninPrefab;
        [SerializeField] private GameObject _shogunPrefab;
        [SerializeField] private GameObject _shinobiPrefab;

        [Header("Start GameMode Event")] 
        [SerializeField] private GameEvent _event;
        
        /// Spawns the Player and the Opponent CPU
        public void SpawnCharacters()
        {
            // Player character
            var prefab = SelectSamuraiPrefab(SaveManager.LoadPlayerCharacter());
            Character p1 = 
                InstantiateCharacter(prefab, PlayerNumber.One);

            // Opponent
            Character px = InstantiateCharacter(_roninPrefab, PlayerNumber.CPU);

            if (p1 is null || px is null) return;
            
            // Set as opponents
            p1.Opponent = px;
            px.Opponent = p1;
            
            // Characters are spawned and ready to go - start game mode
            _event.TriggerEvent();
        }

        /// Returns a GameObject prefab of the passed SamuraiType
        private GameObject SelectSamuraiPrefab(SamuraiType type)
        {
            switch (type)
            {
                case SamuraiType.Ronin: return _roninPrefab;
                case SamuraiType.Shogun: return _shogunPrefab;
                case SamuraiType.Shinobi: return _shinobiPrefab;
                case SamuraiType.Sensei:
                    break;
                case SamuraiType.Onna:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return null;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        /// Instantiates a Character in the scene from the GameManager's prefab list.
        private static Character InstantiateCharacter(GameObject prefab, 
            PlayerNumber playerNumber)
        {
            // Create instance
            Character instance = Instantiate(prefab).GetComponent<Character>();
            instance.Player = playerNumber;
            
            // Positioning, directions, and controls
            instance.SetPosition();
            instance.EndPosition = (playerNumber == PlayerNumber.One
                ? InitParams.Standoff_P1_EndPositionX
                : InitParams.Standoff_PX_EndPositionX);
            instance.SetDirection();
            instance.RegisterControls();

            return instance;
        }
    }
}