using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Inventory
{
  public class ItemTest : IDisposable
  {
    public ItemTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventory_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Item.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      Item firstItem = new Item("solid", "Gouda", "George's Best Gouda from FM", "one pound", 6);
      Item secondItem = new Item("solid", "Gouda", "George's Best Gouda from FM", "one pound", 6);

      //Assert
      Assert.Equal(firstItem, secondItem);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Item testItem = new Item("liquid", "Heavy Cream", "organic", "one half-gallon", 8);

      //Act
      testItem.Save();
      List<Item> result = Item.GetAll();
      List<Item> testList = new List<Item>{testItem};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      Item testItem = new Item("seasoning", "Baking Soda", "Arm and Hammer", "one box", 5);

      //Act
      testItem.Save();
      Item savedItem = Item.GetAll()[0];

      int result = savedItem.GetId();
      int testId = testItem.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsItemInDatabase()
    {
      //Arrange
      Item testItem = new Item("seasoning", "Saffron", "best kind", "one gram", 20);
      testItem.Save();

      //Act
      Item foundItem = Item.Find(testItem.GetId());

      //Assert
      Assert.Equal(testItem, foundItem);
    }

    public void Dispose()
    {
      Item.DeleteAll();
    }
  }
}
