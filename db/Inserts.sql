select * from region;

insert into Region( Descripcion) values('Arica y Parinacota'),('Tarapacá');

select * from comuna;

insert into comuna(Descripcion,IdRegion) values('Arica',1),('Camarones',1),('Putre',1),('General Lagos',1),('Iquique',2)
,('Alto Hospicio',2),('Pozo Almonte',2),('Camiña',2),('Colchane',2),('Huara',2),('Pica',2);

insert into usuario(Nombres,Apellidos,Correo,Clave,Reestablecer,Activo) values('Administrador','Administrador','izxj_pboaw4@TodoTecno.cl','a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3',0,1)
