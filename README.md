# 🏖️ Refugio del Sol - Sistema de Reservas

![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)
![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4?style=flat&logo=blazor)
![MudBlazor](https://img.shields.io/badge/MudBlazor-8.15.0-594AE2?style=flat)
![License](https://img.shields.io/badge/license-Proprietary-red?style=flat)

Sistema de reservas moderno y responsivo para **Refugio del Sol**, una playa privada. Construido con Blazor Server, MudBlazor y animaciones GSAP.

---

## 📋 Tabla de Contenidos

- [Características](#-características)
- [Tecnologías](#-tecnologías)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación](#-instalación)
- [Configuración](#-configuración)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Guía de Desarrollo](#-guía-de-desarrollo)
- [Componentes Principales](#-componentes-principales)
- [Estilos y Animaciones](#-estilos-y-animaciones)
- [Autenticación](#-autenticación)
- [Deployment](#-deployment)
- [Contribución](#-contribución)
- [Licencia](#-licencia)

---

## ✨ Características

### Funcionalidades Principales
- 🎯 **Sistema de Reservas en Tiempo Real** - Selección de fechas y gestión de huéspedes
- 🎨 **Diseño Moderno y Responsivo** - Optimizado para móvil, tablet y desktop
- 🔐 **Autenticación Segura** - Integración con Keycloak (OpenID Connect)
- 🌊 **Hero Animado** - Carousel de imágenes con efectos parallax
- 📸 **Galería Lightbox** - PhotoSwipe para visualización de imágenes
- 🎭 **Animaciones Fluidas** - GSAP y AOS para transiciones suaves
- 📱 **Mobile-First** - Interfaz adaptativa con navegación optimizada
- 🎨 **Material Design** - UI consistente con MudBlazor

### Secciones del Sitio
- Home con hero animado
- Servicios y amenidades
- Galería de fotos
- Reseñas de clientes
- FAQ
- Footer con información de contacto

---

## 🛠️ Tecnologías

### Backend & Framework
- **.NET 10** - Framework principal
- **Blazor Server** - Renderizado interactivo del lado del servidor
- **C# 13** - Lenguaje de programación

### Frontend & UI
- **MudBlazor 8.15.0** - Componentes Material Design
- **CodeBeam.MudBlazor.Extensions 8.3.0** - Extensiones adicionales
- **Bootstrap 5** - Sistema de grid y utilidades

### Animaciones & JavaScript
- **GSAP 3.12.5** - Animaciones y ScrollTrigger
- **AOS 2.3.1** - Animate On Scroll
- **PhotoSwipe 5** - Lightbox para galería

### Autenticación
- **OpenID Connect** - Protocolo de autenticación
- **Keycloak** - Identity Provider
- **Cookie Authentication** - Gestión de sesiones

### Estilos
- **CSS3** - Estilos personalizados
- **Google Fonts (Poppins)** - Tipografía
- **Material Icons** - Iconografía

---

## 📦 Requisitos Previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) o superior
- [Visual Studio 2025](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)
- [Node.js](https://nodejs.org/) (opcional, para herramientas de desarrollo)
- **Keycloak Server** configurado (para autenticación)

---

## 🚀 Instalación

### 1. Clonar el Repositorio

```bash
git clone https://github.com/tu-organizacion/gcg-web-booking.git
cd gcg-web-booking
```

### 2. Restaurar Dependencias

```bash
cd GCG.Web.Booking
dotnet restore
```

### 3. Configurar Variables de Entorno

Edita `appsettings.json` o crea `appsettings.Development.json`:

```json
{
  "Authentication": {
    "Schemes": {
      "OpenIdConnect": {
        "Authority": "http://tu-keycloak:8080/realms/tu-realm",
        "ClientId": "tu-client-id",
        "ClientSecret": "tu-client-secret"
      }
    }
  },
  "BookingApi": {
    "BaseUrl": "https://tu-api-url/api/v1"
  }
}
```

### 4. Ejecutar la Aplicación

```bash
dotnet run
```

O presiona **F5** en Visual Studio.

La aplicación estará disponible en:
- HTTPS: `https://localhost:7129`
- HTTP: `http://localhost:5000`

---

## ⚙️ Configuración

### Configuración de Keycloak

1. **Crear Realm** en Keycloak: `gcg-booking-dev`
2. **Crear Client**: `booking-web`
   - Client Protocol: `openid-connect`
   - Access Type: `confidential`
   - Valid Redirect URIs: `https://localhost:7129/signin-oidc`
3. **Copiar Client Secret** y actualizar `appsettings.json`

### Configuración de Cultura

Por defecto, la aplicación usa `InvariantCulture`. Para cambiar:

```csharp
// Program.cs
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-MX");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("es-MX");
```

### Variables de Entorno

| Variable | Descripción | Valor por Defecto |
|----------|-------------|-------------------|
| `ASPNETCORE_ENVIRONMENT` | Entorno de ejecución | `Development` |
| `ASPNETCORE_URLS` | URLs de escucha | `https://localhost:7129` |
| `Authentication__Schemes__OpenIdConnect__Authority` | URL de Keycloak | - |
| `BookingApi__BaseUrl` | URL de la API de reservas | - |

---

## 📁 Estructura del Proyecto

```
GCG.Web.Booking/
├── Components/
│   ├── Layout/
│   │   ├── AppBar.razor              # Barra de navegación principal
│   │   ├── MainLayout.razor          # Layout principal de la app
│   │   ├── NavMenu.razor             # Menú de navegación
│   │   ├── SidebarDesktop.razor      # Sidebar para admin (desktop)
│   │   ├── SidebarMobile.razor       # Sidebar móvil
│   │   └── UserMenu.razor            # Menú de usuario
│   ├── Pages/
│   │   ├── Home.razor                # Página principal
│   │   ├── Error.razor               # Página de error
│   │   └── NotFound.razor            # Página 404
│   ├── Sections/
│   │   ├── Hero.razor                # Hero con carousel animado
│   │   ├── CallToAction.razor        # Barra de reservas flotante
│   │   └── Footer.razor              # Footer del sitio
│   ├── App.razor                     # Root component
│   └── _Imports.razor                # Importaciones globales
├── Models/
│   ├── Service.cs                    # Modelo de servicio
│   └── ServiceCategory.cs            # Categoría de servicio
├── Utils/
│   ├── Constants.cs                  # Constantes de la app
│   └── DateExtensions.cs             # Extensiones de fecha
├── wwwroot/
│   ├── css/
│   │   └── app.css                   # Estilos principales
│   ├── javascript/
│   │   ├── app.js                    # Scripts de AppBar y navegación
│   │   └── anim.js                   # Animaciones GSAP y AOS
│   ├── images/                       # Recursos de imágenes
│   └── lib/                          # Librerías de terceros
├── appsettings.json                  # Configuración principal
├── appsettings.Development.json      # Config de desarrollo
├── Program.cs                        # Punto de entrada
└── GCG.Web.Booking.csproj           # Archivo de proyecto
```

---

## 👨‍💻 Guía de Desarrollo

### Agregar un Nuevo Componente

```bash
# Desde la carpeta Components/
dotnet new razorcomponent -n MiComponente -o Sections
```

```razor
@* Sections/MiComponente.razor *@
<div class="mi-componente">
    <MudText Typo="Typo.h4">@Titulo</MudText>
</div>

@code {
    [Parameter] public string Titulo { get; set; } = "Título por defecto";
}
```

### Agregar Animaciones GSAP

```razor
@* En tu componente *@
<div class="gsap-elemento" data-aos="fade-up">
    Contenido animado
</div>

@code {
    [Inject] private IJSRuntime JS { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("animInitGSAP");
        }
    }
}
```

### Agregar Nuevos Estilos

```css
/* wwwroot/css/app.css */
.mi-nueva-seccion {
    padding: 3rem 1rem;
    background: linear-gradient(to bottom, #023047, #219ebc);
}

@media (max-width: 768px) {
    .mi-nueva-seccion {
        padding: 2rem 0.5rem;
    }
}
```

---

## 🎨 Componentes Principales

### AppBar
Barra de navegación fija con scroll hide/show.

```razor
<AppBar IsAdmin="@IsAdmin" />
```

**Props:**
- `IsAdmin` (bool): Muestra opciones de administrador

### Hero
Sección hero con carousel animado y parallax.

```razor
<Hero />
```

**Características:**
- Carousel automático (5s)
- Overlay con gradiente
- Animaciones GSAP en scroll
- Responsive para móvil

### CallToAction
Barra de reservas flotante con date picker y selector de huéspedes.

```razor
<CallToAction />
```

**Características:**
- Date Range Picker (MudBlazor)
- Selector de adultos/niños
- Input de código promocional
- Botón de reserva
- Responsive con toggle móvil

---

## 🎭 Estilos y Animaciones

### Sistema de Colores (Theme)

```csharp
Primary = "#219ebc"      // Azul principal
Secondary = "#ffb703"    // Amarillo/Dorado
AppbarBackground = "#023047"  // Azul oscuro
Surface = "#ffffff"      // Blanco
Background = "#f8f9fa"   // Gris claro
```

### Breakpoints Responsivos

```css
/* Mobile First */
@media (min-width: 768px) { /* Tablet */ }
@media (min-width: 960px) { /* Desktop */ }
@media (max-width: 600px) { /* Mobile */ }
```

### Animaciones Disponibles

#### AOS (Animate On Scroll)
```html
<div data-aos="fade-up" data-aos-duration="600">
```

Efectos: `fade`, `fade-up`, `fade-down`, `zoom-in`, `slide-up`

#### GSAP + ScrollTrigger
```javascript
// Parallax en Hero
gsap.to(".carousel", {
    scale: 1.06,
    scrollTrigger: { trigger: ".hero-section", scrub: true }
});
```

### Clases de Utilidad CSS

```css
.hide-on-mobile      /* Oculta en < 768px */
.show-on-mobile      /* Muestra solo en < 768px */
.hide-on-mobile-only /* Oculta solo en < 767px */
```

---

## 🔐 Autenticación

### Flujo de Autenticación

1. **Usuario no autenticado** → Redirige a `/login`
2. **Login** → Challenge a Keycloak (OpenID Connect)
3. **Keycloak autentica** → Callback a `/signin-oidc`
4. **Cookie creada** → Usuario autenticado
5. **Logout** → Redirige a Keycloak logout → `/signout-callback-oidc`

### Proteger una Página

```razor
@page "/admin"
@attribute [Authorize(Roles = "Admin")]

<h3>Página de Administración</h3>
```

### Obtener Usuario Actual

```razor
@inject AuthenticationStateProvider AuthStateProvider

@code {
    protected override async Task OnInitializedAsync()
    {
        var state = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = state.User;
        var isAdmin = user.IsInRole("Admin");
    }
}
```

---

## 🚢 Deployment

### Publicación para Producción

```bash
dotnet publish -c Release -o ./publish
```

### Docker (Ejemplo)

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["GCG.Web.Booking/GCG.Web.Booking.csproj", "GCG.Web.Booking/"]
RUN dotnet restore "GCG.Web.Booking/GCG.Web.Booking.csproj"
COPY . .
WORKDIR "/src/GCG.Web.Booking"
RUN dotnet build "GCG.Web.Booking.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GCG.Web.Booking.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GCG.Web.Booking.dll"]
```

### Variables de Entorno en Producción

```bash
export ASPNETCORE_ENVIRONMENT=Production
export Authentication__Schemes__OpenIdConnect__Authority=https://keycloak.tudominio.com/realms/gcg-booking
export Authentication__Schemes__OpenIdConnect__ClientSecret=tu-secret-seguro
export BookingApi__BaseUrl=https://api.tudominio.com/api/v1
```

---

## 🤝 Contribución

### Flujo de Trabajo

1. Fork el repositorio
2. Crea una rama feature: `git checkout -b feature/nueva-funcionalidad`
3. Commit tus cambios: `git commit -m 'feat: agregar nueva funcionalidad'`
4. Push a la rama: `git push origin feature/nueva-funcionalidad`
5. Abre un Pull Request

### Convenciones de Código

- **C#**: Seguir [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- **CSS**: BEM naming convention
- **JavaScript**: ESLint + Prettier
- **Commits**: [Conventional Commits](https://www.conventionalcommits.org/)

### Code Review

Todo PR debe:
- ✅ Pasar el build sin errores
- ✅ Incluir tests (cuando aplique)
- ✅ Documentación actualizada
- ✅ Code review aprobado por al menos 1 desarrollador

---

## 📝 Notas Técnicas

### Blazor Server vs Blazor WebAssembly

Este proyecto usa **Blazor Server** por:
- ✅ Mejor rendimiento inicial
- ✅ Acceso directo a recursos del servidor
- ✅ Menor tamaño de descarga
- ✅ SEO más sencillo

### Optimizaciones Implementadas

1. **Lazy Loading** de imágenes con `loading="lazy"`
2. **Preload** de imagen principal del hero
3. **CSS Minificado** en producción
4. **Bundling** de scripts con `defer`
5. **Will-change** para elementos animados

### Performance

- **Lighthouse Score**: 95+ (Desktop)
- **First Contentful Paint**: < 1.5s
- **Time to Interactive**: < 3s

---

## 🐛 Troubleshooting

### Error: "Cannot find module 'photoswipe'"
**Solución**: Las librerías se cargan desde CDN. Verifica tu conexión a internet.

### Error: "Authentication failed"
**Solución**: Verifica que Keycloak esté corriendo y la configuración en `appsettings.json` sea correcta.

### Las animaciones no funcionan
**Solución**: Asegúrate de que GSAP y AOS estén cargados correctamente. Abre la consola del navegador.

---

## 📄 Licencia

© 2025 **Global Code Group**. Todos los derechos reservados.

Este proyecto es propiedad de Global Code Group y está protegido por leyes de propiedad intelectual.

---

## 📧 Contacto

- **Website**: [globalcodegroup.com](https://globalcodegroup.com)
- **Email**: info@globalcodegroup.com
- **Cliente**: Refugio del Sol

---

## 🙏 Agradecimientos

- [MudBlazor](https://mudblazor.com/) - Por los excelentes componentes
- [GSAP](https://greensock.com/gsap/) - Por las animaciones fluidas
- [PhotoSwipe](https://photoswipe.com/) - Por la galería lightbox
- [AOS](https://michalsnik.github.io/aos/) - Por las animaciones on scroll

---

**Desarrollado con ❤️ por Global Code Group**
