use db_carrito;

delimiter $$
create procedure sp_usuario_insertar
(
 in _nombres varchar(100),
 in _apellidos varchar(100),
 in _correo varchar(100),
 in _clave varchar(100),
 in _activo bit,
 out Mensaje varchar(500),
 out Resultado int
)
begin
 set Resultado = 0;
if not exists(select * from usuario where Correo = _correo) then
	insert into usuario(Nombres, Apellidos, Correo, Clave, Activo) values(_nombres, _apellidos, _correo, _clave, _activo);
    
    set Resultado = last_insert_id();
else
    set Mensaje = 'El correo del usuario ya existe';
end if;
end
$$

delimiter $$
create procedure sp_usuario_editar
(
 in _id int,
 in _nombres varchar(100),
 in _apellidos varchar(100),
 in _correo varchar(100),
 in _activo bit,
 out Mensaje varchar(500),
 out Resultado int
)
begin
 set Resultado = 0;
if not exists(select * from usuario where Correo = _correo and IdUsuario != _id) then
	update usuario set
    Nombres = _nombres,
    Apellidos = _apellidos,
    Correo = _correo,
    Activo = _activo
    where IdUsuario = _id;
    set Resultado = 1;
else
    set Mensaje = 'El correo del usuario ya existe';
end if;
end
$$

delimiter $$
create procedure sp_usuario_eliminar
(
in _id int
)
begin
 delete from usuario where IdUsuario = _id;
 end
 $$

delimiter $$
create procedure sp_categoria_registrar(
in _Descripcion varchar(100),
in _Activo bit,
out Mensaje varchar(500),
out Resultado int
)
begin
	set Resultado = 0;
	if not exists(select * from categoria where Descripcion = _Descripcion) then
		insert into categoria(Descripcion, Activo) values(_Descripcion, _Activo);
		set Resultado = last_insert_id();
    else
		set Mensaje = 'La categoria ya existe.';
	end if;
end
$$

delimiter $$
create procedure sp_categoria_editar(
in _Id int,
in _Descripcion varchar(100),
in _Activo bit,
out Mensaje varchar(500),
out Resultado bit
)
begin
	set Resultado = 0;
    if not exists(select * from categoria where Descripcion = _Descripcion and IdCategoria != _Id) then
		update categoria set Descripcion = _Descripcion,
        Activo = _Activo
        where IdCategoria = _Id;
        
        set Resultado = 1;
	else
		set Mensaje = 'La categoria ya existe';
	end if;
end$$

delimiter $$
create procedure sp_categoria_eliminar(
in _Id int,
out Mensaje varchar(500),
out Resultado bit)
begin
	set Resultado = 0;
    if not exists(select * from producto p inner join categoria c on c.IdCategoria = p.IdCategoria where p.IdCategoria = _Id)
    then
		delete from categoria where IdCategoria = _Id;
        set Resultado = 1;
	else
		set Mensaje = 'La Categoria se encuentra relacionada a un producto';
	end if;
end$$

Delimiter $$
CREATE PROCEDURE `sp_marca_editar`(
in _Id int,
in _Descripcion varchar(100),
in _Activo bit,
out Mensaje varchar(500),
out Resultado bit
)
begin
	set Resultado = 0;
    if not exists(select * from marca where Descripcion = _Descripcion and IdMarca != _Id) then
		update marca set Descripcion = _Descripcion,
        Activo = _Activo
        where IdMarca = _Id;
        
        set Resultado = 1;
	else
		set Mensaje = 'La marca ya existe';
	end if;
end$$

Delimiter $$
CREATE PROCEDURE `sp_marca_registrar`(
in _Descripcion varchar(100),
in _Activo bit,
out Mensaje varchar(500),
out Resultado int
)
begin
	set Resultado = 0;
	if not exists(select * from marca where Descripcion = _Descripcion) then
		insert into marca(Descripcion, Activo) values(_Descripcion, _Activo);
		set Resultado = last_insert_id();
    else
		set Mensaje = 'La marca ya existe.';
	end if;
end$$

Delimiter $$
CREATE PROCEDURE `sp_marca_eliminar`(
in _Id int,
out Mensaje varchar(500),
out Resultado bit)
begin
	set Resultado = 0;
    if not exists(select * from producto p inner join marca m on p.IdMarca = m.IdMarca where p.IdMarca = _Id)
    then
		delete from marca where IdMarca = _Id;
        set Resultado = 1;
	else
		set Mensaje = 'La Marca se encuentra relacionada a un producto';
	end if;
end$$

Delimiter $$
create procedure sp_producto_ingresar(
	in _Nombre varchar(100),
	in _Descripcion varchar(100),
	in _IdMarca int,
	in _IdCategoria int,
	in _Precio decimal,
	in _Stock int,
	in _Activo bit,
	out Mensaje varchar(100),
	out Resultado int)
