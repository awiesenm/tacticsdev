using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//add to create menu
[CreateAssetMenu(fileName = "New Job", menuName = "Job")]
public class Job : ScriptableObject {

	//override old name definition with 'new'
	new public string name = "New Job";
    public string skillsetName;
	public string description;
	public Sprite icon = null;
    public bool rare;

    public List<ActiveSkill> activeSkills = new List<ActiveSkill>(5);

	//Called when the item is selected 
	public virtual void Assign(){

		//Use the item.
		//Overridden in children.

		Debug.Log("Job changed to " + name);
	}
}


