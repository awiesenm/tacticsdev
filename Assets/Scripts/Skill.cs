using UnityEngine;

//add to create menu
[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Skill")]
public class Skill : ScriptableObject {

	//override old name definition with 'new'
	new public string name = "New Skill";
	public string description;
	public Sprite icon = null;

    public Job job; //revisit
	public int jpCost = 0;
	// node position?

	//Called when the item is selected 
	public virtual void Use(){

		//Use the item.
		//Overridden in children.

		Debug.Log("Using " + name);
	}
}

// public enum Job { Knight, Archer, WhiteMage, BlackMage } //revisit

