# 🏰 Dungeon RPG

Un juego de rol por turnos desarrollado en **C#** con .NET 9.0. Explora un dungeon generado procedimentalmente, derrota enemigos, recolecta loot épico y sobrevive a los jefes finales.

## ✨ Características

- 🎲 **Generación Procedural**: 12 salas únicas (4 pisos × 3 plantas) generadas automáticamente
- ⚔️ **Combate Táctico**: Sistema de turnos con golpes críticos, habilidades y defensa
- 🧙 **Clases Especializadas**: Guerrero, Mago, Pícaro y Paladín con habilidades únicas
- 💎 **Sistema de Items**: Armas, armaduras, pociones y trinkets con 6 niveles de rareza
- 🎁 **Loot Dinámico**: Recompensas basadas en tipo de enemigo (mejores de jefes)
- ⚡ **Eventos Especiales**: Encuentros aleatorios que modifican la batalla
- 📊 **Estadísticas**: Seguimiento de enemigos derrotados, daño total e items recolectados

## 🛠️ Requisitos

- **.NET 9.0** o superior
- **Visual Studio 2022**, **VS Code** o cualquier IDE compatible con .NET

## 📦 Instalación

1. **Clonar o descargar el repositorio**:
```bash
git clone <url-del-repositorio>
cd Dungeon
```

2. **Compilar el proyecto**:
```bash
dotnet build
```

3. **Ejecutar el juego**:
```bash
dotnet run --project Juego/Program.cs
```

O directamente:
```bash
dotnet run
```

## 🎮 Cómo Jugar

### Menú Principal
```
1. Crear/Seleccionar personaje
2. Entrar en la mazmorra
3. Ver inventario
0. Salir
```

### En el Dungeon
- **Atacar**: Combate melee con posibilidad de crítico (×2 daño)
- **Usar Habilidades**: Especiales de tu clase con costo de mana
- **Pociones**: Recupera vida o mana en momentos críticos
- **Huir**: Abandona la sala sin derrotar enemigos

### Progresión
- Completa 3 plantas por piso (cada planta 3 = jefe)
- Al completar un piso: recuperas 25% vida + 50% mana
- Total: **4 pisos × 3 plantas = 12 salas**

## 📂 Estructura del Proyecto

```
Dungeon/
├── 📁 Armaduras/           # Modelos y datos de armaduras
│   ├── Armadura.cs
│   ├── armaduras.json
│   └── ArmadurasRoot.cs
├── 📁 Armas/               # Modelos y datos de armas
│   ├── Arma.cs
│   ├── armas.json
│   └── ArmasRoot.cs
├── 📁 Combate/             # Sistema de combate
│   ├── Combate.cs          # Loop de combate turno a turno
│   ├── CalculoDaño.cs      # Cálculos de daño y defensa
│   ├── Inventario.cs       # Gestión del inventario
│   └── Loot.cs             # Sistema de recompensas
├── 📁 Enemigos/            # Modelos y datos de enemigos
│   ├── Enemigo.cs
│   ├── enemigos.json
│   └── EnemigosRoot.cs
├── 📁 Estadisticas/        # Panel de estadísticas
│   └── Estadisticas.cs
├── 📁 Eventos/             # Sistema de eventos especiales
│   ├── Evento.cs
│   ├── eventos.json
│   └── SistemaEventos.cs
├── 📁 Juego/               # Punto de entrada
│   └── Program.cs          # Menú principal y loop del juego
├── 📁 Personajes/          # Modelos y sistemas de personaje
│   ├── Clase.cs            # Definición de clases
│   ├── clases.json
│   ├── Personaje.cs        # Clase del jugador
│   ├── Habilidad.cs        # Habilidades especiales
│   ├── Funciones.cs        # Selección de clase
│   ├── GenerarHabilidades.cs
│   └── ClasesRoot.cs
├── 📁 Pociones/            # Modelos y datos de pociones
│   ├── Pocion.cs
│   ├── pociones.json
│   └── PocionesRoot.cs
├── 📁 Salas/               # Sistema de salas y dungeon
│   ├── Sala.cs             # Estructura de una sala
│   ├── Funciones.cs        # Presentación de salas
│   └── SalasGenerator.cs   # Generador procedural
├── 📁 Trinkets/            # Modelos y datos de trinkets
│   ├── Trinket.cs
│   ├── trinkets.json
│   └── TrinketsRoot.cs
├── Dungeon.csproj          # Configuración del proyecto
├── Dungeon.sln             # Solución Visual Studio
└── README.md               # Este archivo
```

