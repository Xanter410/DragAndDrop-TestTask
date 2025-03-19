# DragAndDrop-TestTask
Выполненное тестовое задание – DragAndDrop

- Unity 6 (6000.0.33f1)
- Входная точка - Bootstrap
- Настройка зависимостей - ServiceLocator.  
  *Полноценный DI контейнер не стал использовать из-за отсутствия в его необходимости и исключения лишних зависимостей.*
- Для реализации Tween эффектов был использован плагин DOTween (v1.2.765)

---

### Модули:  
1. Ввод - Input  
2. Физика - Physics  
3. Механики курсора - Cursor:  
  3.1. Логика и рендер DragAndDrop механики  
  3.2. Логика перемещение камеры:  
    - На основе удержания и перемещения курсора   
    - На основе приближения Drag объекта к краю экрана  
4. Объекты - Object  
5. Утилиты - Utils  

---

### Общее описание логики работы:

Общая идея: 
- Используем `Polygon Collider2D` для выделения поверхностей куда можно будет поставить объекты. 
- У всех Draggable объектов делаем свой `Collider2D` для вычисления границ объекта, что полезно для коррекции положения на поверхности.
- Для вычисления физики используем `Raycast` и перемещаем с использованием DOTween.
- Для имитации глубины используем `GameObject.transform.localScale`, который зависит от координаты Y объекта.

---

### Более точное описание:

Объекты которые можно переносить:  
1. Находятся на Layer: DraggableObjects.  
2. Содержат компонент `Collider2D`. Используется для модуля Physics.  
3. Содержат MonoBehavior скрипт `ObjectItem`. Содержит данные объекта (базовый Scale), а также настраивает свой scale в зависимости от глубины расположения на старте.  

Имитация физики:  
1. Рассчитывает точку приземления используя `Physics2D.Raycast` и `Collider2D` поверхности. Корректирует положение по X координате, чтобы объект полностью помещался в границы коллайдера поверхности, а не стоял на краю.  
2. Перемещает объект в точку приземления с использованием секвенции DOTween (имитирует свободное падение).  

Взаимодействие пользователя со сценой:  
1. Логика сосредоточена в модуле Cursor. Отдельные классы *(DragAndDrop и CameraMove)* подписываются на сигналы от `IInput` из Input модуля.  
2. Когда пользователь начинает удержание проверяется находится ли под курсором объект который можно перемещать:  
    - Если да, то объект каждый кадр перемещается в положение курсора.  
    - Если нет, то начинается логика перемещение камеры.  
3. Когда пользователь заканчивает удержание:  
    - Если перемещался объект, то вызывает физика и объект управляется ей (падает).  
    - Если перемещалась камера, то передвижение заканчивается.  
4. В процессе захвата объекта и его отпускания его масштабом управляет `DragAndDropRenderer`.

---
