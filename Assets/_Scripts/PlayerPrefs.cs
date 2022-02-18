using System;
using UnityEngine;

public class PlayerPrefs : MonoBehaviour
{
    public static PlayerPrefs instance;

    private void Awake()
    {
        instance = this;
    }

    public int GetWidth()
    {
        return UnityEngine.PlayerPrefs.GetInt("Width");
    }

    public bool UnlockParticle(int num)
    {
        try
        {
            UnityEngine.PlayerPrefs.SetInt(num.ToString(), 1);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool CheckParticleIsUnlock(int num)
    {
        if (UnityEngine.PlayerPrefs.GetInt(num.ToString()) != 0)
        {
            return true;
        }
        return false;
    }

    public float GetLeftPosition()
    {
        return UnityEngine.PlayerPrefs.GetFloat("LeftPos");
    }

    public float GetRightPosition()
    {
        return UnityEngine.PlayerPrefs.GetFloat("RightPos");
    }
    public int GetHeight()
    {
        return UnityEngine.PlayerPrefs.GetInt("Height");
    }

    public void UnlockTransaction(int num)
    {
        UnityEngine.PlayerPrefs.SetInt("TRN" + num, 1);
    }

    public void UnlockTheme(int num)
    {
        UnityEngine.PlayerPrefs.SetInt("THM" + num, 1);
    }


    public int ShortingNo(int num, bool isFavGet)
    {
        if (isFavGet)
        {
            return UnityEngine.PlayerPrefs.GetInt(num + "FavBundle");
        }
        return UnityEngine.PlayerPrefs.GetInt(num + "Fav");
    }

    public void StoreScreenSize()
    {
        UnityEngine.PlayerPrefs.SetInt("Width", Screen.width);
        UnityEngine.PlayerPrefs.SetInt("Height", Screen.height);
        UnityEngine.PlayerPrefs.SetInt("ResolutionStored", 1);
    }

    public int IsStoredResolution()
    {
        return UnityEngine.PlayerPrefs.GetInt("ResolutionStored");
    }

    public void SetToFavouriteTemplate(int number, bool isFavorite)
    {
        int num = UniqueNoForFavouriteTemplate() + 1;
        if (isFavorite)
        {
            UnityEngine.PlayerPrefs.SetInt(number + "FavBundle", num);
        }
        else
        {
            UnityEngine.PlayerPrefs.SetInt(number + "Fav", num);
        }
        ChangeFavouriteTemplateUniqueNo(num);
    }

    public void RemoveFromFavouriteTemplate(int number, bool isFavorite)
    {
        if (isFavorite)
        {
            UnityEngine.PlayerPrefs.SetInt(number + "FavBundle", -1);
        }
        else
        {
            UnityEngine.PlayerPrefs.SetInt(number + "Fav", -1);
        }
    }

    public void ChangeFavouriteTemplateUniqueNo(int num)
    {
        UnityEngine.PlayerPrefs.SetInt("UniqueNoFavouriteTemplate", num);
    }

    public bool IsFav(int num, bool shouldLoadPartileFromBundle)
    {
        if (shouldLoadPartileFromBundle)
        {
            if (UnityEngine.PlayerPrefs.GetInt(num + "FavBundle") == 0 || UnityEngine.PlayerPrefs.GetInt(num + "FavBundle") < 0)
            {
                return false;
            }
            return true;
        }
        if (UnityEngine.PlayerPrefs.GetInt(num + "Fav") == 0 || UnityEngine.PlayerPrefs.GetInt(num + "Fav") < 0)
        {
            return false;
        }
        return true;
    }

    public bool IsTransactionUnlocked(int num)
    {
        int @int = UnityEngine.PlayerPrefs.GetInt("TRN" + num);
        if (@int == 1)
        {
            return true;
        }
        return false;
    }

    public bool IsThemeUnlocked(int num)
    {
        int @int = UnityEngine.PlayerPrefs.GetInt("THM" + num);
        if (@int == 1)
        {
            return true;
        }
        return false;
    }

    public void StoreObjectPosition(float left, float right)
    {
        UnityEngine.PlayerPrefs.SetFloat("LeftPos", left);
        UnityEngine.PlayerPrefs.SetFloat("RightPos", right);
        UnityEngine.PlayerPrefs.SetInt("ObjectPosition", 1);
    }

    public int UniqueNoForFavouriteTemplate()
    {
        return UnityEngine.PlayerPrefs.GetInt("UniqueNoFavouriteTemplate");
    }

    public int isStoredObjectPosition()
    {
        return UnityEngine.PlayerPrefs.GetInt("ObjectPosition");
    }

    public bool CheckIsTransitionUnlock(int num)
    {
        if (UnityEngine.PlayerPrefs.GetInt("TRN" + num) != 0)
        {
            return true;
        }
        return false;
    }

    public bool CheckIsThemeUnlock(int num)
    {
        if (UnityEngine.PlayerPrefs.GetInt("THM" + num) != 0)
        {
            return true;
        }
        return false;
    }

}
