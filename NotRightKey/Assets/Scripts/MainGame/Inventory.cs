using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class Inventory
{
    private List<Item> items = new List<Item>();

    public Inventory() { }

    public Inventory(string csv)
    {
        string[] ar = csv.Split('=', StringSplitOptions.RemoveEmptyEntries);
        items.Clear();
        for (int i = 0; i < ar.Length; i++)
        {
            items.Add(new Item(ar[i]));
        }
    }

    public void AddItem(int id, int cnt)
    {
        bool isNew = true;
        foreach (Item item in items)
        {
            if (item.Id == id)
            {
                item.Add(cnt);
                isNew = false;
                return;
            }
        }
        if (isNew)
        {
            items.Add(new Item(id, "", cnt));
        }
    }

    public bool DecItem(int id, int cnt)
    {
        foreach (Item item in items)
        {
            if (item.Id == id)
            {
                return item.Dec(cnt);
            }
        }
        return false;
    }

    public int CountItemByID(int id)
    {
        foreach(Item item in items)
        {
            if (item.Id == id) return item.Count;
        }
        return 0;
    }

    public string ToCsvString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Item item in items)
        {
            sb.Append($"{item.ToCsvString()}{'='}");
        }
        return sb.ToString();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder($"Count items = {items.Count}");
        foreach (Item item in items)
        {
            sb.Append(item.ToString());
        }
        return sb.ToString();
    }
}

[Serializable]
public class Item
{
    private int id;
    private string name;
    private int count;

    public Item() { }

    public Item(int id, string name, int count)
    {
        this.id = id;
        this.name = name;
        this.count = count;
    }

    public Item(string csv)
    {
        string[] ar = csv.Split('#', StringSplitOptions.RemoveEmptyEntries);
        if (ar.Length >= 2)
        {
            if (int.TryParse(ar[0], out int zn)) id = zn;
            if (int.TryParse(ar[1], out int cnt)) count = cnt;
        }
    }

    public int Id { get { return id; } }
    public int Count { get { return count; } }
    
    public void Add(int cnt)
    {
        count += cnt;
    }

    public bool Dec(int cnt)
    {
        if (cnt >= count)
        {
            count -= cnt;
            return true;
        }
        return false;
    }

    public string ToCsvString(char sep = '#')
    {
        return $"{id}{sep}{count}{sep}";
    }

    public override string ToString()
    {
        return $" item {name}<{id}> = {count} ";
    }
}
