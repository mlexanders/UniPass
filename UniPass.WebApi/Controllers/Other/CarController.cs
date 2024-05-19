// using Microsoft.AspNetCore.Mvc;
// using UniPass.Infrastructure;
// using UniPass.Infrastructure.Repositories;
//
// namespace UniPass.WebApi.Controllers;
//
// [ApiController]
// [Route("/api/[controller]")]
// public class CarController : Controller, ICrud<Car, string>
// {
//     [HttpPost]
//     public async Task Create([FromBody] Car entity)
//     {
//     }
//
//     [HttpPost("many")]
//     public async Task Create([FromBody] Car[] entity)
//     {
//     }
//
//     [HttpGet]
//     public async Task<List<Car>> Read(Specification<Car> specification)
//     {
//         return new List<Car>();
//     }
//
//
//     [HttpPut]
//     public async Task Update([FromBody] Car entity)
//     {
//     }
//
//
//     [HttpPut("many")]
//     public async Task Update([FromBody] Car[] entities)
//     {
//     }
//
//     [HttpDelete("{key}")]
//     public async Task Delete(string key)
//     {
//     }
// }