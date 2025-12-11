# ?? Guía de Nomenclatura y Organización de Tests

## ??? Estructura de Carpetas

```
ALISTAMIENTO_IE.Tests/
??? Unit/                          # Tests unitarios (sin dependencias externas)
?   ??? Forms/                     # Tests de formularios WinForms
?   ?   ??? AlistamientoFormTests.cs
?   ?   ??? MenuFormTests.cs
?   ?   ??? FormDetalleAlistamientoTests.cs
?   ??? Services/                  # Tests de servicios de negocio
?       ??? AlistamientoServiceTests.cs
?       ??? EtiquetaServiceTests.cs
?       ??? KardexServiceTests.cs
??? Integration/                   # Tests de integración (con BD, APIs, etc.)
?   ??? Database/
?   ?   ??? AlistamientoRepositoryTests.cs
?   ?   ??? CamionRepositoryTests.cs
?   ??? Services/
?       ??? EmailServiceIntegrationTests.cs
??? Helpers/                       # Clases auxiliares reutilizables
?   ??? FormTestHelper.cs
?   ??? MockServiceFactory.cs
?   ??? TestDataBuilder.cs
??? Fixtures/                      # Fixtures compartidos entre tests
    ??? DatabaseFixture.cs
    ??? ServiceFixture.cs
```

## ?? Convenciones de Nomenclatura

### 1?? Archivos de Test
**Formato**: `{ClaseAProbar}Tests.cs`

**Ejemplos**:
- `AlistamientoFormTests.cs` ? Prueba el formulario `ALISTAMIENTO`
- `MenuFormTests.cs` ? Prueba el formulario `Menu`
- `AlistamientoServiceTests.cs` ? Prueba `AlistamientoService`

### 2?? Clases de Test
**Formato**: `public class {ClaseAProbar}Tests`

```csharp
public class AlistamientoFormTests { }
public class MenuFormTests { }
```

### 3?? Métodos de Test
**Formato**: `{MétodoOComponente}_{Escenario}_{ResultadoEsperado}`

**Ejemplos**:
```csharp
// ? CORRECTO
[Fact]
public void TxtEtiqueta_CuandoSeEscribe10Caracteres_DebeDispararEvento()

[Fact]
public void BtnAlistar_CuandoNoHayCamionSeleccionado_DebeMostrarMensajeError()

[Fact]
public void CargarCamiones_CuandoNoHayDatos_DebeRetornarListaVacia()

// ? INCORRECTO
[Fact]
public void Test1()

[Fact]
public void PruebaBoton()
```

### 4?? Categorías de Tests (Traits)
Usar atributos `[Trait]` para organizar:

```csharp
[Trait("Category", "Unit")]
[Trait("Category", "Integration")]
[Trait("Feature", "Alistamiento")]
[Trait("Priority", "High")]
```

## ?? Ejemplos Completos

### Test Unitario de Formulario
```csharp
// Archivo: Unit/Forms/AlistamientoFormTests.cs
namespace ALISTAMIENTO_IE.Tests.Unit.Forms;

[Trait("Category", "Unit")]
[Trait("Feature", "Alistamiento")]
public class AlistamientoFormTests
{
    [Fact]
    public void TxtEtiqueta_AlInicializar_DebeExistirEnFormulario()
    {
        // Arrange, Act, Assert
    }
}
```

### Test de Servicio
```csharp
// Archivo: Unit/Services/AlistamientoServiceTests.cs
namespace ALISTAMIENTO_IE.Tests.Unit.Services;

[Trait("Category", "Unit")]
public class AlistamientoServiceTests
{
    [Fact]
    public void ObtenerCamiones_CuandoHayDatos_DebeRetornarLista()
    {
        // Arrange, Act, Assert
    }
}
```

### Test de Integración
```csharp
// Archivo: Integration/Database/AlistamientoRepositoryTests.cs
namespace ALISTAMIENTO_IE.Tests.Integration.Database;

[Trait("Category", "Integration")]
[Collection("DatabaseCollection")]
public class AlistamientoRepositoryTests : IClassFixture<DatabaseFixture>
{
    [Fact]
    public async Task GuardarAlistamiento_ConDatosValidos_DebeGuardarEnBD()
    {
        // Arrange, Act, Assert
    }
}
```

## ?? Comandos para Ejecutar Tests

```bash
# Todos los tests
dotnet test

# Solo tests unitarios
dotnet test --filter "Category=Unit"

# Solo tests de integración
dotnet test --filter "Category=Integration"

# Tests de una feature específica
dotnet test --filter "Feature=Alistamiento"

# Tests de alta prioridad
dotnet test --filter "Priority=High"
```

## ? Checklist de Calidad

- [ ] Nombre del archivo termina en `Tests.cs`
- [ ] Clase usa nomenclatura `{ClaseAProbar}Tests`
- [ ] Métodos usan formato `{Método}_{Escenario}_{Resultado}`
- [ ] Tiene atributo `[Fact]` o `[Theory]`
- [ ] Usa patrón Arrange-Act-Assert
- [ ] Tiene `[Trait]` para categorización
- [ ] Los asserts tienen mensajes descriptivos
- [ ] Los mocks están claramente nombrados (`mock{Servicio}`)
