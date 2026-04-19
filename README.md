API REST construida con ASP.NET Core orientada a la gestión de un sistema de stock y órdenes.
Incluye autenticación de usuarios, manejo de productos, procesamiento de órdenes mediante operaciones CRUD y conexión a terceros (Cloudinary storage).

La aplicación está desplegada en AWS utilizando una instancia EC2 y conectada a una base de datos en Amazon RDS (SQL Server), con configuración de networking previa (security groups, reglas de acceso y conectividad entre servicios).

Además, integra servicios de terceros para el almacenamiento de archivos mediante Cloudinary.

Tecnologías utilizadas:

ASP.NET Core
Entity Framework Core
SQL Server (Amazon RDS)
AWS EC2
Cloudinary (gestión de archivos/media)

Testing y validación:

Pruebas de endpoints realizadas con Postman
Validación de flujos CRUD y autenticación

Estado del proyecto: en desarrollo. Próximamente se agregarán validaciones más robustas, manejo de errores centralizado, roles de usuario y mejoras en seguridad.

Proyecto enfocado en la práctica de desarrollo backend, despliegue en la nube, integración con servicios externos y configuración de infraestructura en AWS.
