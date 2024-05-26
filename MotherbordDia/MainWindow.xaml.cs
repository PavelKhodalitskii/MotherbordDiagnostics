using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MotherbordDia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Button selectedButton = null;
        public SolidColorBrush selectedColor = new SolidColorBrush(Color.FromRgb(0, 212, 255));
        public SolidColorBrush defaultColor = new SolidColorBrush(Colors.LightGray);
        public Dictionary<Button, int> brokenElements = new Dictionary<Button, int>();

        public MainWindow()
        {
            InitializeComponent();

            selectedInstrumentText.Text = "Инструмет не выбран";

            Random rnd = new Random();

            brokenElements[Soket] = rnd.Next(0, 2);
            brokenElements[RAM] = rnd.Next(0, 2);
            brokenElements[Line33] = rnd.Next(0, 3);
            brokenElements[Line5] = rnd.Next(0, 3);
            brokenElements[Line12] = rnd.Next(0, 3);
            brokenElements[Usb] = rnd.Next(0, 2);
            brokenElements[PCIe] = rnd.Next(0, 2);
            brokenElements[Bios] = rnd.Next(0, 2);
            brokenElements[Battery] = rnd.Next(0, 2);

            foreach (KeyValuePair<Button, int> entry in brokenElements)
            {
                Console.WriteLine(entry.Value);
            }

            questionText.Visibility = Visibility.Hidden;

            Voltage.Visibility = Visibility.Hidden;
            Oms.Visibility = Visibility.Hidden;

            Multimetr.Click += Multimetr_Click;
            Voltage.Click += Voltage_Click;
            Oms.Click += Oms_Click;

            Oscilloscope.Click += SelectInstrument_Click;
            PciTester.Click += SelectInstrument_Click;
            RamTester.Click += SelectInstrument_Click;
            SoketTester.Click += SelectInstrument_Click;

            Soket.Click += Socket_Click;
            Line33.Click += Line33_Click;
            Line5.Click += Line5_Click;
            Line12.Click += Line12_Click;
            Usb.Click += Usb_Click;
            Battery.Click += BiosBattery_Click;
            PCIe.Click += PCI_Click;
            Bios.Click += Bios_Click;
            RAM.Click += RAM_Click;
        }

        private void Multimetr_Click(object sender, RoutedEventArgs e)
        {
            if (selectedButton != Voltage && selectedButton != Oms)
            {
                if (selectedButton != null)
                {
                    selectedButton.Background = defaultColor;
                }
                Voltage.Visibility = Visibility.Visible;
                Voltage.Background = selectedColor;
                Oms.Visibility = Visibility.Visible;
                selectedButton = Voltage;
                selectedInstrumentText.Text = "Выбран вольтметр";
            }
            else
            {
                Multimetr.Background = defaultColor;
                Voltage.Visibility = Visibility.Hidden;
                Oms.Visibility = Visibility.Hidden;
                Voltage.Background = defaultColor;
                Oms.Background = defaultColor;
                selectedButton = null;
                selectedInstrumentText.Text = "Инструмент не выбран";
            }
        }

        private void Voltage_Click(object sender, RoutedEventArgs e)
        {
            Oms.Background = defaultColor;
            Voltage.Background = selectedColor;
            selectedButton = Voltage;
            selectedInstrumentText.Text = "Выбран вольтметр";
        }

        private void Oms_Click(object sender, RoutedEventArgs e)
        {
            Voltage.Background = defaultColor;
            Oms.Background = selectedColor;
            selectedButton = Oms;
            selectedInstrumentText.Text = "Выбран омметр";
        }

        private void SelectInstrument_Click(object sender, RoutedEventArgs e)
        {
            if (selectedButton != null)
            {
                selectedButton.Background = defaultColor;
            }
            Button selected = (Button)sender;
            selected.Background = selectedColor;
            selectedButton = selected;
            selectedInstrumentText.Text = "Выбран " + selected.Content;

            questionText.Visibility = Visibility.Hidden;
            var yesButton = MainGrid.Children.OfType<Button>().FirstOrDefault(b => b.Name == "yesButton");
            var noButton = MainGrid.Children.OfType<Button>().FirstOrDefault(b => b.Name == "noButton");
            MainGrid.Children.Remove(yesButton);
            MainGrid.Children.Remove(noButton);
        }

        private void Socket_Click(object sender, RoutedEventArgs e)
        {
            if (selectedButton == SoketTester)
            {
                questionText.Visibility = Visibility.Visible;
                questionText.Text = "Тестирование сокета";
                if (this.brokenElements[Soket] == 1)
                {
                    questionText.Text = "Не все индикаторы тестера горят красным. Устройство исправно?";
                    CreateYesAndNoButtons(50, false, true);
                }
                else
                {
                    questionText.Text = "Все индикаторы тестера горят красным. Устройство исправно?";
                    CreateYesAndNoButtons(50, true, false);
                }
            }
            else
            {
                questionText.Text = "Выбран неправильный инструмент";
            }
        }

        private void Line33_Click(object sender, RoutedEventArgs e)
        {
            LineProcess(3.3, (Button)sender);
        }

        private void Line5_Click(object sender, RoutedEventArgs e)
        {
            LineProcess(5, (Button)sender);
        }

        private void Line12_Click(object sender, RoutedEventArgs e)
        {
            LineProcess(12, (Button)sender);
        }

        private void LineProcess(double line_voltage, Button sender)
        {
            Random rnd = new Random();
            questionText.Visibility = Visibility.Visible;
            if (selectedButton == Voltage)
            {
                if (this.brokenElements[sender] == 1)
                {
                    questionText.Text = string.Format("Напяжение на линии {0}V: {1} Вольт. Исправен ли элемент?", line_voltage, line_voltage - rnd.NextDouble() * rnd.Next(3, (int)line_voltage));
                    CreateYesAndNoButtons(50, false, true);
                }
                else if (this.brokenElements[sender] == 0 || this.brokenElements[sender] == 2)
                {
                    questionText.Text = string.Format("Напяжение на линии {0}V: {1} Вольт. Исправен ли элемент?", line_voltage, line_voltage + rnd.NextDouble() / 10);
                    CreateYesAndNoButtons(50, true, false);
                }
            }
            else if (selectedButton == Oms)
            {
                if (this.brokenElements[sender] == 2)
                {
                    questionText.Text = string.Format("Сопротивление на линии {0}V: 64 Ом. Исправен ли элемент?", line_voltage);
                    CreateYesAndNoButtons(50, false, true);
                }
                else if (this.brokenElements[sender] == 0 || this.brokenElements[sender] == 1)
                {
                    questionText.Text = string.Format("Сопротивление на линии {0}V: 3 кОм. Исправен ли элемент?", line_voltage);
                    CreateYesAndNoButtons(50, true, false);
                }
            }
            else
            {
                questionText.Text = "Выбран неправильный инструмент";
            }
        }

        private void Usb_Click(object sender, RoutedEventArgs e)
        {
            if (selectedButton == Voltage)
            {
                questionText.Visibility = Visibility.Visible;
                if (this.brokenElements[Usb] == 1)
                {
                    questionText.Text = "Красный щуп поставили на GND, а черным пройшлись по линиям D- D+ всех USB портов, показатели отличаются на сотни мВ. Сломан ли южный мост?";
                    CreateYesAndNoButtons(50, true, true);
                }
                else
                {
                    questionText.Text = "Красный поставили щуп на GND, а черным пройшлись по линиям D- D+ всех USB портов, показатели находиятся диапазоне от 0.450мВ до 0.7мВ. Сломан ли южный мост?";
                    CreateYesAndNoButtons(50, false, false);
                }
            }
            else
            {
                questionText.Text = "Выбран неправильный инструмент";
            }
        }

        private void RAM_Click(object sender, RoutedEventArgs e)
        {
            if (selectedButton == RamTester)
            {
                questionText.Visibility = Visibility.Visible;
                if (this.brokenElements[Usb] == 1)
                {
                    questionText.Text = "Все индикаторы тестера горят красным! Правильно ли работает этот элемент?";
                    CreateYesAndNoButtons(50, true, true);
                }
                else
                {
                    questionText.Text = "Ни один из индикаторов не горит! Правильно ли работает этот элемент?";
                    CreateYesAndNoButtons(50, false, false);
                }
            }
            else
            {
                questionText.Text = "Выбран неправильный инструмент";
            }
        }

        private void Bios_Click(object sender, RoutedEventArgs e)
        {
            if (selectedButton == Oscilloscope)
            {
                questionText.Visibility = Visibility.Visible;
                if (this.brokenElements[Bios] == 1)
                {
                    questionText.Text = "На осцилографе вы наблюдаете следующую картину: видно сингалы на ножках input/output микросхемы BIOS. Исправен ли BIOS?";
                    CreateYesAndNoButtons(50, true, true);
                }
                else
                {
                    questionText.Text = "На осцилографе вы наблюдаете следующую картину: отсутствуют сигналы (импульсы) на ножках input/output микросхемы BIOS. Исправен ли BIOS?";
                    CreateYesAndNoButtons(50, false, false);
                }
            }
            else
            {
                questionText.Text = "Выбран неправильный инструмент";
            }
        }

        private void PCI_Click(object sender, RoutedEventArgs e)
        {
            if (selectedButton == PciTester)
            {
                questionText.Visibility = Visibility.Visible;
                if (this.brokenElements[PCIe] == 1)
                {
                    questionText.Text = "На POST карте наблюдается отсутсвие кода. Может ли это сигнализировать о поломке материнской платы?";
                    CreateYesAndNoButtons(50, true, true);
                }
                else
                {
                    questionText.Text = "На POST карте наблюдается код 00. Сигнализирует ли это о поломке материнской платы?";
                    CreateYesAndNoButtons(50, false, false);
                }
            }
            else
            {
                questionText.Text = "Выбран неправильный инструмент";
            }
        }

        private void BiosBattery_Click(object sender, RoutedEventArgs e)
        {
            if (selectedButton == Voltage)
            {
                questionText.Visibility = Visibility.Visible;
                if (this.brokenElements[Usb] == 1)
                {
                    questionText.Text = "Напряжение на батарейке BIOS: 0,5 Вольта. Правильно ли работает этот элемент?";
                    CreateYesAndNoButtons(50, false, true);
                }
                else
                {
                    questionText.Text = "Напряжение на батарейке BIOS: 2.9 Вольта. Правильно ли работает этот элемент?";
                    CreateYesAndNoButtons(50, true, false);
                }
            }
            else
            {
                questionText.Text = "Выбран неправильный инструмент";
            }
        }

        private void SuccessAnswerIfBroken(object sender, RoutedEventArgs e)
        {
            questionText.Text = "Верный ответ! Найдена одна неисправность";
            var yesButton = MainGrid.Children.OfType<Button>().FirstOrDefault(b => b.Name == "yesButton");
            var noButton = MainGrid.Children.OfType<Button>().FirstOrDefault(b => b.Name == "noButton");
            MainGrid.Children.Remove(yesButton);
            MainGrid.Children.Remove(noButton);
            selectedInstrumentText.Text = "Я вообще то удалил";
        }

        private void SuccessAnswerIfWorks(object sender, RoutedEventArgs e)
        {
            questionText.Text = "Верный ответ!";
            var yesButton = MainGrid.Children.OfType<Button>().FirstOrDefault(b => b.Name == "yesButton");
            var noButton = MainGrid.Children.OfType<Button>().FirstOrDefault(b => b.Name == "noButton");
            MainGrid.Children.Remove(yesButton);
            MainGrid.Children.Remove(noButton);
            selectedInstrumentText.Text = "Я вообще то удалил";
        }

        private void UnsuccessAnswer(object sender, RoutedEventArgs e)
        {
            questionText.Text = "Неверный ответ!";
            var yesButton = MainGrid.Children.OfType<Button>().FirstOrDefault(b => b.Name == "yesButton");
            var noButton = MainGrid.Children.OfType<Button>().FirstOrDefault(b => b.Name == "noButton");
            MainGrid.Children.Remove(yesButton);
            MainGrid.Children.Remove(noButton);
            selectedInstrumentText.Text = "Я вообще то удалил";
        }

        private void CreateYesAndNoButtons(int offset, bool yesIsRight, bool isBroken)
        {
            Button yesButton = new Button { Content = "Да", Height = 30, Width = 50 };
            Button noButton = new Button { Content = "Нет", Height = 30, Width = 50 };


            yesButton.HorizontalAlignment = HorizontalAlignment.Right;
            yesButton.VerticalAlignment = VerticalAlignment.Top;
            noButton.HorizontalAlignment = HorizontalAlignment.Right;
            noButton.VerticalAlignment = VerticalAlignment.Top;

            yesButton.Name = "yesButton";
            noButton.Name = "noButton";       
            yesButton.Margin = new Thickness(questionText.Margin.Left, questionText.Margin.Top + offset, 150, questionText.Margin.Bottom);
            noButton.Margin = new Thickness(questionText.Margin.Left, questionText.Margin.Top + offset, 85, questionText.Margin.Bottom);

            if (yesIsRight)
            {
                if (isBroken)
                {
                    yesButton.Click += SuccessAnswerIfBroken;
                }
                else
                {
                    yesButton.Click += SuccessAnswerIfWorks;
                }
                noButton.Click += UnsuccessAnswer;
            }
            else
            {
                if (isBroken)
                {
                    noButton.Click += SuccessAnswerIfBroken;
                }
                else
                {
                    noButton.Click += SuccessAnswerIfWorks;
                }
                yesButton.Click += UnsuccessAnswer;
            }
            
            MainGrid.Children.Add(yesButton);
            MainGrid.Children.Add(noButton);                        
        }
    }
}
