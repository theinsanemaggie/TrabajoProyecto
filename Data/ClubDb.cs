using Microsoft.Data.SqlClient;
using TrabajoProyecto.Models;

namespace TrabajoProyecto.Data
{
    public class ClubDb
    {
        private readonly string _connectionString;

        public ClubDb(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private async Task<SqlConnection> GetConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<List<Club>> GetAllAsync()
        {
            var list = new List<Club>();
            try
            {
                await using var conn = await GetConnectionAsync();
                await using var cmd = new SqlCommand("SELECT * FROM Clubes", conn);
                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new Club
                    {
                        ClubId = reader.GetInt32(reader.GetOrdinal("ClubId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre"))
                    });
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error de base de datos en GetAllAsync: {ex.Message}");
                throw;
            }
            return list;
        }

        public async Task<Club?> GetByIdAsync(int id)
        {
            Club? club = null;
            try
            {
                await using var conn = await GetConnectionAsync();
                await using var cmd = new SqlCommand("SELECT * FROM Clubes WHERE ClubId=@id", conn);
                cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    club = new Club
                    {
                        ClubId = reader.GetInt32(reader.GetOrdinal("ClubId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre"))
                    };
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error de base de datos en GetByIdAsync: {ex.Message}");
                throw;
            }
            return club;
        }

        public async Task AddAsync(Club club)
        {
            try
            {
                await using var conn = await GetConnectionAsync();
                await using var cmd = new SqlCommand("INSERT INTO Clubes (Nombre) VALUES (@Nombre)", conn);
                cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar, 80).Value = club.Nombre;

                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error de base de datos en AddAsync: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAsync(int id, Club club)
        {
            try
            {
                await using var conn = await GetConnectionAsync();
                await using var cmd = new SqlCommand("UPDATE Clubes SET Nombre=@Nombre WHERE ClubId=@id", conn);
                cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar, 80).Value = club.Nombre;
                cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;

                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error de base de datos en UpdateAsync: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await using var conn = await GetConnectionAsync();
                await using var cmd = new SqlCommand("DELETE FROM Clubes WHERE ClubId=@id", conn);
                cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;

                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error de base de datos en DeleteAsync: {ex.Message}");
                throw;
            }
        }
    }
}