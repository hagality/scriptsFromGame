using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XmlLoader : MonoBehaviour
{

    public ItemCollection itemCollection = null;
    public SpellCollection spellCollection = null;

    public static XmlLoader instance;

    void Awake()
    {
        instance = this;

        itemCollection = DeserializeItems();
        spellCollection = DeserializeSpells();
    }

    public TextAsset itemXml;
    public TextAsset spellsXml;

    ItemCollection DeserializeItems()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ItemCollection));
        using (System.IO.StringReader reader = new System.IO.StringReader(itemXml.text))
        {
            return serializer.Deserialize(reader) as ItemCollection;
        }
    }

    SpellCollection DeserializeSpells()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SpellCollection));
        using (System.IO.StringReader reader = new System.IO.StringReader(spellsXml.text))
        {
            return serializer.Deserialize(reader) as SpellCollection;
        }
    }

}





[Serializable()]
[System.Xml.Serialization.XmlRoot("ItemCollection")]
public class ItemCollection
{
    [XmlArray("GameItems")]
    [XmlArrayItem("Item", typeof(Item))]
    public Item[] Item { get; set; }



    public Item FindItem(int id)
    {
        foreach (Item item in Item)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }


    public Item FindItem(string itemName)
    {
        foreach (Item item in Item)
        {
            if (item.itemName == itemName)
            {
                return item;
            }
        }
        return null;
    }
}


[Serializable()]
[System.Xml.Serialization.XmlRoot("SpellCollection")]
public class SpellCollection
{
    [XmlArray("GameSpells")]
    [XmlArrayItem("Spell", typeof(Spell))]
    public Spell[] Spell { get; set; }



    public Spell FindSpell(int id)
    {
        foreach (Spell item in Spell)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }


    public Spell FindSpell(string itemName)
    {
        foreach (Spell item in Spell)
        {
            if (item.spellName == itemName)
            {
                return item;
            }
        }
        return null;
    }
}



