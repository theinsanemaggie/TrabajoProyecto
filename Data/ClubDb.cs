using Microsoft.Data.SqlClient;
using TrabajoProyecto.Models;
using System.Data;

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
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        CantidadSocios = reader.GetInt32(reader.GetOrdinal("CantidadSocios")),
                        CantidadTitulos = reader.GetInt32(reader.GetOrdinal("CantidadTitulos")),
                        FechaFundacion = reader.GetDateTime(reader.GetOrdinal("FechaFundacion")),
                        UbicacionEstadio = reader.IsDBNull(reader.GetOrdinal("UbicacionEstadio")) ? null : reader.GetString(reader.GetOrdinal("UbicacionEstadio")),
                        NombreEstadio = reader.IsDBNull(reader.GetOrdinal("NombreEstadio")) ? null : reader.GetString(reader.GetOrdinal("NombreEstadio"))
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
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    club = new Club
                    {
                        ClubId = reader.GetInt32(reader.GetOrdinal("ClubId")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        CantidadSocios = reader.GetInt32(reader.GetOrdinal("CantidadSocios")),
                        CantidadTitulos = reader.GetInt32(reader.GetOrdinal("CantidadTitulos")),
                        FechaFundacion = reader.GetDateTime(reader.GetOrdinal("FechaFundacion")),
                        UbicacionEstadio = reader.IsDBNull(reader.GetOrdinal("UbicacionEstadio")) ? null : reader.GetString(reader.GetOrdinal("UbicacionEstadio")),
                        NombreEstadio = reader.IsDBNull(reader.GetOrdinal("NombreEstadio")) ? null : reader.GetString(reader.GetOrdinal("NombreEstadio"))
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
                var query = @"
                    INSERT INTO Clubes (Nombre, CantidadSocios, CantidadTitulos, FechaFundacion, UbicacionEstadio, NombreEstadio)
                    VALUES (@Nombre, @CantidadSocios, @CantidadTitulos, @FechaFundacion, @UbicacionEstadio, @NombreEstadio)";

                await using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = club.Nombre;
                cmd.Parameters.Add("@CantidadSocios", SqlDbType.Int).Value = club.CantidadSocios;
                cmd.Parameters.Add("@CantidadTitulos", SqlDbType.Int).Value = club.CantidadTitulos;
                cmd.Parameters.Add("@FechaFundacion", SqlDbType.DateTime2).Value = club.FechaFundacion;
                cmd.Parameters.Add("@UbicacionEstadio", SqlDbType.NVarChar, 150).Value = (object)club.UbicacionEstadio ?? DBNull.Value;
                cmd.Parameters.Add("@NombreEstadio", SqlDbType.NVarChar, 120).Value = (object)club.NombreEstadio ?? DBNull.Value;

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
                var query = @"
                    UPDATE Clubes
                    SET Nombre = @Nombre,
                        CantidadSocios = @CantidadSocios,
                        CantidadTitulos = @CantidadTitulos,
                        FechaFundacion = @FechaFundacion,
                        UbicacionEstadio = @UbicacionEstadio,
                        NombreEstadio = @NombreEstadio
                    WHERE ClubId = @id";

                await using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = club.Nombre;
                cmd.Parameters.Add("@CantidadSocios", SqlDbType.Int).Value = club.CantidadSocios;
                cmd.Parameters.Add("@CantidadTitulos", SqlDbType.Int).Value = club.CantidadTitulos;
                cmd.Parameters.Add("@FechaFundacion", SqlDbType.DateTime2).Value = club.FechaFundacion;
                cmd.Parameters.Add("@UbicacionEstadio", SqlDbType.NVarChar, 150).Value = (object)club.UbicacionEstadio ?? DBNull.Value;
                cmd.Parameters.Add("@NombreEstadio", SqlDbType.NVarChar, 120).Value = (object)club.NombreEstadio ?? DBNull.Value;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

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
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

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