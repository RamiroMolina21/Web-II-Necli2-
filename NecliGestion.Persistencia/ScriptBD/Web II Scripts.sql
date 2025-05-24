-- Tabla Usuario
CREATE TABLE Usuario (
    IdUsuario INT PRIMARY KEY IDENTITY(1,1),
    Identificacion NVARCHAR(50) NOT NULL,
    Telefono NVARCHAR(20) NOT NULL UNIQUE,
    TipoUsuario NVARCHAR(20) NOT NULL,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL,
    Contrasena NVARCHAR(100) NOT NULL,
    FechaNacimiento DATE NOT NULL
);

-- Tabla Cuenta
CREATE TABLE Cuenta (
    IdCuenta INT PRIMARY KEY IDENTITY(1,1),
    Telefono NVARCHAR(20) NOT NULL UNIQUE,
    Nombres NVARCHAR(100) NOT NULL,
    Saldo DECIMAL(18,2) NOT NULL,
    FechaCreacion DATE NOT NULL,
    UsuarioId INT NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(IdUsuario) ON DELETE CASCADE
);

-- En Transaccion, dejamos SOLO UNA eliminación en cascada
CREATE TABLE Transaccion (
    IdTransaccion INT PRIMARY KEY IDENTITY(1,1),
    FechaTransaccion DATETIME NOT NULL,
    NumeroCuentaOrigen NVARCHAR(20) NOT NULL,
    NumeroCuentaDestino NVARCHAR(20) NOT NULL,
    Monto DECIMAL(18,2) NOT NULL,
    Tipo NVARCHAR(50) NOT NULL,
    FOREIGN KEY (NumeroCuentaOrigen) REFERENCES Cuenta(Telefono) ON DELETE CASCADE,
    FOREIGN KEY (NumeroCuentaDestino) REFERENCES Cuenta(Telefono) ON DELETE NO ACTION
);

-- Insertar Usuarios
INSERT INTO Usuario (Identificacion, Telefono, TipoUsuario, Nombres, Apellidos, Correo, Contrasena, FechaNacimiento) VALUES
('1234567890', '3000000001', 'Cliente', 'Juan', 'Pérez', 'juan1@example.com', 'pass1', '1990-01-01'),
('2234567890', '3000000002', 'Cliente', 'Ana', 'López', 'ana2@example.com', 'pass2', '1992-02-02'),
('3234567890', '3000000003', 'Cliente', 'Luis', 'Martínez', 'luis3@example.com', 'pass3', '1993-03-03'),
('4234567890', '3000000004', 'Cliente', 'Laura', 'Gómez', 'laura4@example.com', 'pass4', '1994-04-04'),
('5234567890', '3000000005', 'Cliente', 'Carlos', 'Rodríguez', 'carlos5@example.com', 'pass5', '1995-05-05'),
('6234567890', '3000000006', 'Cliente', 'Sofía', 'Ramírez', 'sofia6@example.com', 'pass6', '1996-06-06'),
('7234567890', '3000000007', 'Cliente', 'Mario', 'Díaz', 'mario7@example.com', 'pass7', '1997-07-07'),
('8234567890', '3000000008', 'Cliente', 'Paula', 'Herrera', 'paula8@example.com', 'pass8', '1998-08-08'),
('9234567890', '3000000009', 'Cliente', 'Andrés', 'Jiménez', 'andres9@example.com', 'pass9', '1999-09-09'),
('1034567890', '3000000010', 'Cliente', 'Natalia', 'Ruiz', 'natalia10@example.com', 'pass10', '2000-10-10'),
('1134567890', '3000000011', 'Cliente', 'Jorge', 'Vargas', 'jorge11@example.com', 'pass11', '1991-11-11'),
('1234567891', '3000000012', 'Cliente', 'Camila', 'Silva', 'camila12@example.com', 'pass12', '1993-12-12'),
('1334567890', '3000000013', 'Cliente', 'David', 'Morales', 'david13@example.com', 'pass13', '1988-03-14'),
('1434567890', '3000000014', 'Cliente', 'Valeria', 'Rojas', 'valeria14@example.com', 'pass14', '1990-06-17'),
('1534567890', '3000000015', 'Cliente', 'Esteban', 'Gutiérrez', 'esteban15@example.com', 'pass15', '1985-07-21'),
('1634567890', '3000000016', 'Cliente', 'Daniela', 'Mendoza', 'daniela16@example.com', 'pass16', '1992-08-23'),
('1734567890', '3000000017', 'Cliente', 'Martín', 'Ortega', 'martin17@example.com', 'pass17', '1994-09-25'),
('1834567890', '3000000018', 'Cliente', 'Lucía', 'Santos', 'lucia18@example.com', 'pass18', '1989-10-29'),
('1934567890', '3000000019', 'Cliente', 'Gabriel', 'Torres', 'gabriel19@example.com', 'pass19', '1993-11-30'),
('2034567890', '3000000020', 'Cliente', 'Fernanda', 'Castro', 'fernanda20@example.com', 'pass20', '1997-12-31');

