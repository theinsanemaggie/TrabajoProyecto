CREATE DATABASE ClubesDB;
GO

USE ClubesDB;
GO

-- Tabla Clubes
CREATE TABLE Clubes (
    ClubId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    CantidadSocios INT NOT NULL DEFAULT 0,
    CantidadTitulos INT NOT NULL DEFAULT 0,
    FechaFundacion DATETIME2 NOT NULL,
    UbicacionEstadio NVARCHAR(150),
    NombreEstadio NVARCHAR(120)
);
GO

-- Tabla Dirigentes
CREATE TABLE Dirigentes (
    DirigenteId INT IDENTITY(1,1) PRIMARY KEY,
    ClubId INT NOT NULL,
    Nombre NVARCHAR(80) NOT NULL,
    Apellido NVARCHAR(80) NOT NULL,
    FechaNacimiento DATETIME2 NOT NULL,
    Rol NVARCHAR(80) NOT NULL,
    Dni INT NOT NULL,
    FOREIGN KEY (ClubId) REFERENCES Clubes(ClubId) ON DELETE CASCADE
);
GO

-- Tabla Socios
CREATE TABLE Socios (
    SocioId INT IDENTITY(1,1) PRIMARY KEY,
    ClubId INT NOT NULL,
    Nombre NVARCHAR(80) NOT NULL,
    Apellido NVARCHAR(80) NOT NULL,
    FechaNacimiento DATETIME2 NOT NULL,
    FechaAsociado DATETIME2 NOT NULL,
    Dni INT NOT NULL,
    CantidadAsistencias INT NOT NULL DEFAULT 0,
    FOREIGN KEY (ClubId) REFERENCES Clubes(ClubId) ON DELETE CASCADE
);
GO