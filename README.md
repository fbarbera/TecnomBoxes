TecnomBoxes es una aplicación Full-Stack (.NET + Angular) que permite la gestión de turnos para talleres.
El sistema permite:

Listar talleres activos (desde la API externa TecnomCRM).

Crear turnos (appointments) con validación de campos.

Listar los turnos registrados (almacenados en memoria).

🚀 Requisitos Previos
🖥 Backend (.NET 8 / 9)

Visual Studio 2022 o VS Code

SDK de .NET 8 o superior

Conexión a Internet (para consultar la API externa TecnomCRM)

💻 Frontend (Angular 20)

Node.js v20 o superior

Angular CLI v20 o superior

🏗️ Estructura del Proyecto
TecnomBoxes/
 ├── TecnomBoxes/                # Backend .NET API
 │   ├── Controllers/
 │   ├── Services/
 │   ├── Models/
 │   ├── Program.cs
 │   ├── appsettings.json
 │   └── ...
 └── tecnomBoxex/                # Frontend Angular
     ├── src/
     ├── app/
     └── ...

⚙️ Ejecución del Proyecto
🧠 1️⃣ Iniciar la API .NET (Backend)

Abrí la solución en Visual Studio o VS Code.

Ejecutá el proyecto TecnomBoxes.

La API se ejecutará en:
👉 http://localhost:5101

Verás en la terminal algo como:

info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5101
info: TecnomBoxes.Program[0]
      ✅ Conectado correctamente a TecnomCRM (200)


Si ves 401 Unauthorized, revisá las credenciales del archivo appsettings.json.

🌐 2️⃣ Iniciar la aplicación Angular (Frontend)

Abrí una nueva terminal dentro de la carpeta:

tecnomBoxex/


Ejecutá:

npm install
ng serve


La aplicación correrá en:
👉 http://localhost:4200

El frontend consumirá automáticamente la API de .NET en http://localhost:5101.

🧾 Endpoints del Backend
🔹 1. GET /api/Workshop

Descripción:
Obtiene la lista de talleres activos desde la API externa TecnomCRM.
La respuesta se cachea durante algunos minutos en memoria para optimizar el rendimiento.

Ejemplo de respuesta:

[
  {
    "id": 2,
    "name": "DEV TALLER",
    "address": "Diag. 74 & C. 47, La Plata, Provincia de Buenos Aires, Argentina",
    "email": "devtaller@tecnom.com.ar",
    "active": true
  }
]


Fuente de datos:
Se conecta a https://dev.tecnomcrm.com/api/v1/places/workshops
con autenticación Basic Auth (credenciales configuradas en appsettings.json):

"TecnomCRM": {
  "BaseUrl": "https://dev.tecnomcrm.com/api/v1/places/workshops",
  "User": "max@tecnom.com.ar",
  "Password": "b0x3sApp"
}

🔹 2. GET /api/Appointment/GetAll

Descripción:
Devuelve la lista de turnos previamente creados.
Los turnos se almacenan en memoria durante la ejecución de la aplicación.

Ejemplo de respuesta:

[
  {
    "place_id": 2,
    "appointment_at": "2025-11-10T14:30:00Z",
    "service_type": "Cambio de aceite",
    "contact": {
      "name": "Juan Pérez",
      "email": "juanperez@gmail.com",
      "phone": "1122334455"
    },
    "vehicle": {
      "make": "Toyota",
      "model": "Corolla",
      "year": 2019,
      "license_plate": "AB123CD"
    }
  }
]

🔹 3. POST /api/Appointment/Create

Descripción:
Permite registrar un nuevo turno.
El backend valida que:

Todos los campos obligatorios estén completos.

El place_id corresponda a un taller activo (consultando el endpoint /api/Workshop).

Ejemplo de cuerpo del request:

{
  "place_id": 2,
  "appointment_at": "2025-11-10T15:00:00Z",
  "service_type": "Cambio de aceite",
  "contact": {
    "name": "Juan Pérez",
    "email": "juanperez@gmail.com",
    "phone": "1122334455"
  },
  "vehicle": {
    "make": "Toyota",
    "model": "Corolla",
    "year": 2019,
    "license_plate": "AB123CD"
  }
}


Respuestas posibles:

Código	Significado	Ejemplo
201 Created	Turno creado correctamente	{ "message": "Turno creado correctamente" }
400 Bad Request	Faltan campos requeridos	{ "error": "El campo email es obligatorio." }
404 Not Found	Taller no activo	{ "error": "El taller no se encuentra activo." }
🧠 Validaciones Backend
Campo	Requerido	Descripción
place_id	✅	ID del taller
appointment_at	✅	Fecha y hora del turno (ISO 8601)
service_type	✅	Tipo de servicio solicitado
contact.name	✅	Nombre de la persona
contact.email	✅	Email de contacto
contact.phone	❌	Teléfono opcional
vehicle.*	❌	Todos los campos del vehículo son opcionales
💾 Almacenamiento

No se usa base de datos.

Los turnos se guardan temporalmente en memoria durante la ejecución del servidor.

Al reiniciar la API, los datos se pierden.

🔐 Autenticación externa

El sistema se conecta a la API de TecnomCRM usando Basic Authentication.
Estas credenciales se definen en appsettings.json y se aplican automáticamente al HttpClient configurado con NamedClient "TecnomCRM".

📦 Caching

Los talleres obtenidos desde TecnomCRM se almacenan en memoria (IMemoryCache) por 10 minutos:

_memoryCache.Set(CacheKey, activeWorkshopsList, TimeSpan.FromMinutes(10));


Esto evita llamadas repetitivas a la API externa y mejora la velocidad.

⚙️ Flujo de uso típico

1️⃣ Iniciar la API (TecnomBoxes)
2️⃣ Iniciar el Frontend (ng serve)
3️⃣ Ingresar a http://localhost:4200

4️⃣ Ver listado de turnos (vacío inicialmente)
5️⃣ Crear un nuevo turno (Seleccionar taller → fecha/hora → contacto → vehículo)
6️⃣ El turno aparece en el listado de inmediato

🧩 Autor / Créditos

Desarrollado por:
Facundo Joaquín Barbera
Full Stack Developer (.NET + Angular)
