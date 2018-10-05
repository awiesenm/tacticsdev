using UnityEngine;

//add to create menu
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

	//override old name definition with 'new'
	new public string name = "New Item";
	public Sprite icon = null;

	//Called when the item is selected 
	public virtual void Use(){

		//Use the item.
		//Overridden in children.

		Debug.Log("Using " + name);
	}

	public void RemoveFromInventory(){
		Inventory.instance.Remove(this);
	}	
}
