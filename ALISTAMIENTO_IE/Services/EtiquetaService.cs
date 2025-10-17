using ALISTAMIENTO_IE.Model;
using ALISTAMIENTO_IE.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ALISTAMIENTO_IE.Services;

public class EtiquetaBusquedaResult
{
    public bool EsValida { get; set; }
    public bool Existe { get; set; }
    public bool ExisteEnKardex { get; set; }
    public string TipoEtiqueta { get; set; } // "ETIQUETA", "ETIQUETA_LINER" o "ETIQUETA_ROLLO"
    public Etiqueta Etiqueta { get; set; }
    public EtiquetaLiner EtiquetaLiner { get; set; }
    public EtiquetaRollo EtiquetaRollo { get; set; }
    public string Mensaje { get; set; }
}

public class EtiquetaService
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["stringConexionLocal"].ConnectionString;
    private readonly string _connectionStringUnoE = ConfigurationManager.ConnectionStrings["stringConexionUnoe"].ConnectionString;

    public EtiquetaService() { }

    public async Task<Etiqueta?> ObtenerEtiquetaPorCodigoAsync(string codigoEtiqueta)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = @"SELECT * FROM ETIQUETA WHERE COD_ETIQUETA = @codigo";

        return await connection.QueryFirstOrDefaultAsync<Etiqueta>(sql, new { codigo = codigoEtiqueta });
    }

    // Async wrappers using Dapper async APIs to avoid bloquear la UI
    public async Task<EtiquetaBusquedaResult> ValidarEtiquetaEnKardexAsync(string codigoEtiqueta)
    {
        var result = new EtiquetaBusquedaResult();
        result.EsValida = !string.IsNullOrWhiteSpace(codigoEtiqueta) && codigoEtiqueta.Length == 10 && codigoEtiqueta.Any(char.IsLetter);
        if (!result.EsValida)
        {
            result.Mensaje = "La etiqueta debe tener 10 caracteres y al menos una letra.";
            return result;
        }

        await using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Buscar en las tres tablas
            var etiqueta = await connection.QueryFirstOrDefaultAsync<Etiqueta>(
                "SELECT * FROM ETIQUETA WHERE COD_ETIQUETA = @codigo", new { codigo = codigoEtiqueta });

            var etiquetaLiner = await connection.QueryFirstOrDefaultAsync<EtiquetaLiner>(
                "SELECT * FROM ETIQUETA_LINER WHERE COD_ETIQUETA_LINER = @codigo", new { codigo = codigoEtiqueta });

            var etiquetaRollo = await connection.QueryFirstOrDefaultAsync<EtiquetaRollo>(
                @"SELECT 
                   *
                FROM ETIQUETA_ROLLO 
                WHERE Cod_Etiqueta_Rollo = @codigo",
                new { codigo = codigoEtiqueta });

            var kardex = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SELECT * FROM KARDEX_BODEGA WHERE enBodega = 1 and etiqueta = @codigo", new { codigo = codigoEtiqueta });
            result.ExisteEnKardex = kardex != null;

            if ((etiqueta != null || etiquetaLiner != null || etiquetaRollo != null))
            {
                result.Existe = true;

                // Determinar el tipo de etiqueta (prioridad: ETIQUETA > ETIQUETA_LINER > ETIQUETA_ROLLO)
                if (etiqueta != null)
                {
                    result.TipoEtiqueta = "ETIQUETA";
                    result.Etiqueta = etiqueta;
                }
                else if (etiquetaLiner != null)
                {
                    result.TipoEtiqueta = "ETIQUETA_LINER";
                    result.EtiquetaLiner = etiquetaLiner;
                }
                else // etiquetaRollo != null
                {
                    result.TipoEtiqueta = "ETIQUETA_ROLLO";
                    result.EtiquetaRollo = etiquetaRollo;
                }

                if (!result.ExisteEnKardex)
                {
                    await connection.ExecuteAsync(@"INSERT INTO KARDEX_BODEGA (etiqueta, idBodega, area, fechaIngreso, TipoEntrada, enBodega, idUsuarioEntrante) VALUES (@etiqueta, @idBodega, @area, @fechaIngreso, @tipoEntrada, @enBodega, @idUsuarioEntrante)",
                        new
                        {
                            etiqueta = codigoEtiqueta,
                            idBodega = 1,
                            area = "ALISTAMIENTO",
                            fechaIngreso = DateTime.Now,
                            tipoEntrada = "M",
                            enBodega = true,
                            idUsuarioEntrante = Common.cache.UserLoginCache.IdUser
                        });
                    result.Mensaje = "ETIQUETA INSERTADA A KARDEX_BODEGA y ALISTADA";
                }
                else
                {
                    result.Mensaje = "Etiqueta ya existe en KARDEX_BODEGA.";
                }
                return result;
            }
            else if (!result.ExisteEnKardex)
            {
                result.Existe = false;
                result.Mensaje = $"N.E – Etiqueta: {codigoEtiqueta}";
                return result;
            }
            else
            {
                result.Existe = false;
                result.Mensaje = $"N.E – Etiqueta: {codigoEtiqueta}";
                return result;
            }
        }
    }
}