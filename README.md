# IAV - Practica 4

## Autores
- Javier Villegas Montelongo ([Yavi123](https://github.com/Yavi123))
- Gonzalo Fernández Moreno ([GonzaloFdezMoreno](https://github.com/GonzaloFdezMoreno))
- Enrique Juan Gamboa ([ivo_hr](https://github.com/ivo-hr))

## Propuesta
Este proyecto es una práctica de la asignatura de Inteligencia Artificial para Videojuegos del Grado en Desarrollo de Videojuegos de la UCM, cuyo enunciado original es este: [Batalla en el Desierto](https://github.com/Narratech/IAV-Coordinacion).

La practica consiste en desarrollar una IA para prototipo de un juego RTS (Real Time Strategy), dentro de un entorno virtual que represente un campo de batalla con dos facciones cuyo objetivo es destruir a la otra. Cada facción puede ser controlada por el jugador o por una IA ya programada.

## Punto de partida
Se parte de un proyecto base de Unity 2021 proporcionado por el profesor y disponible en este repositorio: [IAV-Coordinación](https://github.com/Narratech/IAV-Coordinacion).

En el proyecto podemos ver dos facciones, cada una tiene su base principal, un edificio desde donde aparecen las unidades y dos "cosechadoras" ya creadas.
Las dos facciones son controladas por un GameManager que guarda el estado de la partida y contiene los métodos que deben llamar las IAs de cada facción.
Aparte de un script que permite controlar una facción al jugador, también podemos encontrar entre los assets unos scripts que controlan automáticamente una facción al añadirse estas mismas como controlador.

Cada facción puede crear una de las siguientes unidades, cada una con su coste diferente:
- Cosechadora: Esta unidad se dedica a ir 

## Diseño de la solución

Lo que vamos a realizar para resolver esta práctica es completar todos los scripts que hay en el proyecto sin completar, como el comportamiento del público para que huya cuand cae la lámpara correspondiente, y vuelva cuando la lámpara se encienda.

También habrá que completar los scripts que utiliza el árbol de comportamiento del fantasma para que tenga el comportamiento pedido

La cantante le queda por implementar los scripts que definen sus comportamientos que estan definidos por una máquina de estados

Público:
```mermaid
stateDiagram
    
    VerEspectáculo --> Salir : La lámpara cae
    Salir --> EsperarVestíbulo
    EsperarVestíbulo --> Entrar: La lámpara es recogida
    Entrar --> VerEspectáculo
   
```

Cantante:
```mermaid
stateDiagram
    
    Cantar --> Bambalinas 
    Bambalinas --> Cantar 
    Cantar --> Llevada : El fantasma la rapta 
    Bambalinas --> Llevada : El fantasma la rapta
    Llevada --> Volver : El fantasma/vizconde la suelta en zona reconocible
    Llevada --> Desorientada : El fantasma/vizconde la suelta en zona no reconocible
    Desorientada --> Llevada : El fantasma/vizconde la recoge
    Volver --> Cantar : Si estaba cantando
    Volver --> Bambalinas : Si no estaba cantando
   
```
Fantasma:

```mermaid
stateDiagram

    Buscar --> TirarLampara : Hay publico
    TirarLampara --> Capturar : Se va el publico
    Buscar --> Capturar : No hay publico
    Capturar --> LlevarCelda 
    LlevarCelda --> CerrarCelda : Llega a la celda
    LlevarCelda --> Buscar : El vizconde la salva
    CerrarCelda --> Buscar : El vizconde la salva
```

Para dibujar espacios de coordenadas 2D con puntos y vectores, se podría incrustar una imagen de Google Draw, o intentar incrustarlo en el repositorio también con Mermaid. 

## Pruebas y métricas

- [Vídeo con la batería de pruebas](https://youtu.be/7uuU-owzgjM)

## Ampliaciones

Se han realizado las siguientes ampliaciones :trollface:

## Producción

Las tareas se han realizado y el esfuerzo ha sido repartido entre los autores. Esto se podrá documentar en una tabla como esta o usando la [pestaña de Proyectos](https://github.com/orgs/Narratech/projects/4/views/1) de GitHub.

| Estado  |  Tarea  |  Fecha  |  
|:-:|:--|:-:|
| ✔ | Diseño: Primer borrador | 23-03-2023 |
| ✔ | Característica A: Mundo virtual, movimiento e interaccion con entorno | ... |
| ✔ | Característica B: Logica del público| ... |
| ✔ | Característica C: Maquina de estados cantante| ... |
| ✔ | Característica D: Arbol de comportamiento fantasma| ... |
| ✔ | Característica E: Sistema gestion sensorial fantasma| ... |
|  | OPCIONAL |  |
| :x: | Escenario complejo, portales | ... |
| :x: | Escenario mecanismos complejos | ... |
| :x: | Fantasma mas inteligente | ... |
| :x: | Mejor gestion sensorial | ... |
| :x: | Más personajes | ... |

## Referencias

Los recursos de terceros utilizados son de uso público.

- *AI for Games*, Ian Millington.
- [Kaykit Medieval Builder Pack](https://kaylousberg.itch.io/kaykit-medieval-builder-pack)
- [Kaykit Dungeon](https://kaylousberg.itch.io/kaykit-dungeon)
- [Kaykit Animations](https://kaylousberg.itch.io/kaykit-animations)
