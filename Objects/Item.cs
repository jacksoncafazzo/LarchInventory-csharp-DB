using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Inventory
{
  public class Item
  {
    private int _id;
    private int _category;
    private string _name;
    private string _description;
    private string _amount;
    private int _price;

    public Item(int category, string name, string Description, string amount, int price, int Id = 0)
    {
      _id = Id;
      _category = category;
      _name = name;
      _description = Description;
      _amount = amount;
      _price = price;

    }
    public int GetCategory()
    {
      return _category;
    }
    public void SetCategory(int newCategory)
    {
      _category = newCategory;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public string GetAmount()
    {
      return _amount;
    }
    public void SetAmount(string newAmount)
    {
      _amount = newAmount;
    }
    public int GetPrice()
    {
      return _price;
    }
    public void SetPrice(int newPrice)
    {
      _price = newPrice;
    }
    public int GetId()
    {
      return _id;
    }

    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM items;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        int itemCategory = rdr.GetInt32(1);
        string itemName = rdr.GetString(2);
        string itemDescription = rdr.GetString(3);
        string itemAmount = rdr.GetString(4);
        int itemPrice = rdr.GetInt32(5);
        Item newItem = new Item(itemCategory, itemName, itemDescription, itemAmount, itemPrice, itemId);
        allItems.Add(newItem);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allItems;
    }

    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is Item))
      {
        return false;
      }
      else
      {
        Item newItem = (Item) otherItem;
        bool idEquality = (this.GetId() == newItem.GetId());
        bool descriptionEquality = (this.GetDescription() == newItem.GetDescription());
        return (idEquality && descriptionEquality);

      }
    }


    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO items (category, name, description, amount, price) OUTPUT INSERTED.id VALUES (@ItemCategory, @ItemName, @ItemDescription, @ItemAmount, @ItemPrice)", conn);

      SqlParameter categoryParameter = new SqlParameter();
      categoryParameter.ParameterName = "@ItemCategory";
      categoryParameter.Value = this.GetCategory();
      cmd.Parameters.Add(categoryParameter);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@ItemName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@ItemDescription";
      descriptionParameter.Value = this.GetDescription();
      cmd.Parameters.Add(descriptionParameter);

      SqlParameter amountParameter = new SqlParameter();
      amountParameter.ParameterName = "@ItemAmount";
      amountParameter.Value = this.GetAmount();
      cmd.Parameters.Add(amountParameter);

      SqlParameter priceParameter = new SqlParameter();
      priceParameter.ParameterName = "@ItemPrice";
      priceParameter.Value = this.GetPrice();
      cmd.Parameters.Add(priceParameter);


      rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM items;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Item Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM items WHERE id = @ItemId;", conn);
      SqlParameter itemIdParameter = new SqlParameter();
      itemIdParameter.ParameterName = "@ItemId";
      itemIdParameter.Value = id.ToString();
      cmd.Parameters.Add(itemIdParameter);
      rdr = cmd.ExecuteReader();

      int foundItemId = 0;
      int foundItemCategory = 0;
      string foundItemName = null;
      string foundItemDescription = null;
      string foundItemAmount = null;
      int foundItemPrice = 0;
      while(rdr.Read())
      {
        foundItemId = rdr.GetInt32(0);
        foundItemCategory = rdr.GetInt32(1);
        foundItemName = rdr.GetString(2);
        foundItemDescription = rdr.GetString(3);
        foundItemAmount = rdr.GetString(4);
        foundItemPrice = rdr.GetInt32(5);
      }
      Item foundItem = new Item(foundItemCategory, foundItemName, foundItemDescription, foundItemAmount, foundItemPrice, foundItemId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundItem;
    }
  }
}
