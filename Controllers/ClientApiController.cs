using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace pengwin_learning_dapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientApiController : ControllerBase
    {
        private MySqlConnection _connection = new MySqlConnection("Server=localhost;Database=csharp_api_db;User=root;Password='';");

        [HttpGet("GetClients")]
        public async Task<IActionResult> GetClients(){

            const string query = "SELECT * FROM Clients ORDER BY id DESC";
            var result  = await _connection.QueryAsync<Client>(query);
            
            if(result.Count() == 0)
                return BadRequest("alskjdfalskjf");

            return Ok(result);
        }

        // for the update function
        [HttpGet("GetClient")]
        public async Task<ActionResult<Client>> GetClients(int id){

            const string query = "SELECT * FROM Clients WHERE Id = @id LIMIT 1";
            var result  = await _connection.QueryFirstAsync<Client>(query, new {id = id});
            
            if(result == null)
                return BadRequest("alskjdfalskjf");

            return Ok(result);
        }

        [HttpPost("SaveClient")]
        public async Task<IActionResult> SaveClientAsync(Client client){
            
            const string query = "INSERT INTO Clients (ClientName, Residency) VALUES (@ClientName, @Residency); SELECT * FROM Clients ORDER BY Id DESC LIMIT 1";
            
            var result  = await _connection.QueryAsync<Client>(query, client);

            return Ok(result);
        }

        [HttpPut("UpdateClient")]
        public async Task<IActionResult> UpdateClientAsync(int Id, Client client){
            
            const string query = "UPDATE Clients SET ClientName = @Cn, Residency = @R WHERE Id = @Id; SELECT * FROM Clients WHERE Id = @Id LIMIT 1";
            
            var result  = await _connection.QueryAsync<Client>(query, new {
                Id = Id,
                Cn = client.ClientName,
                R = client.Residency
            });

            return Ok(result);
        }

        [HttpDelete("DeleteClient")]
        public async Task<IActionResult> DeleteClient(int Id){
            
            const string query = "DELETE FROM Clients WHERE Id = @Id";
            
            await _connection.QueryAsync<Client>(query, new { Id = Id,});

            return Ok();
        }
    }
}