begin
	set Resultado = 0;
    if not exists(select * from producto where nombre = _Nombre)
    then
		insert into producto(Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo)
					Values(_Nombre, _Descripcion, _IdMarca, _IdCategoria, _Precio, _Stock, _Activo);
		set Resultado = last_insert_id();
	else
		set Mensaje = 'El producto ya existe';
	end if;	
end$$

Delimiter $$
create procedure sp_producto_editar(
	in _IdProducto int,
    in _Nombre varchar(100),
	in _Descripcion varchar(100),
	in _IdMarca int,
	in _IdCategoria int,
	in _Precio decimal,
	in _Stock int,
	in _Activo bit,
	out Mensaje varchar(100),
	out Resultado bit)
begin
	set Resultado = 0;
    if not exists(select * from producto where nombre = _Nombre and IdProducto != _IdProducto)
    then
		update producto set
        Nombre = _Nombre,
        Descripcion = _descripcion,
        IdMarca = _IdMarca,
        IdCategoria = _IdCategoria,
        Precio = _Precio,
        Stock = _Stock,
        Activo = _Activo
        where IdProducto = _IdProducto;
        
		set Resultado = 1;
	else
		set Mensaje = 'El producto ya existe';
	end if;	
end$$

Delimiter $$
create procedure sp_producto_eliminar(
	in _IdProducto int,
	out Mensaje varchar(100),
	out Resultado bit)
begin
	set Resultado = 0;
    if not exists(select * from detalle_venta dv inner join producto p on p.IdProducto = dv.IdProducto where p.IdProducto = _IdProducto)
    then
		delete from producto where IdProducto = _IdProducto;
        set Resultado = 1;
	else
		set Mensaje = 'El producto se encuentra relacionado a una venta';
	end if;
end$$

Delimiter $$
create procedure sp_Producto_Selectivo()
begin
	select p.IdProducto, p.Nombre, p.Descripcion, 
    m.IdMarca, m.Descripcion as DesMarca,
    c.IdCategoria, c.Descripcion as DesCategoria,
    p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo
    from producto p 
	inner join marca m on m.IdMarca = p.IdMarca
	inner join categoria c on c.IdCategoria = p.IdCategoria; 
end$$

Delimiter $$
create procedure sp_producto_Imagen(
	in _RutaImagen varchar(100),
	in _NombreImagen varchar(100),
    in _IdProducto int)
begin
    if exists(select * from producto where IdProducto = _IdProducto) 
    then
		update producto set
        RutaImagen = _RutaImagen,
        NombreImagen = _NombreImagen
        where IdProducto = _IdProducto; 
	end if;
end$$

Delimiter $$
create procedure sp_ReporteDashboard()
begin
	select
	(select count(*) from cliente) TotalCliente,
	(select ifnull(sum(Cantidad), 0) from detalle_venta) TotalVenta,
	(select count(*) from producto) TotalProducto;
end$$

Delimiter $$
create procedure sp_Reporte_Ventas(
in _FechaInicio varchar(10),
in _FechaFin varchar(10),
in _IdTransaccion varchar(50))
begin
	
	select date_format( v.FechaVenta,"%d-%m-%Y") as FechaVenta, CONCAT( c.Nombres,' ', c.Apellidos) as Cliente,
	p.Nombre as Producto, p.Precio, dv.Cantidad, dv.Total, v.IdTransaccion
	from detalle_venta dv
	inner join producto p on p.IdProducto = dv.IdProducto
	inner join venta v on v.IdVenta = dv.IdVenta
	inner join cliente c on c.IdCliente = v.IdCliente
    where v.FechaVenta between _FechaInicio and _FechaFin
    and v.IdTransaccion = if(_IdTransaccion = '', v.IdTransaccion, _IdTransaccion);
end$$

Delimiter $$
create procedure sp_RegistrarCliente(
in _Nombres varchar(100),
in _Apellidos varchar(100),
in _Correo varchar(100),
in _Clave varchar(100),
out Mensaje varchar(500),
out Resultado int)
begin
	set Resultado = 0;
    if not exists(select * from cliente where correo = _Correo) 
    then
		insert into cliente(nombres, apellidos, correo, clave, reestablecer) values(_Nombres, _Apellidos, _Correo, _Clave, 0);
		set Resultado = last_insert_id();
    else
		set Mensaje = 'El correo del usuario ya existe';
	end if;
end$$

Delimiter $$
create procedure sp_marca_select(
in _idcategoria int)
begin

