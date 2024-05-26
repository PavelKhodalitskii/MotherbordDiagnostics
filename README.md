#Автор: Ходалицкий Павел Юрьевич (ИСИб-22-1 (ИрНИТУ))

# Приложение - симулятор диагностики материской платы.
Приложение написано на C# с использованием WPF. Программа иммитирует реальную диагностику в формате вопросов о наблюдаемых при опреденных дейтсвиях событиях.

## Описанные неисправности:
<ul>
  <li>Тестирование напряжения: Проверка уровней напряжения линий 3.3V, 5V и 12V</li>
  <li>Тестирование линий на кз: Проверка сопротивления линий 3.3V, 5V и 12V</li>
  <li>Тестирование слотов ОЗУ: Проверка состояния слотов ОЗУ.</li>
  <li>Тестирование на поломку южного моста. Промерка USB портов для проверки неисправности южного моста</li>
  <li>Тестирование сокета материнской платы</li>
  <li>Тестирование PSIe с помощью POST карты</li>
  <li>Тестирование микросхемы BIOS. Промерка ножек input/output с помощью осцилографа</li>
  <li>Тестирование напряжения на батарейке BIOS.</li>
</ul>

## Как использовать?
Очень просто.
1) Выберете инсрумент которым хотите воспользоваться в меню в верхней части экрана.
2) Выберете элемент, который хотите протестировать.
3) Сделайте вывод из полученных данных.

## Структура кода:
Основной код приложения содержится в двух файлах:
MainWindow.xaml - разметка интерфейса приложения.
MainWindow.xaml.cs - логика приложения. Обрабочтики всех кнопок. Содержит класс MainWindow(), который контролирует instance приложения.
