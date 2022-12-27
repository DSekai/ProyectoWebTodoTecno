

create database DB_CARRITO;
USE DB_CARRITO;


CREATE TABLE CATEGORIA(
IdCategoria int primary key auto_increment,
Descripcion varchar(100),
Activo bit default 1,
FechaRegistro datetime default current_timestamp()
);

CREATE TABLE MARCA(
IdMarca int primary key auto_increment,
Descripcion varchar(100),
Activo bit default 1,
FechaRegistro datetime default current_timestamp()
);

CREATE TABLE PRODUCTO(
IdProducto int primary key auto_increment,
Nombre varchar(500),
Descripcion varchar(500),
IdMarca int,
foreign key (IdMarca) references Marca(IdMarca),
IdCategoria int,
Foreign key (IdCategoria) references Categoria(IdCategoria),
Precio decimal(10,2) default 0,
Stock int,
RutaImagen varchar(100),
NombreImagen varchar(100),
Activo bit default 1,
FechaRegistro datetime default current_timestamp()
);

CREATE TABLE CLIENTE(
IdCliente int primary key auto_increment,
Nombres varchar(100),
Apellidos varchar(100),
Correo varchar(100),
Clave varchar(150),
Reestablecer bit default 0,
FechaRegistro datetime default current_timestamp()
);

CREATE TABLE CARRITO(
IdCarrito int primary key auto_increment,
IdCliente int,
Foreign key (IdCliente) references CLIENTE(IdCliente),
IdProducto int,
Foreign key (IdProducto) references PRODUCTO(IdProducto),
Cantidad int
);

create table VENTA(
IdVenta int primary key auto_increment,
IdCliente int,
Foreign key (IdCliente) references CLIENTE(IdCliente),
TotalProducto int,
MontoTotal decimal(10,2),
Contacto varchar(50),
IdRegion varchar(10),
Telefono varchar(50),
Direccion varchar(500),
IdTransaccion varchar(50),
FechaVenta datetime default current_timestamp()
);

create table DETALLE_VENTA(
IdDetalleVenta int primary key auto_increment,
IdVenta int,
foreign key (IdVenta) references venta (IdVenta),
IdProducto int,
Foreign key(IdProducto) references PRODUCTO(IdProducto),
Cantidad int,
Total decimal(10,2)
);

CREATE TABLE USUARIO(
IdUsuario int primary key auto_increment,
Nombres varchar(100),
Apellidos varchar(100),
Correo varchar(100),
Clave varchar(150),
Reestablecer bit default 1,
Activo bit default 1,
FechaRegistro datetime default current_timestamp()
);

CREATE TABLE Region (
  IdRegion int primary key auto_increment,
  Descripcion varchar(45) NOT NULL
);

CREATE TABLE Comuna (
  IdComuna int auto_increment primary key,
  Descripcion varchar(45) NOT NULL,
  IdRegion int,
  foreign key(IdRegion) references Region(IdRegion)
);