select distinct m.IdMarca, m.Descripcion from producto p
inner join categoria c on c.IdCategoria = p.IdCategoria
inner join marca m on m.IdMarca = p.IdMarca and m.Activo = 1
where c.IdCategoria = if(_idcategoria = 0, c.IdCategoria, _idcategoria);
end$$

Delimiter $$
create procedure sp_ExisteCarrito (
in _IdCliente int,
in _IdProducto int,
out Resultado bit)
begin
	if exists(select * from carrito where IdCliente = _IdCliente and IdProducto = _IdProducto) 
		then
		set Resultado = 1;
	else
		set Resultado = 0;
    end if;
end$$

Delimiter $$
create procedure sp_OperacionCarrito (
in _IdCliente int,
in _IdProducto int,
in _sumar bit,
out Mensaje varchar(500),
out Resultado bit)
begin

declare _existecarrito bit;
declare _stockproducto int;

set _existecarrito = if (exists(select * from carrito where IdCliente = _IdCliente and IdProducto = _IdProducto), 1, 0);
set _stockproducto = (select stock from producto where IdProducto = _IdProducto);
set Resultado = 1;
set Mensaje = '';

if(_sumar = 1) then
 if (_stockproducto >  0) then
  if(_existecarrito = 1) then
     update carrito set Cantidad = Cantidad + 1 where IdCliente = _IdCliente and IdProducto = _IdProducto;
     update producto set Stock = Stock - 1 where IdProducto = _IdProducto;
  else
     insert into carrito (IdCliente, IdProducto, Cantidad) values (_IdCliente, _IdProducto, 1);
	 update producto set Stock = Stock - 1 where IdProducto = _IdProducto;
     end if;
 else
  set Resultado = 0;
  set Mensaje = 'El producto no cuenta con stock disponible';
 end if;
else 
update carrito set Cantidad = Cantidad - 1 where IdCliente = _IdCliente and IdProducto = _IdProducto;
update producto set Stock = Stock + 1 where IdProducto = _IdProducto;
end if;
end$$

Delimiter $$
create procedure sp_ObtenerCarritoCliente(
in _IdCliente int)
begin
	select p.IdProducto, m.Descripcion as DesMarca, p.Nombre, p.Precio, c.Cantidad, p.RutaImagen, p.NombreImagen 
	from carrito c
	inner join producto p on p.IdProducto = c.IdProducto
	inner join marca m on m.IdMarca = p.IdMarca
	where c.IdCliente = _idcliente;
end$$

Delimiter $$
create procedure sp_EliminarCarrito(
in _IdCliente int,
in _IdProducto int,
out Resultado bit)
begin
	declare _CantidadProducto int;
    
	set Resultado = 1;
    set _CantidadProducto = (select cantidad from carrito where IdCliente = _IdCliente and IdProducto = _IdProducto);
    
    update producto set stock = stock + _CantidadProducto where IdProducto = _IdProducto;
    delete from carrito where IdCliente = _IdCliente and IdProducto = _IdProducto limit 1;
end$$

Delimiter $$
create procedure sp_Registrar_Venta(
in _IdCliente int,
in _TotalProducto int,
in _MontoTotal decimal(18,2),
in _Contacto varchar(100),
in _IdRegion varchar(2),
in _Telefono varchar(10),
in _Direccion varchar(100),
in _IdTransaccion varchar(50),
out Resultado int,
out Mensaje varchar(500)
)
begin
    set Resultado = 0;
    set Mensaje = '';
    
    insert into venta(IdCliente,TotalProducto,MontoTotal,Contacto,IdRegion,Telefono,Direccion,IdTransaccion)
    values(_Idcliente,_TotalProducto,_MontoTotal,_Contacto,_IdRegion,_Telefono,_Direccion,_IdTransaccion);
    set Resultado = last_insert_id();
    
    delete from carrito where IdCliente = _IdCliente;
end$$

Delimiter $$
create procedure sp_Registrar_DetalleVenta(
in _IdVenta int,
in _IdProducto int,
in _Cantidad int,
in _TotalProducto int,
out Resultado bit,
out Mensaje varchar(500)
)
begin
    insert into detalle_venta(IdVenta,IdProducto,Cantidad,Total) values(_IdVenta,_IdProducto,_Cantidad,_TotalProducto);
    set Resultado = 1;
end$$

Delimiter $$
create procedure sp_ListarCompra(
in _IdCliente int)
begin

select p.RutaImagen,p.NombreImagen,p.Nombre,p.Precio,
dv.Cantidad,dv.Total,v.IdTransaccion from detalle_venta dv 
inner join producto p on p.IdProducto = dv.IdProducto
inner join venta v on v.IdVenta = dv.IdVenta 
where v.IdCliente = _IdCliente;

end$$