## ⚙️ Sistemas de Juego

### 🔄 Sistema de Combate
- **Turnos**: Personaje ataca → Enemigo contraataca → Siguiente turno
- **Daño Base**: Fuerza × 50 + Bonus de armas + Bonus de trinkets
- **Defensa**: Constitución ÷ 2 + Armaduras + Trinkets
- **Críticos**: 20% + bonus de trinkets = ×2 daño
- **Mana Pasivo**: Recupera 3 puntos por turno

### 💄 Cálculo de Daño
```
Daño Jugador = (Daño Base × Multiplicador - Defensa Enemigo) mín 1
Daño Enemigo = (Ataque Enemigo - Defensa Jugador) mín 1
Multiplicador = 2.0 si crítico, 1.3 si normal
```

### 🎪 Sistema de Eventos
Eventos especiales pueden ocurrir al entrar a una sala:
- **Daño**: Recibe daño base del evento
- **Vida**: Recupera puntos de vida
- **Mana**: Recupera puntos de mana
- **Tesoro**: Obtiene un item raro de la sala
- **Bendición**: Próximo ataque +50%
- **Mercader**: Encuentro especial (sin efecto)

### 💰 Sistema de Loot
- **Enemigos Normales**: Objetos Común/Poco Común/Raro
- **Jefes**: Objetos Épico/Legendario/Mítico
- **Cantidad**: 1-2 objetos de enemigos normales, 2-3 de jefes

### 💪 Sistema de Atributos
Cada clase tiene 4 atributos básicos:
- **Fuerza**: Aumenta daño de ataque
- **Inteligencia**: Aumenta poder de habilidades
- **Constitución**: Aumenta vida máxima y defensa
- **Agilidad**: (Base para críticos futuros)

## 👥 Clases Disponibles

### ⚔️ Guerrero
- **Vida**: Alta (120 HP)
- **Mana**: Bajo (30)
- **Especialidad**: Gran daño físico
- **Habilidad**: Golpe Poderoso (inflinge 2× daño)

### 🧙 Mago
- **Vida**: Baja (80 HP)
- **Mana**: Alto (60)
- **Especialidad**: Daño mágico y utilidad
- **Habilidad**: Bola de Fuego (inflinge daño aumentado)

### 🗡️ Pícaro
- **Vida**: Media (100 HP)
- **Mana**: Media (40)
- **Especialidad**: Críticos frecuentes
- **Habilidad**: Ataque Certero (mayor probabilidad de crítico)

### ⚪ Paladín
- **Vida**: Media-Alta (110 HP)
- **Mana**: Media (45)
- **Especialidad**: Defensa y curación
- **Habilidad**: Escudo Divino (aumenta defensa y recupera vida)

## 🐉 Enemigos y Jefes

### Estructura de Níveis
- **Piso 1**: Enemigos nivel 1 (vida 20-40)
- **Piso 2**: Enemigos nivel 2 (vida 40-60)
- **Piso 3**: Enemigos nivel 3 (vida 60-80)
- **Piso 4**: Enemigos nivel 4 (vida 80-100)

### Tipos de Enemigos
- **Subditos**: 3 por sala normal, 2 por sala jefe
- **Jefes**: 1 por sala jefe (planta 3 de cada piso)

### Ejemplos
- Goblin, Orco, Troll (variación por nivel)
- Dragón, Lich, Archmago (jefes)

## 💎 Items y Rareza

### Niveles de Rareza (de menor a mayor)
1. **Común** (blanco) - Base
2. **Poco Común** (verde) - 15% mejorado
3. **Rara** (azul) - 30% mejorado
4. **Épica** (púrpura) - 50% mejorado
5. **Legendaria** (naranja) - 80% mejorado
6. **Mítica** (dorado) - 100% mejorado

### Tipos de Items
- **Armas**: Aumentan daño (5-50 daño base)
- **Armaduras**: Aumentan defensa (2-20 defensa)
- **Pociones**: Restauran vida/mana (10-50 puntos)
- **Trinkets**: Efectos especiales (fuerza, defensa, crítico)

## 🔧 Desarrollo

### Compilación
```bash
dotnet build
```

### Compilación en Release
```bash
dotnet build -c Release
```

### Ejecución en Modo Debug
```bash
dotnet run -c Debug
```

### Ejecutar Tests (si existen)
```bash
dotnet test
```

### Limpiar Proyecto
```bash
dotnet clean
```