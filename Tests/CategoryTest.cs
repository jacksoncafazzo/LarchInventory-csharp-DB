using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Inventory
{
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=category_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_GetItems_FindsItemsInDatabase()
    {
      //Arrange
      Category newCategory = new Category("Couscous", 0);
      newCategory.Save();

      //Act
      List<Item> foundItems = Category.GetItems();

      //Assert
      Assert.Equal(testItem, foundItems);
    }
