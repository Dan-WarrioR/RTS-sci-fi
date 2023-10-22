using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class PlanetData : ScriptableObject
{
	[field: SerializeField] public string PlanetName { get; private set; }
	[field: SerializeField] public Sprite PlanetTerrainImage { get; private set; }
	[field: SerializeField] public string PlanetDescription { get; private set; }    
}
