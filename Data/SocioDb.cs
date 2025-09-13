using Microsoft.Data.SqlClient;
using TrabajoProyecto.Models;

namespace TrabajoProyecto.Data
{
    public class SocioDb
    {
        private readonly string _connectionString;

        public SocioDb(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private async Task<SqlConnection> GetConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<List<Socio>> GetAllAsync()
        {
            var list = new List<Socio>();
            try
            {
                await using var conn = await GetConnectionAsync();
                await using var cmd = new SqlCommand("SELECT * FROM Socios", conn);
                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new Socio
                    {
                        SocioId = reader.GetInt32(reader.GetOrdinal("SocioId")),
                        ClubId = reader.GetInt32(reader.GetOrdinal("ClubId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                        FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                        FechaAsociado = reader.GetDateTime(reader.GetOrdinal("FechaAsociado")),
                        Dni = reader.GetInt32(reader.GetOrdinal("Dni")),
                        CantidadAsistencias = reader.GetInt32(reader.GetOrdinal("CantidadAsistencias"))
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

        public async Task<Socio?> GetByIdAsync(int id)
        {
            Socio? s = null;
            try
            {
                await using var conn = await GetConnectionAsync();
                await using var cmd = new SqlCommand("SELECT * FROM Socios WHERE SocioId=@id", conn);
                cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    s = new Socio
                    {
                        SocioId = reader.GetInt32(reader.GetOrdinal("SocioId")),
                        ClubId = reader.GetInt32(reader.GetOrdinal("ClubId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                        FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                        FechaAsociado = reader.GetDateTime(reader.GetOrdinal("FechaAsociado")),
                        Dni = reader.GetInt32(reader.GetOrdinal("Dni")),
                        CantidadAsistencias = reader.GetInt32(reader.GetOrdinal("CantidadAsistencias"))
                    };
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error de base de datos en GetByIdAsync: {ex.Message}");
                throw;
            }
            return s;
        }

        public async Task AddAsync(Socio socio)
        {
            try
            {
                await using var conn = await GetConnectionAsync();
                await using var cmd = new SqlCommand(
                    @"INSERT INTO Socios (ClubId, Nombre, Apellido, FechaNacimiento, FechaAsociado, Dni, CantidadAsistencias) 
                      VALUES (@ClubId, @Nombre, @Apellido, @FechaNacimiento, @FechaAsociado, @Dni, @CantidadAsistencias)", conn);

                cmd.Parameters.Add("@ClubId", System.Data.SqlDbType.Int).Value = socio.ClubId;
                cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar, 80).Value = socio.Nombre;
                cmd.Parameters.Add("@Apellido", System.Data.SqlDbType.NVarChar, 80).Value = socio.Apellido;
                cmd.Parameters.Add("@FechaNacimiento", System.Data.SqlDbType.DateTime).Value = socio.FechaNacimiento;
                cmd.Parameters.Add("@FechaAsociado", System.Data.SqlDbType.DateTime).Value = socio.FechaAsociado;
                cmd.Parameters.Add("@Dni", System.Data.SqlDbType.Int).Value = socio.Dni;
                cmd.Parameters.Add("@CantidadAsistencias", System.Data.SqlDbType.Int).Value = socio.CantidadAsistencias;

                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error de base de datos en AddAsync: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAsync(int id, Socio socio)
        {
            try
            {
                await using var conn = await GetConnectionAsync();
                await using var cmd = new SqlCommand(
                    @"UPDATE Socios SET ClubId=@ClubId, Nombre=@Nombre, Apellido=@Apellido, 
                      FechaNacimiento=@FechaNacimiento, FechaAsociado=@FechaAsociado, Dni=@Dni, CantidadAsistencias=@CantidadAsistencias 
                      WHERE SocioId=@id", conn);

                cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@ClubId", System.Data.SqlDbType.Int).Value = socio.ClubId;
                cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar, 80).Value = socio.Nombre;
                cmd.Parameters.Add("@Apellido", System.Data.SqlDbType.NVarChar, 80).Value = socio.Apellido;
                cmd.Parameters.Add("@FechaNacimiento", System.Data.SqlDbType.DateTime).Value = socio.FechaNacimiento;
                cmd.Parameters.Add("@FechaAsociado", System.Data.SqlDbType.DateTime).Value = socio.FechaAsociado;
                cmd.Parameters.Add("@Dni", System.Data.SqlDbType.Int).Value = socio.Dni;
                cmd.Parameters.Add("@CantidadAsistencias", System.Data.SqlDbType.Int).Value = socio.CantidadAsistencias;

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
                await using var cmd = new SqlCommand("DELETE FROM Socios WHERE SocioId=@id", conn);
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