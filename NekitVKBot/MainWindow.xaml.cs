using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;

using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;
using System.Reflection;

using AnyBot.Components;
using AnyBot.System;
using AnyBotSystem;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Input;
using System.Threading.Tasks;

namespace NekitVKBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Settings
        internal static VKSettings vk;
        public Configure configure = new Configure();

        //Форма
        private Settings settings;
        private componentW componentF;
        #endregion

        private bool IsPaused = false;
        private Thread threadForBot;
        private ThreadStart threadStart;
        public static List<Component> components = new List<Component>();

        /// <summary>
        /// Сохранить данные пользователя.
        /// </summary>
        public static void SaveData()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("settings.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, vk);
            }
        }
        /// <summary>
        /// Загрузить данные пользователя.
        /// </summary>
        public static void LoadData()
        {
            if (File.Exists("settings.dat"))
            {
                using (FileStream fs = new FileStream("settings.dat", FileMode.OpenOrCreate))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    vk = (VKSettings)formatter.Deserialize(fs);
                }
            }
            else
            {
                vk = new VKSettings("token", 0, 25);
                SaveData();
            }
            vk.API = new VkApi();
        }

        public scoreSamgups ssTEST;
        /// <summary>
        /// Подключение сборок и конфигурация их.
        /// </summary>
        public void SetupComponents()
        {
            configure.API = vk.API;
            configure.sendToConsole = ConsoleWrite;

            string path = @$"{Environment.CurrentDirectory}\Modules";
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    if (new FileInfo(file).Extension == ".dll")
                    {
                        var asm = Assembly.LoadFile(file);
                        foreach (var t in asm.GetTypes())
                        {
                            if (t.BaseType == typeof(Component))
                            {
                                var instance = t.GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
                                components.Add((Component)instance);
                                ((AnyBot.Components.System)instance).Initialize(configure);
                                //foreach (var item in asm.GetReferencedAssemblies())
                                //{
                                //    MessageBox.Show(@$"{path}\{item.Name}.dll");
                                //    if (File.Exists(@$"{path}\{item.Name}.dll"))
                                //    {
                                //        Assembly.LoadFile(@$"{path}\{item.Name}.dll");
                                //    }
                                //    else
                                //    {
                                //        MessageBox.Show($"Для правильной работы модуля {((Component)instance).Name} нужно загрузить сборку {item.Name}");
                                //    }
                                //}

                            }
                        }
                    }
                }
            }

            var ss = new scoreSamgups()
            {
                web = web
            };
            ssTEST = ss;
            ss.Nav();
            components.Add(ss);
            
            ((AnyBot.Components.System)ss).Initialize(configure);
        }
        /// <summary>
        ///  Запуск основной логики для ВК.
        /// </summary>
        public void VKLogic()
        {
            IsPaused = false;
            VkApi api;

            PlayOrPauseButton(true);
            //Обновление компонентов при старте
            foreach (var component in components)
            {
                Task.Run(() => 
                {
                    component.OnStart();
                });
                
            }

            api = vk.API;
            api.Authorize(new ApiAuthParams { AccessToken = vk.Token });

            bool fatalEx = false;
            while (fatalEx != true)
            {
                //Обновление компонентов при обновлении
                foreach (var component in components) component.OnUpdate();
                try
                {
                    #region Обновление запроса на сервер

                    var s = api.Groups.GetLongPollServer(vk.IDGroup);
                    var poll = api.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams
                    { Server = s.Server, Ts = s.Ts, Key = s.Key, Wait = vk.Delay });

                    #endregion
                    #region Получение и обработка обновлений
                    if (poll?.Updates != null)
                    {
                        foreach (var groupUpdate in poll.Updates)
                            //Обновление компонентов при событии
                            foreach (var component in components) component.OnEvent(groupUpdate);
                    }

                    #endregion

                }
                catch (Exception ex)
                {
                    #region Фатальные ошибки

                    if (ex is VkNet.Exception.UserAuthorizationFailException ||
                        ex is VkNet.Exception.CannotBlacklistYourselfException)
                    {
                        //Обновление компонентов при фатальной ошибке 
                        foreach (var component in components) component.OnFatalException(ex);
                        fatalEx = true;
                    }

                    #endregion
                    #region Обычные ошибки

                    else
                    {
                        //Обновление компонентов при ошибке
                        ConsoleWrite("Бля");
                        foreach (var component in components) component.OnException(ex);
                    }

                    #endregion
                }

                if (IsPaused) break;
            }

            PlayOrPauseButton(false);
        }

        #region GUI

        #region GUI Fields
        private BrushConverter converter = new BrushConverter();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Task.Run(() => 
            {
                LoadData();
                SetupComponents();
                componentW.LoadComponents();
            });
            {
                threadStart = new ThreadStart(VKLogic);
                threadForBot = new Thread(threadStart);
            }
            
        }
        private void wb()
        {
            

            var login = web.Document.GetElementById("musername_");
            var password = web.Document.GetElementById("mpassword_");
            login.SetAttribute("value", "78567");
            password.SetAttribute("value", "123Qwe");

            web.Document.InvokeScript("GoFoo");
            //versionselector(1, 2);
            while(web.Document == null)
            {

            }
            web.Document.InvokeScript("versionselector", new string[] { "1", "2" });
            while (web.Document == null)
            {

            }
            //;
            var doc = web.DocumentText;
            web.Document.InvokeScript("openlerntrectory");
            var can = web.DocumentText;
            Thread.Sleep(3000);
            if (doc == can) MessageBox.Show("nice");
            else MessageBox.Show("nope");

        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            
            //wb();
            if (threadForBot.ThreadState == ThreadState.WaitSleepJoin)
            {
                IsPaused = true;
                Start.Cursor = Cursors.Wait;
            }
            if (threadForBot.ThreadState == ThreadState.Unstarted)
            {
                //SetupComponents();

                threadForBot.Start();

            }
            if (threadForBot.ThreadState == ThreadState.Stopped)
            {
                //SetupComponents();
                threadForBot = new Thread(new ThreadStart(VKLogic));
                threadForBot.Start();
            }
            ssTEST.html = web.DocumentText;
            

        }
        private void settingsMenu(object sender, RoutedEventArgs e)
        {
            if (settings == null)
            {
                settings = new Settings();

            }
            if (settings != null && settings.isClosed == true)
            {
                settings = new Settings();
            }
            settings.Show();
            settings.Focus();
        }
        private void componentMenu(object sender, RoutedEventArgs e)
        {
            if (settings == null)
            {
                componentF = new componentW();

            }
            if (settings != null && settings.isClosed == true)
            {
                componentF = new componentW();
            }
            componentF.Show();
            componentF.Focus();
        }
        private void PlayOrPauseButton(bool State)
        {
            if (State == true)
            {
                Dispatcher.Invoke(() =>
                {
                    Start.Header = "█";
                    var brush = (Brush)converter.ConvertFromString("#CCB00000");
                    Start.Background = brush;
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    Start.Cursor = Cursors.Arrow;

                    var brush = (Brush)converter.ConvertFromString("#CC57B443");
                    Start.Header = "►";
                    Start.Background = brush;
                });
            }
        }
        private void MainWindow1_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Вывод текста в консоль.
        /// </summary>
        /// <param name="Mes">Текст</param>
        private void ConsoleWrite(string Mes)
        {
            this.Dispatcher.Invoke(() =>
            {
                TBConsole.AppendText($"[{DateTime.Now.Hour}:{DateTime.Now.Minute}] {Mes}\n");
            });
        }
        #endregion


    }
}

//#FFABD199
//var u = api.Users.Get(new long[] { (long)a.MessageNew.Message.PeerId });

//this.Dispatcher.Invoke(() =>
//{
//    TBConsole.AppendText($"[{DateTime.Now.Hour}:{DateTime.Now.Minute}] Бот запустился!\n\n");
//});

//Random rnd = new Random();
//api.Messages.Send(new MessagesSendParams
//            {
//                RandomId = rnd.Next(),
//                UserId = userID,
//                Message = message,
//            });