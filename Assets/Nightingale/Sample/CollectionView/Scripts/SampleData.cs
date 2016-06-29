using UnityEngine;
using System.Collections;

public enum TypeCategory {
	Weapon,
	Armor,
	Accessory,
	Potion,
}

public class SampleData {
	public int Id;
	public string Name;
	public int Rare;
	public TypeCategory Category;
}
