using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Inventory
{
  public class Category
  {
    private int _id;
    private string _name;
    private int _ingredientid;

    public Category(string Name, int Ingredientid, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _ingredientid = Ingredientid;
    }

    public override bool Equals(System.Object otherCategory)
    {
        if (!(otherCategory is Category))
        {
          return false;
        }
        else
        {
          Category newCategory = (Category) otherCategory;
          bool idEquality = this.GetId() == newCategory.GetId();
          bool nameEquality = this.GetName() == newCategory.GetName();
          return (idEquality && nameEquality);
        }
    }

    public int GetId()
    {
      return _id;
    }

    public int GetItemId()
    {
      return _ingredientid;
    }

    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public static List<Category> GetAll()
    {
      List<Category> allCategories = new List<Category>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM categories;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        string categoryName = rdr.GetString(1);
        Category newCategory = new Category(categoryName, categoryId);
        allCategories.Add(newCategory);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allCategories;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO categories (name, ingredientid) OUTPUT INSERTED.id VALUES (@CategoryName, @IngredientId);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CategoryName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);

      SqlParameter nameParameter = new SqlParameter();
      ingredientidParameter.ParameterName = "@IngredientId";
      ingredientidParameter.Value = this.GetItemId();
      cmd.Parameters.Add(ingredientidParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM categories;", conn);
      cmd.ExecuteNonQuery();
    }

    // public static Category Find(string name)
    // {
    //   SqlConnection conn = DB.Connection();
    //   SqlDataReader rdr = null;
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT * FROM categories WHERE name = @CategoryName;", conn);
    //   SqlParameter nameParameter = new SqlParameter();
    //   nameParameter.ParameterName = "@CategoryName";
    //   nameParameter.Value = name;
    //   cmd.Parameters.Add(nameParameter);
    //   rdr = cmd.ExecuteReader();
    //
    //   int foundCategoryId = 0;
    //   string foundCategoryDescription = null;
    //
    //   while(rdr.Read())
    //   {
    //     foundCategoryId = rdr.GetInt32(0);
    //     foundCategoryDescription = rdr.GetString(1);
    //   }
    //   Category foundCategory = new Category(foundCategoryDescription, foundCategoryId);
    //
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    //   return foundCategory;
    // }

    public List<Item> GetItems()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients WHERE id = @ItemID;", conn);
      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@ItemID";
      idParameter.Value = this.GetId();
      cmd.Parameters.Add(idParameter);
      rdr = cmd.ExecuteReader();

      List<Item> Items  = new List<Item> {};
      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemCategory = rdr.GetString(1);
        string itemName = rdr.GetString(2);
        string itemDescription = rdr.GetString(3);
        string itemAmount = rdr.GetString(4);
        int itemPrice = rdr.GetInt32(5);

        Item newItem = new Item(itemId, itemCategory, itemName, itemDescription, itemAmount, itemPrice);
        Items.Add(newItem);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return Items;
    }
  }
}
