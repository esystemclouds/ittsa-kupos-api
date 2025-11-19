# Guía de instalación y ejecución local  
**Proyecto:** `ittsa-kupos-api`  
**Repositorio:** https://github.com/esystemclouds/ittsa-kupos-api.git  
**Tipo de proyecto:** REST API en C#  
**IDE recomendado:** Visual Studio 2022 o superior (Windows)

---

## 1. Prerrequisitos

1. **Sistema operativo**
   - Windows 10 o superior (64 bits).

2. **Visual Studio 2022 o superior**
   - Workloads recomendados al instalar Visual Studio:
     - **ASP.NET and web development**
     - **.NET desktop development** (opcional pero recomendable)
   - Asegúrate de incluir compatibilidad con **.NET Framework 4.x** (si el proyecto lo usa) o **.NET 6/7/8** (si fuera .NET Core/ASP.NET Core).

3. **Git**
   - Instalar desde: https://git-scm.com/downloads
   - Durante la instalación puedes usar las opciones por defecto.

4. **Base de datos**
   - El proyecto tiene una capa `IttsabusAPI.DataAccess`.  
   - Normalmente utilizará alguna cadena de conexión (por ejemplo, SQL Server).  
   - Deberás:
     - Tener instalado SQL Server (LocalDB, Developer o Express) **o** el motor de BD que use el proyecto.
     - Crear la base de datos necesaria y ajustar la cadena de conexión en el archivo de configuración (ver sección 5).

---

## 2. Clonar el repositorio

1. Abre **Git Bash**, **CMD** o **PowerShell** en la carpeta donde quieras guardar el código.  
2. Ejecuta:

   ```bash
   git clone https://github.com/esystemclouds/ittsa-kupos-api.git

**Al finalizar, tendrás una carpeta llamada ittsa-kupos-api con la solución:**
ittsa-kupos-api/
  IttsabusAPI.sln
  IttsabusAPI.EndPoint/
  IttsabusAPI.DataAccess/
  IttsabusAPI.Entidades/
  IttsabusAPI.Manager/
  packages/


  
## 3. Abrir la solución en Visual Studio

1. Abre Visual Studio 2022.
2. Menú File → Open → Project/Solution…
3. Navega a la carpeta clonada ittsa-kupos-api.
4. Selecciona el archivo IttsabusAPI.sln y haz clic en Open.
5. Espera a que Visual Studio cargue todos los proyectos de la solución.

## 4. Restaurar paquetes NuGet

1. En Visual Studio, en el Solution Explorer, haz clic derecho sobre la solución IttsabusAPI.sln.
2. Selecciona “Restore NuGet Packages”.
3. Espera a que se descarguen todas las dependencias.
4. Si aparece algún mensaje de error (por versión de .NET o paquetes antiguos), deberá revisarse caso a caso, pero en condiciones normales se descargan automáticamente.

## 5. Configuración de la base de datos y parámetros de la API
ASP.NET Web API (.NET Framework)
Archivo Web.config dentro del proyecto IttsabusAPI.EndPoint.
### 5.1. Revisar y ajustar la cadena de conexión
1. Abre el archivo de configuración correspondiente:
2. Web.config
3. Busca la sección de connectionStrings o el bloque donde se definan las cadenas de conexión, por ejemplo:
   <connectionStrings> <add name="KuposDb" connectionString="Data Source=SERVIDOR;Initial Catalog=NombreBD;User ID=usuario;Password=clave;" providerName="System.Data.SqlClient" /> </connectionStrings>

## 6. Seleccionar el proyecto de inicio y perfil de ejecución

En el Solution Explorer, identifica el proyecto que expone la API, normalmente IttsabusAPI.EndPoint.
Clic derecho sobre IttsabusAPI.EndPoint → Set as StartUp Project.
En la barra superior de Visual Studio:
 - Asegúrate de que el perfil de ejecución sea:IIS Express

## 7. Compilar el proyecto
1. Menú Build → Build Solution (o Ctrl + Shift + B).
2. Verifica en la ventana Error List que no existan errores de compilación.
3. Si aparecen advertencias (warnings), se pueden revisar, pero no impiden la ejecución inicial.

## 8. Ejecutar la API localmente

1. Pulsa F5 (Debug) o Ctrl + F5 (sin debug).
2. Visual Studio levantará la API:
 - Se abrirá el navegador apuntando a una URL como:
   http://localhost:12345/ o similar.

puedes probarla con:
Postman
 - curl en terminal
 - o la propia URL del navegador (GET simples).
 curl http://localhost:12345/api/NombreDelRecurso
