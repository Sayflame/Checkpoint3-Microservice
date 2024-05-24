using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Data;
using web_api_restaurante.Entidades;

namespace web_api_restaurante.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly string? _connectionString;

        public UsuarioController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        private IDbConnection OpenConnection()
        {
            IDbConnection dbConnection = new SqliteConnection(_connectionString);
            dbConnection.Open();
            return dbConnection;
        }
        //GetAll
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using IDbConnection dbConnection = OpenConnection();
            var result = await dbConnection.QueryAsync<Usuario>("select id, nome, senha from Usuario;");
            return Ok(result);
        }
        //GetbyId
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            using IDbConnection dbConnection = OpenConnection();
            string sql = "select id , nome , senha from Usuario;";
            var Usuario = await dbConnection.QueryFirstOrDefaultAsync<Usuario>(sql, new { id });
            if (Usuario == null)
            {
                return NotFound();
            }
            return Ok(Usuario);
        }
        //Add
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            using IDbConnection dbConnection = OpenConnection();

            dbConnection.Execute("insert into Usuario(nome,senha) values(@Nome, @Senha)", usuario);
            return Ok();
        }
        //UpdateUsuario
        [HttpPut]
        public IActionResult Put([FromBody] Usuario usuario)
        {

            using IDbConnection dbConnection = OpenConnection();

            // Atualiza o Usuario
            var query = @"UPDATE Usuario SET 
                          Nome = @Nome,
                          Senha = @Senha,
                          WHERE Id = @Id";

            dbConnection.Execute(query, usuario);

            return Ok();
        }
        //DeleteUsuario
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using IDbConnection dbConnection = OpenConnection();

            var Usuario = await dbConnection.QueryAsync<Usuario>("delete from Usuario where id = @id;", new { id });
            return Ok();
        }
    }
}