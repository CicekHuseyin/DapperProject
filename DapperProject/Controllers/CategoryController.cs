using Dapper;
using DapperProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly string _connection = "Server=DESKTOP-KR485FT\\SQLEXPRESS; initial catalog=DbNews; integrated security=true";

        public async Task<IActionResult> Index()
        {
            await using var connection = new SqlConnection(_connection);
            var values = await connection.QueryAsync<ResultCategoryViewModel>("Select * from Categories");
            //Yukarıdaki kodda sorgudan gelen değerler ile view model deki propertyler otomatik maplenmiş oluyor.
            //QueryAsync Sql komutu yazmamızı sağlar.
            return View(values);
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(ResultCategoryViewModel model)
        {
            await using var connection = new SqlConnection(_connection);
            var query = $"Insert Into Categories (CategoryName,CategoryStatus) Values ('{model.CategoryName}','True')";
            await connection.ExecuteAsync(query);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await using var connection = new SqlConnection(_connection);
            var query = $"Delete From Categories Where CategoryID='{id}'";
            await connection.ExecuteAsync(query);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            await using var connection = new SqlConnection(_connection);
            var values = await connection.QueryFirstAsync<ResultCategoryViewModel>($"Select * from Categories Where CategoryID='{id}'");
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(ResultCategoryViewModel model)
        {
            await using var connection = new SqlConnection(_connection);
            var query = $"update Categories set CategoryName='{model.CategoryName}',CategoryStatus='{model.CategoryStatus}' where CategoryID='{model.CategoryID}'";
            await connection.ExecuteAsync(query);
            return RedirectToAction("Index");
        }
    }
}
