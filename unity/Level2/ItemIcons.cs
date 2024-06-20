using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu]
public class ItemIcons : ScriptableObject
{
    public List<Icon> objects;
}



[Serializable]
public class Icon
{
    public Item item;
    public Sprite image;

}