using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class SkillFetcher : MonoBehaviour
{
    public static List<SkillData> _skillDatas = new List<SkillData>();

    public static List<SkillData> _chosenSkills = new List<SkillData>();

    private Dictionary<string, SkillData> _skillDatasDictionary =
        new Dictionary<string, SkillData>();

    internal static List<SkillData> GetSkillDatas()
    {
        return _skillDatas;
    }

    private FirebaseFirestore _db;

    internal bool FetchingDone;

    // Start is called before the first frame update
    void Start()
    {
        _db = FirebaseFirestore.DefaultInstance;
        FetchSkills();
    }
    
    private async void FetchSkills()
    {
        if (_skillDatas.Count > 0)
        {
            _skillDatas.Clear();
        }

        FetchingDone = false;
        
        _chosenSkills = Enumerable.Repeat(new SkillData(), 4).ToList();

        await _db.Collection("Skills").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            var result = task.Result;

            foreach (var skillDataDoc in result.Documents)
            {
                var skillData = skillDataDoc.ConvertTo<SkillData>();
                
                _skillDatas.Add(skillData);
                _skillDatasDictionary.Add(skillData.SkillId, skillData);
            }
        });

        FetchingDone = true;
    }
}