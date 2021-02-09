using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizuAutomation.Core
{
    public class WebDriverExecucao
    {


        #region GanharNoInsta

        public static void ExecuteGanharNoInsta(Dizu dizu, Task task)
        {

            if (dizu != null && dizu.Contas != null)
            {
                var ListaContas = dizu.Contas.ToList();
                foreach (var Conta in ListaContas)
                {
                    //Esperar(60000);
                    var chromeOptions = new ChromeOptions();
                    //chromeOptions.AddArguments("headless");
                    //chromeOptions.AddArguments(new List<string>() { "no-sandbox", "headless" });
                    //var driver = new ChromeDriver(Directory.GetCurrentDirectory(), chromeOptions);
                    var options = new ChromeOptions();
                    options.AddArguments(new List<string>() { "no-sandbox", "headless" });
                    var driver = new RemoteWebDriver(new Uri("http://localhost:4444/"), options);
                    var waiter = new WebDriverWait(driver, new TimeSpan(0, 0, 60));
                    
                    switch (Conta.Tipo)
                    {
                        case "instagram":
                            //ExecutarInstagramGanhar(driver, waiter, Conta, dizu.Usuario, dizu.Senha);
                            ExecutarInstagramGanhar(driver, waiter, Conta, dizu.Usuario, dizu.Senha).Start();
                            break;

                        default:
                            break;
                    }
                }
            }

        }
        private static async Task ExecutarInstagramGanhar(RemoteWebDriver driver, WebDriverWait waiter, DizuConta conta, string dizuUsuario, string dizuSenha)
        {
            await LogarInsta(driver, waiter, conta.Usuario, conta.Senha);

            await Esperar(2000);
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            driver.SwitchTo().Window(driver.WindowHandles.Last());

            LogarGanharNoInsta(driver, waiter, dizuUsuario, dizuSenha);


            Navegar(driver, waiter, "https://www.ganharnoinsta.com/painel/?pagina=sistema");

            waiter.Until(ExpectedConditions.ElementToBeClickable(By.Id("contaig")));
            driver.FindElement(By.Id("contaig")).Click();


            var ContasDisponiveis = new SelectElement(driver.FindElement(By.Id("contaig")));
            ContasDisponiveis.SelectByText(conta.Usuario);


            driver.FindElement(By.Id("btn_iniciar")).Click();
            await Esperar(1000);
            int NroDeFollows = 0;
            int infinite = 1;
            while (infinite == 1)
            {
                if ((NroDeFollows % 30) == 0 && NroDeFollows != 0)
                {
                    await Esperar(300000);
                }

                var btnTarefaExiste = ExisteElementoPeloId(driver, "tarefa");
                if (await btnTarefaExiste)
                {
                    if(await ExisteElemento(driver,"#tarefa > b"))
                    {
                        string tipo = driver.FindElement(By.CssSelector("#tarefa > b")).Text;
                        //Console.WriteLine($"Tarefa Encontrada  {DateTime.Now}");

                        driver.FindElement(By.Id("btn-acessar")).Click();
                        driver.SwitchTo().Window(driver.WindowHandles.Last());
                        bool Fez = false;
                        if (tipo.Contains("Seguir"))
                        {
                            await SeguirInstagram(driver, waiter);
                            Fez = true;
                        }

                        if (tipo.Contains("Curtir"))
                        {
                            await CurtirInstagram(driver);
                            Fez = true;
                        }

                        driver.SwitchTo().Window(driver.WindowHandles.Last());
                        var btnConfirmar = ExisteElementoPeloId(driver, "btn-confirmar");
                        if (await btnConfirmar && Fez)
                        {
                            driver.FindElementById("btn-confirmar").Click();
                            //Console.WriteLine($"Cliquei em confirmar {DateTime.Now} - {conta.Usuario}");
                            NroDeFollows++;
                            Console.WriteLine($"Realizado {NroDeFollows} - {conta.Usuario}");
                        }
                        else
                        {
                            await Esperar(5000);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Sem Tarefas -Ganhar pelo Insta - {conta.Usuario}");
                    await Esperar(180000);
                }
                await Esperar(1000);
                //}
                //else
                //    Esperar(5000);
            }

        }

        private static void LogarGanharNoInsta(RemoteWebDriver driver, WebDriverWait waiter, string Usuario, string Senha)
        {
            try
            {
                Navegar(driver, waiter, "https://www.ganharnoinsta.com/painel/");


                driver.FindElement(By.Id("uname")).SendKeys(Usuario);
                driver.FindElement(By.Id("pwd")).SendKeys(Senha);
                driver.FindElement(By.CssSelector(@"body > div.main-wrapper > div > div > div.col-lg-5.col-md-7.bg-white > div > form > div > div:nth-child(3) > button")).Click();

                waiter.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#main-wrapper > div.page-wrapper > div > div > div > h3")));

            }
            catch (Exception)
            {
            }


        }


        #endregion

        #region Dizu
        public static async Task<List<Task>> ExecuteDizu(Dizu dizu)
        {
            var tasks = new List<Task>();
            if (dizu != null && dizu.Contas != null)
            {
                var ListaContas = dizu.Contas.ToList();
                foreach (var Conta in ListaContas)
                {
                    var options = new ChromeOptions();

                    var driver = new RemoteWebDriver(new Uri("http://192.168.15.15:4444/wd/hub"), options);
                    var waiter = new WebDriverWait(driver, new TimeSpan(0, 2, 60));
                    switch (Conta.Tipo)
                    {
                        case "instagram":
                            try
                            {
                               tasks.Add(ExecutarInstagram(driver, waiter, Conta, dizu.Usuario, dizu.Senha));
                               Console.WriteLine("Proximo");
                            }
                            catch (Exception ex)
                            {

                            }
                            //ExecutarInstagram(driver, waiter, Conta, dizu.Usuario, dizu.Senha);

                            break;

                        default:
                            break;
                    }
                }
                
                
            }
            return tasks;



        }
        private static async Task ExecutarInstagram(RemoteWebDriver driver, WebDriverWait waiter, DizuConta conta, string dizuUsuario, string dizuSenha)
        {
            try
            {
                
                await LogarInsta(driver, waiter, conta.Usuario, conta.Senha);

                await Esperar(2000);
                ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
                driver.SwitchTo().Window(driver.WindowHandles.Last());
                LogarNoDizu(driver, waiter, dizuUsuario, dizuSenha);

                int NroDeFollows = 0;
                int infinite = 1;
               
                while (infinite == 1)
                {
                    //Console.WriteLine($"Verificando tarefas - {DateTime.Now}");
                    if ((NroDeFollows % 30) == 0 && NroDeFollows != 0)// Tempo de espera para nao tomar ban
                    {
                        await Esperar(300000);
                    }


                    Navegar(driver, waiter, "https://dizu.com.br/painel/conectar");

                    waiter.Until(ExpectedConditions.ElementToBeClickable(By.Id("instagram_id")));
                    driver.FindElement(By.Id("instagram_id")).Click();


                    var ContasDisponiveis = new SelectElement(driver.FindElement(By.Id("instagram_id")));
                    ContasDisponiveis.SelectByText(conta.Usuario);


                    driver.FindElement(By.Id("curtida05")).Click();
                    driver.FindElement(By.Id("iniciarTarefas")).Click();


                    await Esperar(1000);
                    
                    var btnTarefaExiste = ExisteElemento(driver, "#conectar_form > div.btn_site > div.btn-dizu.bg-white.sombra");
                    if (await btnTarefaExiste)
                    {
                        string tipo = driver.FindElement(By.CssSelector("body > div.container-fluid > div > div.content-body > div:nth-child(4) > div > div.row > div.tarefasLista > div.box_user.tarefa > div.btn_site > div > p")).Text;
                        Console.WriteLine($"Tarefa Encontrada  {DateTime.Now}");

                        driver.FindElement(By.CssSelector("#conectar_form > div.btn_site > div.btn-dizu.bg-white.sombra")).Click();
                        driver.SwitchTo().Window(driver.WindowHandles.Last());
                        bool Fez = false;
                        switch (tipo.ToLower())
                        {
                            case "seguir":
                                await SeguirInstagram(driver, waiter);
                                Fez = true;
                                break;
                            case "curtir":
                                await CurtirInstagram(driver);
                                Fez = true;
                                break;
                            default:
                                break;
                        }
                        driver.SwitchTo().Window(driver.WindowHandles.Last());
                        var btnConfirmar = ExisteElemento(driver, "#conectar_step_5 > button > p");
                        if (await btnConfirmar && Fez)
                        {
                            driver.FindElementByCssSelector("#conectar_step_5 > button > p").Click();
                            //Console.WriteLine($"Cliquei em confirmar {DateTime.Now} - {conta.Usuario}");
                            NroDeFollows++;
                            Console.WriteLine($"Realizado {NroDeFollows} - {conta.Usuario}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Sem Tarefas - {conta.Usuario}");
                        await Esperar(180000);
                    }
                    await Esperar(2000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void LogarNoDizu(RemoteWebDriver driver, WebDriverWait waiter, string Usuario, string Senha)
        {
            driver.Navigate().GoToUrl("https://dizu.com.br/login");
            waiter.Until(ExpectedConditions.UrlToBe("https://dizu.com.br/login"));

            driver.FindElement(By.Id("login")).SendKeys(Usuario);
            driver.FindElement(By.Id("senha")).SendKeys(Senha);
            driver.FindElement(By.CssSelector(@"#FormLogin > div.btn_site.col-sm-12.col-xs-12.col-lg-12 > button")).Click();

            waiter.Until(ExpectedConditions.UrlToBe("https://dizu.com.br/painel"));
        }

        #endregion
        #region Core

        public static async Task ExecuteAutomation(Settings settings)
        {
            var tasks = new List<Task>();
            //ExecuteGanharNoInsta(settings.GanharNoInsta);
            tasks.AddRange(await ExecuteDizu(settings.Dizu));
            Task.WaitAll(tasks.ToArray());
        }


        private static async Task<bool> LogarInsta(RemoteWebDriver driver, WebDriverWait waiter, string Usuario, string Senha)
        {
            try
            {
                Console.WriteLine($"Logando com {Usuario}");
                Navegar(driver, waiter, "https://www.instagram.com/");
                await Esperar(1000);
                var result = ExisteElemento(driver, "#loginForm > div > div:nth-child(1) > div > label > input");
                driver.FindElement(By.Name("username")).SendKeys(Usuario);
                driver.FindElement(By.Name("password")).SendKeys(Senha);
                driver.FindElement(By.CssSelector(@"#loginForm > div > div:nth-child(3)")).Click();
                await Esperar(6000);
                //int codeI = 0;
                //while (driver.Url.Contains("two_factor"))
                //{
                //    waiter.Until(ExpectedConditions.ElementToBeClickable(By.Name("verificationCode")));
                //    for (int i = 0; i < 8; i++)
                //        driver.FindElement(By.Name("verificationCode")).SendKeys(Keys.Backspace);

                //    driver.FindElement(By.Name("verificationCode")).SendKeys(conta.Codigos_Seguranca[codeI]);
                //    codeI++;
                //    driver.FindElement(By.CssSelector(@"#react-root > section > main > div > div > div:nth-child(1) > div > form > div.Igw0E.IwRSH.eGOV_._4EzTm.MGdpg.CovQj.jKUp7.iHqQ7 > button")).Click();
                //    Esperar(6000);
                //}
                Console.WriteLine($"Logado - no Instagram {Usuario}");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        private static void Navegar(RemoteWebDriver driver, WebDriverWait waiter, string Site)
        {
            driver.Navigate().GoToUrl(Site);
            waiter.Until(ExpectedConditions.UrlToBe(Site));
        }

        private static async Task Esperar(int miliseconds)
        {    
            await Task.Delay(miliseconds);
        }

        private static async Task<bool> ExisteElemento(RemoteWebDriver driver, string Path)
        {
            bool result = false;
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    await Esperar(3000);
                    IWebElement e = driver.FindElement(By.CssSelector(Path));
                    if (e.Size.IsEmpty)
                        result = false;
                    else
                    {
                        result = true;
                        break;
                    }
                }
                catch (Exception ex) { result = false; }


            }
            
            return result;
        }
        private static async Task<bool> ExisteElementoXPTH(RemoteWebDriver driver, string Path)
        {
            try
            {
                bool result = false;
                for (int i = 0; i < 10; i++)
                {
                    await Esperar(3000);

                    IWebElement e = driver.FindElement(By.XPath(Path));
                    if (e.Size.IsEmpty)
                        result = false;
                    else
                    {
                        result = true;
                        break;
                    }

                }

                return result;
            }
            catch (Exception ex) { return false; }
        }

        private static async Task<bool> ExisteElementoPeloText(RemoteWebDriver driver, string texto)
        {
            try
            {
                bool result = false;
                for (int i = 0; i < 10; i++)
                {

                    IWebElement e = driver.FindElement(By.XPath($"//*[contains(text(),'{texto}')]"));
                    if (e == null)
                        result = false;
                    else
                    {
                        result = true;
                        break;
                    }

                    await Esperar(1000);
                }

                return result;
            }
            catch (Exception ex) { return false; }
        }


        private static async Task<bool> ExisteElementoPeloId(RemoteWebDriver driver, string texto)
        {
            bool result = false;
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    IWebElement e = driver.FindElement(By.XPath($"//*[contains(text(),'{texto}')]"));
                    if (e == null)
                        result = false;
                    else
                    {
                        result = true;
                        break;
                    }

                    await Esperar(1000);
                    return result;
                }
                catch (Exception ex)
                {
                    result = false;
                    await Esperar(3000);
                }

            }
            return result;
        }
        #endregion


        #region Instagram
        private static async Task SeguirInstagram(RemoteWebDriver driver, WebDriverWait waiter)
        {
            await Esperar(4000);
            var btnFollow = ExisteElemento(driver, "#react-root > section > main > div > header > section > div.nZSzR > div.Igw0E.IwRSH.eGOV_.ybXk5._4EzTm > div > div > div > span > span.vBF20._1OSdk > button");
            if (await btnFollow)
            {
                //await Esperar(1000);
                //waiter.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//*[contains(text(),'Follow')]")));
                driver.FindElement(By.CssSelector("#react-root > section > main > div > header > section > div.nZSzR > div.Igw0E.IwRSH.eGOV_.ybXk5._4EzTm > div > div > div > span > span.vBF20._1OSdk > button")).Click();
                //Console.WriteLine("Botao de Follow Dado");
               



            }
            await Esperar(2000);
            driver.Close();
        }

        private static async Task CurtirInstagram(RemoteWebDriver driver)
        {
            var btnCurtir = ExisteElemento(driver, "#react-root > section > main > div > div > article > div.eo2As > section.ltpMr.Slqrh > span.fr66n > button");
            if (await btnCurtir)
            {
                driver.FindElementByCssSelector("#react-root > section > main > div > div > article > div.eo2As > section.ltpMr.Slqrh > span.fr66n > button").Click();
                //Console.WriteLine("Botao de Follow Dado");
                await Esperar(1000);

            }
            driver.Close();
        }

        #endregion


    }
}
