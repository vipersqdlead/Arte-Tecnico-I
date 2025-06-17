# 🎮 Arte Técnico I - Universidad SEK

Este repositorio contiene el proyecto de Unity utilizado para la clase **Arte Técnico I**, de la carrera **Diseño y Desarrollo de Videojuegos** de la **Universidad SEK**.

---

## 🚀 Flujo de Trabajo

Seguimos una estructura basada en **GitFlow**:

### 🔵 `main` branch

- 🔒 Es la rama principal del repositorio.
- 🚫 **No trabajes directamente sobre `main`.**
- ✅ Solo se actualiza con versiones estables y revisadas desde `develop`.

### 🟢 `develop` o `dev` branch

- 🛠️ Rama base para desarrollo diario.
- ✨ Todo trabajo debe partir desde esta rama.

---

## 💡 Convenciones de Proyecto

### 🧠 Nomenclatura

- **Carpetas**: `PascalCase` (`MyScripts`, `SceneManager`)
- **Archivos de C#**: `PascalCase` coincidente con la clase (`PlayerController.cs`)
- **Variables**: `camelCase` para privadas; `PascalCase` para publicas
- **Constantes**: `ALL_CAPS_WITH_UNDERSCORES`
- **Prefabs / Assets**: `[tipo]_[nombreCamel]` → `ps_explosion`, `ui_scorePanel`

---

### 🗂️ Jerarquía de Carpetas

Organiza tu trabajo dentro de `/Assets` de la siguiente manera:

    Assets/
    ├── ME*/ ← Tu desarrollo del momento evaluativo
    ├──├── Materials/ ← Los materiales creados para la evaluación
    ├──├── Scripts/ ← Scripts usados en la evaluación
    ├──├── Particle System/ ← Los prefabs de sistemas de particulas usados en la evaluación
    ├──├── Scripts/ ← Código fuente
    ├── Scenes/ ← Tus escenas específicas, NO usar SampleScene
    ├── Shaders/ ← Shaders y SubGraphs
    ├──├── SubGraphs/

> 📛 **Evita trabajar en `SampleScene.unity`**. Crea tu propia escena con nombre claro y temático: `ME1_Borrador`, `Clase_Fireball`, etc.

---

## ✅ Convenciones de Commits

Utilizamos [Conventional Commits](https://www.conventionalcommits.org):

| Tipo        | Descripción                               |
| ----------- | ----------------------------------------- |
| `feat:`     | Nueva funcionalidad                       |
| `fix:`      | Corrección de bugs                        |
| `docs:`     | Cambios en documentación                  |
| `style:`    | Cambios de formato, sin afectar lógica    |
| `refactor:` | Refactorización sin cambiar funcionalidad |
| `test:`     | Añadir o actualizar pruebas               |
| `chore:`    | Tareas menores (build, config, etc)       |

Por lo tanto a la hora de escribir un commit debemos señalar el tipo de trabajo realizado.

📝 Ejemplo:  
`feat: add player movement system` este commit indica que se agregó una nueva característica al proyecto, un sistema de movimiento de player.

---

## 🚫 Reglas y Buenas Prácticas

- ⛔ **No trabajes dentro de `SampleScene.unity`**.
- ⛔ **No hagas commit directo a `main`**.
- ❌ No dejes `Debug.Log` o código de prueba en producción.
- 🔌 No añadas paquetes sin comunicarlo.
- ⚙️ No cambies configuraciones globales sin consenso.
- 🧼 Evita nombres ofensivos o groseros en commits, variables o comentarios.

---

¿Dudas o sugerencias? Abre un issue o coméntalo en el canal del proyecto.
