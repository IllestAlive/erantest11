using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : Instancable<SkillManager>
{
    public List<Sprite> skillImagesCircular = new List<Sprite>();
    public List<GameObject> shines = new List<GameObject>();
}
