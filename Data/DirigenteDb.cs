using Microsoft.Data.SqlClient;
using System.Data;
using TrabajoProyecto.Models;

namespace TrabajoProyecto.Data
{
    public class DirigenteDb
    {
        private readonly string _connectionString;

        public DirigenteDb(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private async Task<SqlConnection> GetConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<List<Dirigente>> GetAllAsync()
        {
            var list = new List<Dirigente>();
            try
            {
                await using var conn = await GetConnectionAsync();
                await using var cmd = new SqlCommand("SELECT * FROM Dirigentes", conn);
                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new Dirigente
                    {
                        DirigenteId = reader.GetInt32(reader.GetOrdinal("DirigenteId")),
                        ClubId = reader.GetInt32(reader.GetOrdinal("ClubId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                        Rol = reader.GetString(reader.GetOrdinal("Rol")),
                        FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                        Dni = reader.GetInt32(reader.GetOrdinal("Dni"))
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

        public async Task<Dirigente?> GetByIdAsync(int id)
        {
            Dirigente? dirigente = null;
            try
            {
                await using var conn = await GetConnectionAsync();
                await using var cmd = new SqlCommand("SELECT * FROM Dirigentes WHERE DirigenteId=@id", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    dirigente = new Dirigente
                    {
                        DirigenteId = reader.GetInt32(reader.GetOrdinal("DirigenteId")),
                        ClubId = reader.GetInt32(reader.GetOrdinal("ClubId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                        Rol = reader.GetString(reader.GetOrdinal("Rol")),
                        FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                        Dni = reader.GetInt32(reader.GetOrdinal("Dni"))
                    };
                }
            }
            catch (InvalidCastException ex)
            {
                // Add a breakpoint here to find out exactly which column is the problem
                Console.WriteLine($"Error de conversión de tipo en GetByIdAsync: {ex.Message}");
                throw;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error de base de datos en GetByIdAsync: {ex.Message}");
                throw;
            }
            return dirigente;
        }

        public async Task AddAsync(Dirigente dirigente)
        {
            try
            {
                await using var conn = await GetConnectionAsync();
                await using var cmd = new SqlCommand(
                    @"INSERT INTO Dirigentes (ClubId, Nombre, Apellido, FechaNacimiento, Rol, Dni) 
                      VALUES (@ClubId, @Nombre, @Apellido, @FechaNacimiento, @Rol, @Dni)", conn);

                cmd.Parameters.Add("@ClubId", System.Data.SqlDbType.Int).Value = dirigente.ClubId;
                cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar, 80).Value = dirigente.Nombre;
                cmd.Parameters.Add("@Apellido", System.Data.SqlDbType.NVarChar, 80).Value = dirigente.Apellido;
                cmd.Parameters.Add("@FechaNacimiento", System.Data.SqlDbType.DateTime).Value = dirigente.FechaNacimiento;
                cmd.Parameters.Add("@Rol", System.Data.SqlDbType.NVarChar, 80).Value = dirigente.Rol;
                cmd.Parameters.Add("@Dni", System.Data.SqlDbType.Int).Value = dirigente.Dni;

                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error de base de datos en AddAsync: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAsync(int id, Dirigente dirigente)
        {
            try
            {
                await using var conn = await GetConnectionAsync();
                await using var cmd = new SqlCommand(
                    @"UPDATE Dirigentes SET ClubId=@ClubId, Nombre=@Nombre, Apellido=@Apellido, 
                      FechaNacimiento=@FechaNacimiento, Rol=@Rol, Dni=@Dni WHERE DirigenteId=@id", conn);

                cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@ClubId", System.Data.SqlDbType.Int).Value = dirigente.ClubId;
                cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar, 80).Value = dirigente.Nombre;
                cmd.Parameters.Add("@Apellido", System.Data.SqlDbType.NVarChar, 80).Value = dirigente.Apellido;
                cmd.Parameters.Add("@FechaNacimiento", System.Data.SqlDbType.DateTime).Value = dirigente.FechaNacimiento;
                cmd.Parameters.Add("@Rol", System.Data.SqlDbType.NVarChar, 80).Value = dirigente.Rol;
                cmd.Parameters.Add("@Dni", System.Data.SqlDbType.Int).Value = dirigente.Dni;

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
                await using var cmd = new SqlCommand("DELETE FROM Dirigentes WHERE DirigenteId=@id", conn);
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