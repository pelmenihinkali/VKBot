using AnyBot.Components;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace NekitVKBot
{
    /// <summary>
    /// Логика взаимодействия для componentW.xaml
    /// </summary>
    public partial class componentW : Window
    {
        Component component;
        new Label Name;
        Label Description;
        CheckBox IsEnable;

        List<UIElement> uIElements = new List<UIElement>();
        const int Space = 30;
        public componentW()
        {
            InitializeComponent();

            //LoadComponents();

            Name = new Label();
            Name.Content = "Выбери компонент";
            Description = new Label();
            IsEnable = new CheckBox();
            Stack.Children.Add(Name);
            Stack.Children.Add(Description);
            Stack.Children.Add(IsEnable);
            Description.MinHeight = Space;
            IsEnable.Content = "Enable component";

            IsEnable.Visibility = Visibility.Hidden;
            IsEnable.FlowDirection = FlowDirection.RightToLeft;
            IsEnable.HorizontalAlignment = HorizontalAlignment.Left;
            IsEnable.MinHeight = Space;
            IsEnable.Margin = new Thickness(5, 0, 0, 0);
        }

        #region События формы
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var component in MainWindow.components)
            {
                List.Items.Add(component);
            }
        }
        private void List_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DeleteElements();
            component = (Component)List.SelectedItem;

            Name.Content = component.Name;
            Description.Content = component.Description;
            IsEnable.IsChecked = component.Enable;
            IsEnable.Unchecked += (sender, e) =>
            {
                component.Enable = false;
            };
            IsEnable.Checked += (sender, e) =>
            {
                component.Enable = true;
            };

            IsEnable.Visibility = Visibility.Visible;

            UpdateGrid();

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveComponents();
        }
        #endregion

        #region Обновление инспектора
        private void UpdateGrid()
        {
            List<string> allFieldsInBase = new List<string>();
            foreach (var item in typeof(Component).GetFields())
            {
                allFieldsInBase.Add(item.Name);
            }

            foreach (var Field in component.GetType().GetFields())
            {
                if (!allFieldsInBase.Contains(Field.Name))
                {
                    if (Field.FieldType == typeof(System.String))
                    {
                        CreateStringBox(Field);
                    }
                    if (Field.FieldType == typeof(System.Boolean))
                    {
                        CreateBoolBox(Field);
                    }
                    if (Field.FieldType == typeof(int))
                    {
                        CreateInt32Box(Field);
                    }
                    if (Field.FieldType == typeof(List<string>))
                    {
                        CreateListString(Field);
                    }
                }
            }
        }
        private void DeleteElements()
        {
            foreach (var element in uIElements)
            {
                Stack.Children.Remove(element);
            }
        }
        #endregion
        #region Элементы управления
        private void CreateBoolBox(FieldInfo Field)
        {
            CheckBox cb = new CheckBox();
            Stack.Children.Add(cb);
            cb.Content = Field.Name;
            cb.IsChecked = (bool)Field.GetValue(component);
            cb.FlowDirection = FlowDirection.RightToLeft;
            cb.HorizontalAlignment = HorizontalAlignment.Left;
            cb.MinHeight = Space;
            cb.Margin = new Thickness(5, 0, 0, 0);

            cb.Checked += (sender, e) => { Field.SetValue(component, true); };
            cb.Unchecked += (sender, e) => { Field.SetValue(component, false); };

            uIElements.Add(cb);
        }
        private void CreateStringBox(FieldInfo Field)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            Stack.Children.Add(sp);
            Label label = new Label();
            label.MinWidth = 100;
            label.Content = Field.Name;
            TextBox tb = new TextBox();
            tb.MinWidth = 200;
            tb.MinHeight = Space - 15;
            tb.Text = (string)Field.GetValue(component);
            tb.TextChanged += (sender, e) => { Field.SetValue(component, ((TextBox)sender).Text); };
            sp.Children.Add(label);
            sp.Children.Add(tb);

            uIElements.Add(sp);
        }
        private void CreateInt32Box(FieldInfo Field)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            Stack.Children.Add(sp);
            Label label = new Label();
            label.MinWidth = 100;
            label.Content = Field.Name;
            TextBox tb = new TextBox();
            tb.MinWidth = 200;
            tb.MaxLength = 9;
            tb.Text = ((System.Int32)Field.GetValue(component)).ToString();
            tb.PreviewTextInput += (sender, e) =>
            {
                if (!Char.IsDigit(e.Text, 0))
                {
                    e.Handled = true;
                }
            };
            tb.TextChanged += (sender, e) =>
            {
                if (tb.Text == "") tb.Text = "0";
                Field.SetValue(component, Convert.ToInt32(tb.Text));
            };
            sp.Children.Add(label);
            sp.Children.Add(tb);

            uIElements.Add(sp);
        }
        private void CreateListString(FieldInfo Field)
        {
            List<string> lists = (List<string>)Field.GetValue(component);
            StackPanel sp = new StackPanel();
            Stack.Children.Add(sp);
            {
                Label label = new Label();
                label.MinWidth = 100;
                label.Content = Field.Name;
                sp.Children.Add(label);
            }
            StackPanel list = new StackPanel();
            sp.Children.Add(list);
            int i = 0;
            foreach (var item in lists)
            {
                int index = i;
                stringtbforlist(Field, list, lists, item, index);
                i++;
            }
            StackPanel buttons = new StackPanel();

            Button create = new Button();
            create.Content = "Create";
            create.Click += (sender, e) =>
            {
                string newElement = "New element";
                lists.Add(newElement);
                stringtbforlist(Field, list, lists, newElement, lists.Count-1);
            };

            Button delete = new Button();
            delete.Content = "Remove";
            delete.Click += (sender, e) =>
            {
                lists.RemoveAt(lists.Count - 1);
                list.Children.RemoveAt(list.Children.Count-1);
            };

            buttons.Children.Add(delete);
            buttons.Children.Add(create);

            sp.Children.Add(buttons);
            uIElements.Add(sp);
        }
        private void stringtbforlist(FieldInfo Field, StackPanel list, List<string>lists, string item, int index)
        {
            StackPanel splist = new StackPanel();
            splist.Orientation = Orientation.Horizontal;
            list.Children.Add(splist);
            Label label = new Label();
            label.MinWidth = 100;
            label.Content = $"{index} - ";
            TextBox tb = new TextBox();
            tb.MinWidth = 200;
            tb.MinHeight = Space - 15;
            tb.Text = item;
            tb.TextChanged += (sender, e) => { lists[index] = tb.Text; Field.SetValue(component, lists); };
            splist.Children.Add(label);
            splist.Children.Add(tb);
        }
        #endregion

        #region Сохранение и загрузка полей сборок
        public static void SaveComponents()
        {
            XDocument xdoc = new XDocument();
            XElement cs = new XElement("Components");

            foreach (var item in MainWindow.components)
            {
                XElement c = new XElement("Component");
                XAttribute cNameType = new XAttribute("Name", item.GetType().FullName);


                foreach (var Field in item.GetType().GetFields())
                {
                    if (Field.FieldType == typeof(System.String))
                    {
                        XElement xfield = new XElement("Field", (string)Field.GetValue(item),
                            new XAttribute("Name", Field.Name),
                            new XAttribute("Type", typeof(System.String).FullName)
                            );
                        c.Add(xfield);
                    }
                    //if (Field.FieldType == typeof(System.Boolean))
                    //{
                    //    CreateBoolBox(Field);
                    //}
                    //if (Field.FieldType == typeof(int))
                    //{
                    //    CreateInt32Box(Field);
                    //}
                    //if (Field.FieldType == typeof(List<string>))
                    //{
                    //    CreateListString(Field);
                    //}

                }
                c.Add(cNameType);
                cs.Add(c);
            }

            xdoc.Add(cs);
            xdoc.Save("Components.xml");
        }
        public static void LoadComponents()
        {
            XDocument xdoc = XDocument.Load("Components.xml");
            foreach (XElement componentx in xdoc.Element("Components").Elements("Component"))
            {
                string nameAttribute = componentx.Attribute("Name").Value;
                foreach (var item in MainWindow.components)
                {
                    if(item.GetType().FullName == nameAttribute)
                    {
                        foreach (var field in componentx.Elements("Field"))
                        {
                            var asmField = item.GetType().GetField(field.Attribute("Name").Value);
                            if (asmField != null)
                            {
                                if(asmField.FieldType.FullName == "System.String")
                                {
                                    asmField.SetValue(item, field.Value);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

       
    }
}
