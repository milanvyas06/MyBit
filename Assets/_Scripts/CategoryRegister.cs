using System;
using UnityEngine;

[Serializable]
public class CategoryRegister : MonoBehaviour
{
    [Serializable]
    public class JSonCategoryInformation
    {
        public GameObject ParticleButtonParent;

        public GameObject ParticleButton;
    }

    public bool isGeneratedFromJson;

    public string catName;

    public CategoryList _CatList;

    public GameObject GeneratedcategoryButton;

    public GameObject CategoryContainer;

    public JSonCategoryInformation _JsonCategoryInformation;
}
