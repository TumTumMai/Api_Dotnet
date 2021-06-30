using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dtest.Controllers
{

    [Route("api/[controller]")]
    public class DController : ControllerBase
    {
        public DController(AppDb db)
        {
            Db = db;
        }

        // GET api/D
        [DisableCors]
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new DPostQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }

        // GET api/D/ID
        [DisableCors]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new DPostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/D
        [DisableCors]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]DPost body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/D/ID
        [DisableCors]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] DPost body)
        {
            await Db.Connection.OpenAsync();
            var query = new DPostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.Date = body.Date;
            result.Discipline = body.Discipline;
            result.Project = body.Project;
            result.Status = body.Status;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/D/ID
        [DisableCors]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new DPostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        // DELETE api/D
        //[HttpDelete]
        //public async Task<IActionResult> DeleteAll()
        //{
        //    await Db.Connection.OpenAsync();
        //    var query = new DPostQuery(Db);
        //    await query.DeleteAllAsync();
        //    return new OkResult();
        //}

        public AppDb Db { get; }
    }
}