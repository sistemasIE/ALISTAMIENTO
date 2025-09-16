-- Sentencia para crear la tabla UBICACIONES_ALISTAMIENTO.
CREATE TABLE UBICACIONES_ALISTAMIENTO (
    id INT PRIMARY KEY IDENTITY(1,1),
    estado BIT NOT NULL,
    nombre VARCHAR(20) NOT NULL
);
GO

-- Sentencias para insertar las 39 ubicaciones.
-- Cada número del 1 al 13 tiene tres variantes: A, B, y C.
INSERT INTO UBICACIONES_ALISTAMIENTO (nombre, estado)
VALUES
('AL-1-A', 1), ('AL-1-B', 1), ('AL-1-C', 1),
('AL-2-A', 1), ('AL-2-B', 1), ('AL-2-C', 1),
('AL-3-A', 1), ('AL-3-B', 1), ('AL-3-C', 1),
('AL-4-A', 1), ('AL-4-B', 1), ('AL-4-C', 1),
('AL-5-A', 1), ('AL-5-B', 1), ('AL-5-C', 1),
('AL-6-A', 1), ('AL-6-B', 1), ('AL-6-C', 1),
('AL-7-A', 1), ('AL-7-B', 1), ('AL-7-C', 1),
('AL-8-A', 1), ('AL-8-B', 1), ('AL-8-C', 1),
('AL-9-A', 1), ('AL-9-B', 1), ('AL-9-C', 1),
('AL-10-A', 1), ('AL-10-B', 1), ('AL-10-C', 1),
('AL-11-A', 1), ('AL-11-B', 1), ('AL-11-C', 1),
('AL-12-A', 1), ('AL-12-B', 1), ('AL-12-C', 1),
('AL-13-A', 1), ('AL-13-B', 1), ('AL-13-C', 1);
GO