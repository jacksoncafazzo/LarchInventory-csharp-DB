using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Inventory
{
  public class Item
  {
    private int _id;
    private string _category;
    private string _name;
    private string _description;
    private string _amount;
    private int _price;

    public Item(string category, string name, string description, string amount, int price, int Id = 0)
    {
      _id = Id;
      _category = category;
      _name = name;
      _description = description;
      _amount = amount;
      _price = price;

    }
    public string GetCategory()
    {
      return _category;
    }
    public void SetCategory(string newCategory)
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

      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemCategory = rdr.GetString(1);
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

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO ingredients (category, name, description, amount, price) OUTPUT INSERTED.id VALUES (@ItemCategory, @ItemName, @ItemDescription, @ItemAmount, @ItemPrice)", conn);

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
    public static void DeleteItem(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM ingredients WHERE id = " + id + ";", conn);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM ingredients;", conn);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Item Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients WHERE id = @ItemId;", conn);
      SqlParameter itemIdParameter = new SqlParameter();
      itemIdParameter.ParameterName = "@ItemId";
      itemIdParameter.Value = id.ToString();
      cmd.Parameters.Add(itemIdParameter);
      rdr = cmd.ExecuteReader();

      int foundItemId = 0;
      string foundItemCategory = null;
      string foundItemName = null;
      string foundItemDescription = null;
      string foundItemAmount = null;
      int foundItemPrice = 0;
      while(rdr.Read())
      {
        foundItemId = rdr.GetInt32(0);
        foundItemCategory = rdr.GetString(1);
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
