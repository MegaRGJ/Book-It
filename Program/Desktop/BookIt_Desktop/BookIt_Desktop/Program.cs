using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace BookIt_Desktop
{

    static class Program
    {
        private static int attempts;

        public static SplashScreen splashScreen = null;
        public static Login loginScreen = null;
        public static bool connected = false;
        public static bool loggedIn = false;

        

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RunConnectionRoutine();
            Thread.Sleep(200);
            while (!connected) { if (attempts >= 3) break; }
            if (connected) //Stop program if failed to connect to database
            {
                RunLoginRoutine();
                DisposeForm(splashScreen);

                Thread.Sleep(200);
                while (loginScreen.running) { }

                DisposeForm(loginScreen);
                if (loginScreen.LoggedIn) //Stop program if user failed to log in
                {
                    loggedIn = true;
                    Main main = new Main();
                    main.Show();
                    main.Activate();//Run main program if user has logged in
                }
                else Application.Exit(); //No idea why this is needed, somehow makes it work
            }
            else Application.Exit();
        }

        public static void DisposeForm(Form form)
        {
            form.Invoke(new Action(form.Close));
            form.Dispose();
            form = null;
        }

        public static void RunLoginRoutine()
        {
            Thread loginThread = new Thread(new ThreadStart(
                delegate
                {
                    loginScreen = new Login();

                    Application.Run(loginScreen);
                }));
            loginThread.SetApartmentState(ApartmentState.STA);
            loginThread.Start();
        }

        public static void RunConnectionRoutine()
        {
            Thread splashThread = new Thread(new ThreadStart(
                delegate
                {
                    splashScreen = new SplashScreen();

                    Application.Run(splashScreen);
                }));
            splashThread.SetApartmentState(ApartmentState.STA);
            splashThread.Start();
            ConnectToServer();
        }

        static void ConnectToServer()
        {
            //TODO - store this information (encrypted) in a local file, AND need to change log in information to non-superadmin account            
            while (true)
            {
                if (BookItDependancies.Server.ConnectToDatabase("SA", "TSTboKK4A", "127.0.0.1,14333", "BOOKIT") == true)
                {
                    connected = true;
                    break;
                }
                attempts += 1;
                if (attempts > 2)
                {
                    DisplayMessage("Failed to connect to BookIt server, please try again later." + "\n" + "If this problem persits please contact support at our website", "Connection error");
                    break;
                }
            }
        }

        public static void DisplayMessage(string message, string title)
        {
            MessageBox.Show(new Form() { WindowState = FormWindowState.Maximized, TopMost = true }, message, title, MessageBoxButtons.OK);
        }
    }
}
