using ALISTAMIENTO_IE.DTOs;
using ALISTAMIENTO_IE.Interfaces;
using ALISTAMIENTO_IE.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ALISTAMIENTO_IE.Services;
public class EtiquetaService : IEtiquetaService
{
    private readonly string _connectionString;

    public EtiquetaService()
    {

        _connectionString = ConfigurationManager.ConnectionStrings["stringConexionLocal"].ConnectionString;
    }

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
            Etiqueta? etiqueta = await connection.QueryFirstOrDefaultAsync<Etiqueta>(
                "SELECT * FROM ETIQUETA WHERE COD_ETIQUETA = @codigo", new { codigo = codigoEtiqueta });

            EtiquetaLiner? etiquetaLiner = await connection.QueryFirstOrDefaultAsync<EtiquetaLiner>(
                "SELECT * FROM ETIQUETA_LINER WHERE COD_ETIQUETA_LINER = @codigo", new { codigo = codigoEtiqueta });

            EtiquetaRollo? etiquetaRollo = await connection.QueryFirstOrDefaultAsync<EtiquetaRollo>(
                @"SELECT 
                   *
                FROM ETIQUETA_ROLLO 
                WHERE Cod_Etiqueta_Rollo = @codigo",
                new { codigo = codigoEtiqueta });

            KardexBodega? kardex = await connection.QueryFirstOrDefaultAsync<KardexBodega>(
                "SELECT * FROM KARDEX_BODEGA WHERE enBodega = 1 and etiqueta = @codigo", new { codigo = codigoEtiqueta });
            result.ExisteEnKardex = kardex != null;

            if ((etiqueta != null || etiquetaLiner != null || etiquetaRollo != null))
            {

                // Obtener el item: 
                var item = etiqueta?.COD_ITEM ?? etiquetaLiner?.ITEM ?? etiquetaRollo?.Item;

                // Obtener el tipo de unidad del item.



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
                result.Mensaje = $"✗✗✗✗✗ N.E – {codigoEtiqueta} ✗✗✗✗✗";
                return result;
            }
            else
            {
                result.Existe = false;
                result.Mensaje = $"✗✗✗✗✗ N.E – {codigoEtiqueta} ✗✗✗✗✗";
                return result;
            }
        }
    }
}