-- Insertar Cuentas
INSERT INTO Cuenta (Telefono, Nombres, Saldo, FechaCreacion, UsuarioId) VALUES
('3000000001', 'Juan', 1200.00, GETDATE(), 1),
('3000000002', 'Ana', 800.00, GETDATE(), 2),
('3000000003', 'Luis', 450.00, GETDATE(), 3),
('3000000004', 'Laura', 300.00, GETDATE(), 4),
('3000000005', 'Carlos', 980.00, GETDATE(), 5),
('3000000006', 'Sofía', 750.00, GETDATE(), 6),
('3000000007', 'Mario', 620.00, GETDATE(), 7),
('3000000008', 'Paula', 1100.00, GETDATE(), 8),
('3000000009', 'Andrés', 500.00, GETDATE(), 9),
('3000000010', 'Natalia', 200.00, GETDATE(), 10),
('3000000011', 'Jorge', 650.00, GETDATE(), 11),
('3000000012', 'Camila', 720.00, GETDATE(), 12),
('3000000013', 'David', 800.00, GETDATE(), 13),
('3000000014', 'Valeria', 900.00, GETDATE(), 14),
('3000000015', 'Esteban', 1000.00, GETDATE(), 15),
('3000000016', 'Daniela', 950.00, GETDATE(), 16),
('3000000017', 'Martín', 300.00, GETDATE(), 17),
('3000000018', 'Lucía', 880.00, GETDATE(), 18),
('3000000019', 'Gabriel', 430.00, GETDATE(), 19),
('3000000020', 'Fernanda', 1200.00, GETDATE(), 20);

-- Insertar Transacciones (entre teléfonos)
INSERT INTO Transaccion (FechaTransaccion, NumeroCuentaOrigen, NumeroCuentaDestino, Monto, Tipo) VALUES
(GETDATE(), '3000000001', '3000000002', 50.00, 'Recibido'),
(GETDATE(), '3000000003', '3000000004', 75.00, 'Enviado'),
(GETDATE(), '3000000005', '3000000006', 100.00, 'Enviado'),
(GETDATE(), '3000000007', '3000000008', 120.00, 'Recibido'),
(GETDATE(), '3000000009', '3000000010', 30.00, 'Enviado'),
(GETDATE(), '3000000011', '3000000012', 60.00, 'Recibido'),
(GETDATE(), '3000000013', '3000000014', 90.00, 'Enviado'),
(GETDATE(), '3000000015', '3000000016', 40.00, 'Enviado'),
(GETDATE(), '3000000017', '3000000018', 110.00, 'Enviado'),
(GETDATE(), '3000000019', '3000000020', 80.00, 'Recibido'),
(GETDATE(), '3000000002', '3000000001', 25.00, 'Enviado'),
(GETDATE(), '3000000004', '3000000003', 35.00, 'Recibido'),
(GETDATE(), '3000000006', '3000000005', 70.00, 'Enviado'),
(GETDATE(), '3000000008', '3000000007', 60.00, 'Recibido'),
(GETDATE(), '3000000010', '3000000009', 20.00, 'Recibido'),
(GETDATE(), '3000000012', '3000000011', 15.00, 'Enviado'),
(GETDATE(), '3000000014', '3000000013', 45.00, 'Recibido'),
(GETDATE(), '3000000016', '3000000015', 85.00, 'Recibido'),
(GETDATE(), '3000000018', '3000000017', 95.00, 'Recibido'),
(GETDATE(), '3000000020', '3000000019', 55.00, 'Enviado');
