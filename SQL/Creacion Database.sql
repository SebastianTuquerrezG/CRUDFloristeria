-- Crear tabla Cliente
CREATE TABLE Cliente (
    CodigoCliente INT NOT NULL AUTO_INCREMENT,
    NombreCliente VARCHAR(255) NOT NULL,
    CorreoCliente VARCHAR(255),
    TelefonoCliente VARCHAR(20) NOT NULL,
    FechaNacimientoCliente DATE,
    PRIMARY KEY (CodigoCliente)
);

-- Crear tabla Venta
CREATE TABLE Venta (
    CodigoVenta INT NOT NULL AUTO_INCREMENT,
    CodigoCliente INT NOT NULL,
    FechaVenta DATE NOT NULL,
    ProductoVenta VARCHAR(255) NOT NULL,
    PrecioVenta DECIMAL(10, 2) NOT NULL,
    MensajeVenta TEXT,
    FotoVenta MEDIUMBLOB,
    PRIMARY KEY (CodigoVenta),
    FOREIGN KEY (CodigoCliente) REFERENCES Cliente(CodigoCliente)
);

-- Añadir restricciones y opciones adicionales
ALTER TABLE Cliente ADD CONSTRAINT chk_fecha_nacimiento CHECK (FechaNacimientoCliente IS NULL OR (MONTH(FechaNacimientoCliente) BETWEEN 1 AND 12 AND DAY(FechaNacimientoCliente) BETWEEN 1 AND 31));

-- Asegúrate de que las tablas fueron creadas correctamente
SHOW TABLES;

select * from cliente;
select * from venta;
SELECT CodigoVenta, LENGTH(FotoVenta) AS ImageSize FROM Venta;