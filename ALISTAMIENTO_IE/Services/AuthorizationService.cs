namespace ALISTAMIENTO_IE.Services
{
    using ALISTAMIENTO_IE.Interfaces;
    using BCrypt.Net;
    using Dapper;
    using Microsoft.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Threading.Tasks;

    public class AuthorizationService : IAuthorizationService
    {
        private readonly IDbConnection _connection;

        // Existing constructor for DI - keep it
        public AuthorizationService(IDbConnection connection)
        {
            _connection = connection;
        }

        // Convenience constructor that accepts a raw connection string and creates a SqlConnection
        public AuthorizationService(string connectionString)
            : this(new SqlConnection(connectionString))
        {
        }

        // Factory to create the service using a connection string named in app.config / web.config
        public static AuthorizationService CreateFromConfig(string connectionStringName = "DefaultConnection")
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionStringName]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs))
            {
                throw new ConfigurationErrorsException($"Connection string '{connectionStringName}' not found in config.");
            }

            return new AuthorizationService(cs);
        }


        public async Task<int> CreateAuthorizationAsync(int areaId, int roleId, string plainPassword)
        {
            string hashedPassword = BCrypt.HashPassword(plainPassword);

            const string query = @"
            INSERT INTO AUTORIZACION_AREA_ROL (ID_AREA, ID_ROL, HASHED_PASSWORD, ACTIVO)
            VALUES (@AreaId, @RoleId, @HashedPassword, 1);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
        ";

            return await _connection.ExecuteScalarAsync<int>(query, new { AreaId = areaId, RoleId = roleId, HashedPassword = hashedPassword });
        }

        public async Task<bool> UpdatePasswordAsync(int areaId, int roleId, string newPlainPassword)
        {
            string newHashed = BCrypt.HashPassword(newPlainPassword);

            const string query = @"
            UPDATE AUTORIZACION_AREA_ROL
            SET HASHED_PASSWORD = @NewHashed, FECHA_ACTUALIZACION = GETDATE()
            WHERE ID_AREA = @AreaId AND ID_ROL = @RoleId;
        ";

            int rows = await _connection.ExecuteAsync(query, new { AreaId = areaId, RoleId = roleId, NewHashed = newHashed });
            return rows > 0;
        }

        public async Task<bool> ValidatePasswordAsync(int areaId, int roleId, string inputPassword)
        {
            const string query = @"
            SELECT HASHED_PASSWORD FROM AUTORIZACION_AREA_ROL
            WHERE ID_AREA = @AreaId AND ID_ROL = @RoleId AND ACTIVO = 1;
        ";

            string? hashed = await _connection.QueryFirstOrDefaultAsync<string>(query, new { AreaId = areaId, RoleId = roleId });
            if (hashed == null) return false;

            return BCrypt.Verify(inputPassword, hashed);
        }
    }

}
