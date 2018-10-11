using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillset
{
    public Job job;
    public List<Skill> skills = new List<Skill>();

    public Skillset(Job job, List<Skill> unlockedSkills)
    {
        this.job = job;
        foreach (Skill skill in unlockedSkills)
        {
            if (skill.job == job)
            {
                skills.Add(skill);
            }
        }
    }

    // Add skill to characer's skillset after skill unlock
    public void AddSKill(Skill skill) //decide to keep or use instance.skill.Add();
    {
        skills.Add(skill);
    }

    public void RemoveSkill(Skill skill) //decide to keep or use instance.skill.Add();
    {
        skills.Remove(skill);
    }
}