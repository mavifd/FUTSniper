using DevExpress.XtraEditors;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace FifaSnipe
{
    public partial class MainFrm : DevExpress.XtraEditors.XtraForm
    {
        private CancellationTokenSource _cts = new();
        private EdgeDriver? driver;

        private int BuyCount;
        private int LoopCount;
        private int ErrorCount;

        public MainFrm()
        {
            InitializeComponent();
        }

        private async Task PriceSetupAsync(int currentprice, int minbidval, CancellationToken token) // fiyatı ayarla
        {
            try
            {
                InfoLog($"Fiyat ayarlanıyor...");
                if (token.IsCancellationRequested || driver == null) return;
                int loopcounter = 0;
                while (loopcounter < processtimer.Value)
                {
                    System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> priceInput = driver.FindElements(By.ClassName("ut-number-input-control"));
                    if (priceInput.Count != 0)
                    {
                        if (minbidval != 0)
                        {
                            priceInput[2].Clear();
                            priceInput[2].SendKeys(minbidval.ToString());
                            return;
                        }

                        priceInput[2].Clear();
                        priceInput[2].SendKeys(currentprice.ToString());

                        SuccessLog($"Fiyat ayarlama başarılı.");
                        return;
                    }
                    await Task.Delay(100, token);
                    loopcounter++;
                }
                ErrorLog($"Fiyat ayarlama başarısız.");
            }
            catch (Exception ex)
            {
                ErrorLog($"Fiyat ayarlama başarısız. (Genel Hata)\nDetay:{ex}");
                ErrorCount++;
            }
            return;
        }

        private async Task BackPageActionAsync(CancellationToken token) //arama sayfasına dön
        {
            try
            {
                InfoLog($"Arama sayfasına dönülüyor...");
                if (token.IsCancellationRequested || driver == null) return;
                int loopcounter = 0;
                while (loopcounter < processtimer.Value)
                {
                    IWebElement? backPgs = driver.FindElements(By.TagName("h1")).FirstOrDefault(header => header.Text == "Search Results" && header.GetAttribute("class") == "title");
                    if (backPgs != null)
                    {
                        IWebElement? backPgsBtn = driver.FindElement(By.ClassName("ut-navigation-button-control"));
                        if (backPgsBtn != null)
                        {
                            backPgsBtn.Click();
                            SuccessLog($"Arama sayfasına dönme başarılı.");
                            return;
                        }
                    }
                    await Task.Delay(100, token);
                    loopcounter++;
                }
                ErrorLog($"Arama sayfasına dönme başarısız.");
            }
            catch (Exception ex)
            {
                ErrorLog($"Arama sayfasına dönme başarısız.\nDetay:{ex}");
                ErrorCount++;
            }
            return;
        }

        private async Task SearchActionAsync(CancellationToken token) //arama yap
        {
            try
            {
                InfoLog($"Arama yapılıyor...");
                if (token.IsCancellationRequested || driver == null) return;
                int loopcounter = 0;
                while (loopcounter < processtimer.Value)
                {
                    IWebElement? searchButton = driver.FindElements(By.TagName("button")).FirstOrDefault(header => header.Text == "Search");
                    if (searchButton != null)
                    {
                        searchButton.Click();
                        SuccessLog($"Arama yapıldı.");
                        return;
                    }
                    await Task.Delay(100, token);
                    loopcounter++;
                }
                ErrorLog($"Arama başarısız."); //hata 6
            }
            catch (Exception ex)
            {
                InfoLog($"Arama başarısız.\nDetay:{ex}"); //hata 7
                ErrorCount++;
            }
            return;
        }

        private async Task BuyFunctionAsync(CancellationToken token) //sonuçları kontrol et ve satın al
        {
            InfoLog($"Arama sonuçları kontrol ediliyor...");
            if (token.IsCancellationRequested || driver == null) return;
            int resultCheckCounter = 0;
            bool buyBtnCheck = false;
            bool noResultCheck = false;
            while (resultCheckCounter < processtimer.Value)
            {
                try
                {
                    IWebElement? buyBtn = driver.FindElements(By.TagName("button")).FirstOrDefault(el => el.Text.Contains("Buy Now for"));
                    if (buyBtn != null)
                    {
                        buyBtn.Click();
                        buyBtnCheck = true;
                        break;
                    }
                    IWebElement? noResult = driver.FindElements(By.TagName("section")).FirstOrDefault(el => el.Text.Contains("No results"));
                    if (noResult != null)
                    {
                        noResultCheck = true;
                        break;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    InfoLog("Öğe güncellenmiş, tekrar deneniyor... (Sorun yok.)");
                }
                catch (Exception ex)
                {
                    ErrorLog($"Sonuç kontrol başarısız.\nDetay:{ex}");
                }
                await Task.Delay(100, token);
                resultCheckCounter++;
            }
            if (buyBtnCheck) SuccessLog($"Satın alma bulundu.");
            else if (noResultCheck) { InfoLog($"Sonuç yok."); return; }
            InfoLog($"Satın alma onaylanıyor...");
            if (token.IsCancellationRequested) return;
            try
            {
                int confirmBtnCounter = 0;
                bool success = false;
                while (confirmBtnCounter < processtimer.Value)
                {
                    IWebElement? searchButton = driver.FindElements(By.TagName("button")).FirstOrDefault(header => header.Text == "Ok");
                    if (searchButton != null)
                    {
                        searchButton.Click();
                        success = true;
                        break;
                    }
                    await Task.Delay(100, token);
                    confirmBtnCounter++;
                }
                if (success)
                {
                    SuccessLog($"Satın alma onaylandı!");
                    BuyCount++;
                    InfoLog($"EAFC Sunucu güncellemesi için 25 saniye bekleniyor...");
                    await Task.Delay((int)(buycdval.Value * 1000), token);
                }
                else ErrorLog("Onay kutusu yok.");
            }
            catch (Exception ex)
            {
                ErrorLog($"Satın alma onay kontrolü başarısız.\nDetay:{ex}");
                ErrorCount++;
            }
        }

        private async Task MainFunctionAsync(CancellationToken token)
        {
            InfoLog($"Başlatılıyor...");

            var processes = Process.GetProcessesByName("msedgedriver");
            foreach (var process in processes)
            {
                try
                {
                    process.Kill();
                    process.WaitForExit();
                    InfoLog($"Eski sürücü sonlandırıldı: {process.Id}");
                }
                catch (Exception ex)
                {
                    InfoLog($"İşlem sonlandırılamadı (Sürücü): {process.Id}, Hata: {ex.Message}");
                }
            }

            var options = new EdgeOptions
            {
                DebuggerAddress = "127.0.0.1:9111"
            };

            InfoLog($"Başlatılıyor... DRV");
            driver = new EdgeDriver(options);
            InfoLog($"Başlatılıyor... DRV SCS");


            foreach (var handle in driver.WindowHandles)
            {
                driver.SwitchTo().Window(handle);
                if (driver.Title.Contains("FC Ultimate"))
                    break;
            }

            InfoLog("Attach işlemi tamam.");

            InfoLog($"Sayfa: {driver.Title}");

            //IReadOnlyCollection<IWebElement> containers = driver.FindElements(By.CssSelector("*"));
            //foreach (IWebElement element in containers) { InfoLog($"Tag: {element.TagName} | Text: {element.Text} | Class: {element.GetAttribute("class")}"); }

            int currentprice = int.Parse(maxpriceTxt.Text);
            int increment = int.Parse(changeTxt.Text);
            int maxvalue = int.Parse(maxchangeTxt.Text);
            int minbidval = 0;
            int loopcount = 0;
            int cdloopcount = 0;

            while (true)
            {
                if (driver == null)
                {
                    InfoLog($"Driver kapalı, baştan başlatın.");
                    break;
                }

                InfoLog($"Mevcut Fiyat: {currentprice}.");
                InfoLog($"Döngü Sayısı: {loopcount}.");

                if (cdloopcount == targetloopval.Value)
                {
                    InfoLog($"Döngü molası başladı. {loopcdval.Value} saniye.");
                    await Task.Delay((int)(loopcdval.Value * 1000), token);
                    cdloopcount = 0;
                }

                if (loopcount != 0)
                {
                    if (currentprice <= maxvalue) currentprice += increment;
                    if (loopcount % 2 == 0) minbidval = 150;
                    else minbidval = 0;
                }

                await PriceSetupAsync(currentprice, minbidval, token);

                await SearchActionAsync(token);

                await BuyFunctionAsync(token);

                await BackPageActionAsync(token);

                loopcount++;
                cdloopcount++;
                LoopCount = loopcount;

                InfoLog($"Hız Limiti Koruması - {ratelimitval.Value / 1000} Saniye Bekleme");

                await Task.Delay((int)(ratelimitval.Value), token);

                if (token.IsCancellationRequested) break;
            }

            driver?.Quit();
            InfoLog($"İşlem sonu.", System.Drawing.Color.DarkMagenta);
        }

        private void StatUpdater_Tick(object sender, EventArgs e)
        {
            UpdateCounts();
            CheckErrorRate();
        }

        private void UpdateCounts()
        {
            UpdateText(buycountTxt, BuyCount.ToString());
            UpdateText(loopcountTxt, LoopCount.ToString());
            UpdateText(errorcountTxt, ErrorCount.ToString());
        }

        private void SuccessLog(string message)
        {
            string text = $"[{DateTime.Now:dd/MM HH:mm}] {message}{Environment.NewLine}";
            if (LoggerTXT.InvokeRequired) LoggerTXT.Invoke(new Action(() => { AppendColoredText(text, System.Drawing.Color.Green); }));
            else AppendColoredText(text, System.Drawing.Color.Green);
        }

        private static void UpdateText(Control control, string text)
        {
            if (control.InvokeRequired) control.Invoke(new Action(() => { control.Text = text; }));
            else control.Text = text;
        }

        private void ErrorLog(string message)
        {
            errorTimestamps.Add(DateTime.Now);
            try
            {
                string currentDate = DateTime.Now.ToString("dd_MM");
                string fileName = $"{currentDate}_errimage.png";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            }
            catch (Exception)
            {
                InfoLog("Resim yükleme hatası...");
            }

            string text = $"[{DateTime.Now:dd/MM HH:mm}] {message}{Environment.NewLine}";
            if (LoggerTXT.InvokeRequired) LoggerTXT.Invoke(new Action(() => { AppendColoredText(text, System.Drawing.Color.Red); }));
            else AppendColoredText(text, System.Drawing.Color.Red);
        }

        private void InfoLog(string message, System.Drawing.Color? color = null)
        {
            string text = $"[{DateTime.Now:dd/MM HH:mm}] {message}{Environment.NewLine}";
            if (LoggerTXT.InvokeRequired) LoggerTXT.Invoke(new Action(() => { AppendColoredText(text, color ?? LoggerTXT.ForeColor); }));
            else AppendColoredText(text, color ?? LoggerTXT.ForeColor);
        }

        private void AppendColoredText(string text, System.Drawing.Color color)
        {
            int start = LoggerTXT.TextLength;
            LoggerTXT.AppendText(text);
            int end = LoggerTXT.TextLength;
            LoggerTXT.Select(start, end - start);
            LoggerTXT.SelectionColor = color;
            LoggerTXT.SelectionLength = 0;
            LoggerTXT.SelectionColor = LoggerTXT.ForeColor;
            LoggerTXT.Focus();
        }

        private List<DateTime> errorTimestamps = [];

        private void CheckErrorRate()
        {
            DateTime now = DateTime.Now;
            errorTimestamps = [.. errorTimestamps.Where(timestamp => (now - timestamp).TotalMinutes <= 1)];
            if (errorTimestamps.Count >= 5)
            {
                ErrorLog("Son 1 dakika içinde 5'den fazla hata oluştu. Program durduruluyor.");
                _cts?.Cancel();
            }
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            InfoLog($"SnipeBot.", System.Drawing.Color.Blue);
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            InfoLog($"İşlem başlatıldı.", System.Drawing.Color.BurlyWood);
            StartBtn.Enabled = false;
            StopBtn.Enabled = true;

            maxpriceTxt.Enabled = false;
            changeTxt.Enabled = false;
            maxchangeTxt.Enabled = false;

            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            _ = Task.Run(async () => await MainFunctionAsync(token));
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            StartBtn.Enabled = true;
            StopBtn.Enabled = false;

            maxpriceTxt.Enabled = true;
            changeTxt.Enabled = true;
            maxchangeTxt.Enabled = true;

            InfoLog($"İşlem durdurma komutu verildi.", System.Drawing.Color.DarkSalmon);
            _cts?.Cancel();
        }

        private void PrepareEdge_Click(object sender, EventArgs e)
        {
            var processes = Process.GetProcessesByName("msedge");
            foreach (var process in processes)
            {
                try
                {
                    process.Kill();
                    process.WaitForExit();
                    InfoLog($"İşlem sonlandırıldı: {process.Id}");
                }
                catch (Exception ex)
                {
                    InfoLog($"İşlem sonlandırılamadı: {process.Id}, Hata: {ex.Message}");
                }
            }
            try
            {
                ProcessStartInfo psi = new()
                {
                    FileName = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                    Arguments = "--remote-debugging-port=9111",
                    UseShellExecute = false
                };
                Process.Start(psi);
                InfoLog("msedge.exe başarıyla başlatıldı.");
            }
            catch (Exception ex)
            {
                InfoLog($"msedge.exe başlatılamadı. Hata: {ex.Message}");
            }
        }

        private void LayoutControlGroup3_DoubleClick(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("Sayacı sıfırlamak istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                BuyCount = 0;
                LoopCount = 0;
                ErrorCount = 0;
            }
        }

        private void Topmostcb_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = topmostcb.Checked;
        }
    }
}