using UnityEngine;

public class Collectable : Piece
{
	[field: SerializeField] public bool IsCollectableBelow { get; private set; }
	[field: SerializeField] public bool IsCollectableFromExplode { get; private set; }
}